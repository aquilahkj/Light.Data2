using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace Light.Data.Postgre
{
    internal class PostgreCommandFactory : CommandFactory
    {
        public override bool SupportBatchInsertIdentity => true;

        private readonly DateTimeFormater dateTimeFormater = new DateTimeFormater();

        private const string defaultDateTime = "YYYY-MM-DD HH:MI:SS";

        public PostgreCommandFactory()
        {
            _strictMode = true;
            dateTimeFormater.YearFormat = "YYYY";
            dateTimeFormater.MonthFormat = "MM";
            dateTimeFormater.DayFormat = "DD";
            dateTimeFormater.HourFormat = "HH";
            dateTimeFormater.MinuteFormat = "MI";
            dateTimeFormater.SecondFormat = "SS";
        }

        public override CommandData CreateTruncateTableCommand(DataTableEntityMapping mapping, CreateSqlState state)
        {
            var data = base.CreateTruncateTableCommand(mapping, state);
            if (mapping.IdentityField != null)
            {
                var restartSeq = $"alter sequence \"{GetIdentitySeq(mapping, state)}\" restart;";
                data.CommandText += restartSeq;
            }

            return data;
        }

        public override string CreateBooleanQuerySql(object fieldName, bool isTrue)
        {
            return $"{fieldName}={(isTrue ? "true" : "false")}";
        }

        public override string CreateDataFieldSql(string fieldName)
        {
            if (_strictMode)
            {
                return $"\"{fieldName}\"";
            }
            else
            {
                return base.CreateDataFieldSql(fieldName);
            }
        }

        public override string CreateDataTableSql(string tableName)
        {
            if (_strictMode)
            {
                return $"\"{tableName}\"";
            }
            else
            {
                return base.CreateDataTableSql(tableName);
            }
        }

        public override string CreateDividedSql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"({field}::float/{value})";
            }
            else
            {
                return $"({value}/{field}::float)";
            }
        }

        public override CommandData CreateSelectBaseCommand(DataEntityMapping mapping, string customSelect,
            QueryExpression query, OrderExpression order, Region region, CreateSqlState state) //, bool distinct)
        {
            var command = base.CreateSelectBaseCommand(mapping, customSelect, query, order, region, state);
            if (region != null)
            {
                command.CommandText = region.Start == 0
                    ? $"{command.CommandText} limit {region.Size}"
                    : $"{command.CommandText} limit {region.Size} offset {region.Start}";

                command.InnerPage = true;
            }

            return command;
        }

        public override CommandData CreateSelectJoinTableBaseCommand(string customSelect, List<IJoinModel> modelList,
            QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            var command = base.CreateSelectJoinTableBaseCommand(customSelect, modelList, query, order, region, state);
            if (region != null)
            {
                command.CommandText = region.Start == 0
                    ? $"{command.CommandText} limit {region.Size}"
                    : $"{command.CommandText} limit {region.Size} offset {region.Start}";
                command.InnerPage = true;
            }

            return command;
        }

        public override CommandData CreateAggregateTableCommand(DataEntityMapping mapping, AggregateSelector selector,
            AggregateGroupBy groupBy, QueryExpression query, QueryExpression having, OrderExpression order,
            Region region, CreateSqlState state)
        {
            var command =
                base.CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order, region, state);
            if (region != null)
            {
                command.CommandText = region.Start == 0
                    ? $"{command.CommandText} limit {region.Size}"
                    : $"{command.CommandText} limit {region.Size} offset {region.Start}";

                command.InnerPage = true;
            }

            return command;
        }

        public override CommandData CreateBaseInsertCommand(DataTableEntityMapping mapping, object entity, bool refresh,
            bool updateIdentity, CreateSqlState state)
        {
            var command = base.CreateBaseInsertCommand(mapping, entity, refresh, false, state);
            if (updateIdentity && mapping.IdentityField != null)
            {
                var idenSql = $"returning {CreateDataFieldSql(mapping.IdentityField.Name)}";
                command.CommandText += idenSql;
                command.IdentitySql = true;
            }

            return command;
        }

        public override CommandData CreateBatchInsertWithIdentityCommand(DataTableEntityMapping mapping, IList entitys,
            bool refresh, CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0)
            {
                throw new ArgumentNullException(nameof(entitys));
            }

            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            var insertLen = fields.Count;
            if (insertLen == 0)
            {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            if (!mapping.HasIdentity)
            {
                throw new LightDataException(string.Format(SR.NoIdentityField, mapping.ObjectType));
            }

            string insertSql = null;
            string cacheKey = null;
            if (state.Seed == 0)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_batchInsertCache.TryGetCommand(cacheKey, out var cache))
                {
                    insertSql = cache;
                }
            }

            if (insertSql == null)
            {
                var insertList = new string[insertLen];
                for (var i = 0; i < insertLen; i++)
                {
                    var field = fields[i];
                    insertList[i] = CreateDataFieldSql(field.Name);
                }

                var insert = string.Join(",", insertList);
                insertSql = $"insert into {CreateDataTableMappingSql(mapping, state)}({insert})";
                if (cacheKey != null)
                {
                    _batchInsertCache.SetCommand(cacheKey, insertSql);
                }
            }

            var totalSql = new StringBuilder();

            totalSql.Append($"{insertSql}values");
            var cur = 0;
            var end = entitys.Count;
            foreach (var entity in entitys)
            {
                var valuesList = new string[insertLen];
                for (var i = 0; i < insertLen; i++)
                {
                    var field = fields[i];
                    var value = field.ToInsert(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }

                var values = string.Join(",", valuesList);
                totalSql.Append($"({values})");
                cur++;
                if (cur < end)
                {
                    totalSql.Append(',');
                }
                else
                {
                    totalSql.Append($"returning {CreateDataFieldSql(mapping.IdentityField.Name)} as id;");
                }
            }

            var command = new CommandData(totalSql.ToString());
            return command;
        }

        public override CommandData CreateBatchInsertCommand(DataTableEntityMapping mapping, IList entitys,
            bool refresh, CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0)
            {
                throw new ArgumentNullException(nameof(entitys));
            }

            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            var insertLen = fields.Count;
            if (insertLen == 0)
            {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            string insertSql = null;
            string cacheKey = null;
            if (state.Seed == 0)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_batchInsertCache.TryGetCommand(cacheKey, out var cache))
                {
                    insertSql = cache;
                }
            }

            if (insertSql == null)
            {
                var insertList = new string[insertLen];
                for (var i = 0; i < insertLen; i++)
                {
                    var field = fields[i];
                    insertList[i] = CreateDataFieldSql(field.Name);
                }

                var insert = string.Join(",", insertList);
                insertSql = $"insert into {CreateDataTableMappingSql(mapping, state)}({insert})";
                if (cacheKey != null)
                {
                    _batchInsertCache.SetCommand(cacheKey, insertSql);
                }
            }

            var totalSql = new StringBuilder();

            totalSql.Append($"{insertSql}values");
            var cur = 0;
            var end = entitys.Count;
            foreach (var entity in entitys)
            {
                var valuesList = new string[insertLen];
                for (var i = 0; i < insertLen; i++)
                {
                    var field = fields[i];
                    var value = field.ToInsert(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }

                var values = string.Join(",", valuesList);
                totalSql.Append($"({values})");
                cur++;
                totalSql.Append(cur < end ? ',' : ';');
            }

            var command = new CommandData(totalSql.ToString());
            return command;
        }

        public override string CreateCollectionParamsQuerySql(object fieldName, QueryCollectionPredicate predicate,
            IEnumerable<string> list)
        {
            if (predicate == QueryCollectionPredicate.In || predicate == QueryCollectionPredicate.NotIn)
            {
                return base.CreateCollectionParamsQuerySql(fieldName, predicate, list);
            }

            var op = GetQueryCollectionPredicate(predicate);

            var i = 0;
            var sb = new StringBuilder();
            sb.Append($"{fieldName} {op} (");
            foreach (var item in list)
            {
                if (i > 0)
                    sb.Append(" union all ");
                sb.Append($"select {item}");
                i++;
            }

            sb.Append(")");
            return sb.ToString();
        }

        public override string CreateRandomOrderBySql(DataEntityMapping mapping, string aliasName, bool fullFieldName)
        {
            return "random()";
        }

        public override string CreateIdentitySql(DataTableEntityMapping mapping, CreateSqlState state)
        {
            if (mapping.IdentityField != null)
            {
                return $"select currval('\"{GetIdentitySeq(mapping, state)}\"');";
            }

            return string.Empty;
        }

        private static string GetIdentitySeq(DataTableEntityMapping mapping, CreateSqlState state)
        {
            if (mapping.IdentityField == null)
            {
                throw new LightDataException(SR.NoIdentityField);
            }

            string seq;
            var postgreIdentity = mapping.ExtentParams.GetParamValue("PostgreIdentitySeq");
            if (!string.IsNullOrEmpty(postgreIdentity))
            {
                seq = postgreIdentity;
            }
            else
            {
                if (!state.TryGetAliasTableName(mapping, out var name))
                {
                    name = mapping.TableName;
                }

                seq = $"{name}_{mapping.IdentityField.Name}_seq";
            }

            return seq;
        }

        public override string CreateMatchSql(object field, bool starts, bool ends)
        {
            var sb = new StringBuilder();
            if (starts)
            {
                sb.Append($"'{Wildcards}'||");
            }

            sb.Append(field);
            if (ends)
            {
                sb.Append($"||'{Wildcards}'");
            }

            return sb.ToString();
        }

        public override string CreateBooleanQuerySql(object field, bool isTrue, bool isEqual, bool isReverse)
        {
            if (!isReverse)
            {
                return $"{field}{(isEqual ? "=" : "!=")}{(isTrue ? "true" : "false")}";
            }
            else
            {
                return $"{(isTrue ? "true" : "false")}{(isEqual ? "=" : "!=")}{field}";
            }
        }

        public override string CreateConcatSql(params object[] values)
        {
            var value1 = string.Join("||", values);
            var sql = $"({value1})";
            return sql;
        }

        public override string CreateDualConcatSql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"({field}||{value})";
            }
            else
            {
                return $"({value}||{field})";
            }
        }

        public override string CreateDateSql(object field)
        {
            return $"date({field})";
        }

        public override string CreateDateTimeFormatSql(object field, string format)
        {
            var sqlFormat = string.IsNullOrEmpty(format) ? defaultDateTime : dateTimeFormater.FormatData(format);

            return $"to_char({field},'{sqlFormat}')";
        }

        public override string CreateTruncateSql(object field)
        {
            return $"trunc({field}::numeric)";
        }

        public override string CreateLogSql(object field)
        {
            return $"ln({field}::numeric)";
        }

        public override string CreateLogSql(object field, object value)
        {
            return $"log({value}::numeric,{field}::numeric)";
        }

        public override string CreateLog10Sql(object field)
        {
            return $"log({field}::numeric)";
        }

        public override string CreateExpSql(object field)
        {
            return $"exp({field}::numeric)";
        }

        public override string CreatePowSql(object field, object value)
        {
            return $"power({field}::numeric,{value}::numeric)";
        }

        public override string CreateSinSql(object field)
        {
            return $"sin({field}::numeric)";
        }

        public override string CreateCosSql(object field)
        {
            return $"cos({field}::numeric)";
        }

        public override string CreateAsinSql(object field)
        {
            return $"asin({field}::numeric)";
        }

        public override string CreateAcosSql(object field)
        {
            return $"acos({field}::numeric)";
        }

        public override string CreateTanSql(object field)
        {
            return $"tan({field}::numeric)";
        }

        public override string CreateAtanSql(object field)
        {
            return $"atan({field}::numeric)";
        }

        public override string CreateAtan2Sql(object field, object value)
        {
            return $"atan2({field}::numeric,{value}::numeric)";
        }

        public override string CreateCeilingSql(object field)
        {
            return $"ceiling({field}::numeric)";
        }

        public override string CreateFloorSql(object field)
        {
            return $"floor({field}::numeric)";
        }

        public override string CreateRoundSql(object field, object value)
        {
            return $"round({field}::numeric,{value}::int4)";
        }

        public override string CreateSqrtSql(object field)
        {
            return $"Sqrt({field}::numeric)";
        }

        public override string CreateYearSql(object field)
        {
            return $"extract(year from {field})::int4";
        }

        public override string CreateMonthSql(object field)
        {
            return $"extract(month from {field})::int4";
        }

        public override string CreateDaySql(object field)
        {
            return $"extract(day from {field})::int4";
        }

        public override string CreateHourSql(object field)
        {
            return $"extract(hour from {field})::int4";
        }

        public override string CreateMinuteSql(object field)
        {
            return $"extract(minute from {field})::int4";
        }

        public override string CreateSecondSql(object field)
        {
            return $"extract(second from {field})::int4";
        }

        public override string CreateWeekSql(object field)
        {
            return $"extract(week from {field})::int4";
        }

        public override string CreateWeekDaySql(object field)
        {
            return $"extract(dow from {field})::int4";
        }

        public override string CreateYearDaySql(object field)
        {
            return $"extract(doy from {field})::int4";
        }

        public override string CreateLengthSql(object field)
        {
            return $"length({field})";
        }

        public override string CreateSubStringSql(object field, object start, object size)
        {
            if (Equals(size, null))
            {
                return $"substr({field},{start}+1)";
            }
            else
            {
                return $"substr({field},{start}+1,{size})";
            }
        }

        public override string CreateIndexOfSql(object field, object value, object startIndex)
        {
            if (Equals(startIndex, null))
            {
                return $"strpos({field},{value})-1";
            }
            else
            {
                return
                    $"(case when strpos(substr({field},{startIndex}+1),{value})>0 then strpos(substr({field},{startIndex}+1),{value})+{startIndex}-1 else -1 end)";
            }
        }

        public override string CreateReplaceSql(object field, object oldValue, object newValue)
        {
            return $"replace({field},{oldValue},{newValue})";
        }

        public override string CreateToLowerSql(object field)
        {
            return $"lower({field})";
        }

        public override string CreateToUpperSql(object field)
        {
            return $"upper({field})";
        }

        public override string CreateTrimSql(object field)
        {
            return $"trim({field})";
        }

        public override string CreateDataBaseTimeSql()
        {
            return "current_time";
        }

        public override string ParameterPrefix => ":";
    }
}