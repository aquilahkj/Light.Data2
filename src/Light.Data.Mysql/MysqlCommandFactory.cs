using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Mysql
{
    internal class MysqlCommandFactory : CommandFactory
    {
        public override bool SupportBatchInsertIdentity => true;

        private readonly DateTimeFormater dateTimeFormater = new DateTimeFormater();

        private const string defaultDateTime = "%Y-%m-%d %H:%i:%S";

        public MysqlCommandFactory()
        {
            dateTimeFormater.YearFormat = "%Y";
            dateTimeFormater.MonthFormat = "%m";
            dateTimeFormater.DayFormat = "%d";
            dateTimeFormater.HourFormat = "%H";
            dateTimeFormater.MinuteFormat = "%i";
            dateTimeFormater.SecondFormat = "%S";

            HavingAlias = true;
            OrderByAlias = true;
        }

        public override string CreateDataFieldSql(string fieldName)
        {
            if (_strictMode)
            {
                return $"`{fieldName}`";
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
                return $"`{tableName}`";
            }
            else
            {
                return base.CreateDataTableSql(tableName);
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
                    : $"{command.CommandText} limit {region.Start},{region.Size}";

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
                    : $"{command.CommandText} limit {region.Start},{region.Size}";
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
                    : $"{command.CommandText} limit {region.Start},{region.Size}";

                command.InnerPage = true;
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

            totalSql.Append("drop temporary table if exists `temptb`;create temporary table `temptb`(`id` int(11));");

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
                totalSql.AppendFormat("{0}values({1});insert into `temptb`(`id`) select last_insert_id();", insertSql,
                    values);
            }

            totalSql.Append("select `id` from `temptb`;");
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

            totalSql.AppendFormat("{0}values", insertSql);
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
                totalSql.AppendFormat("({0})", values);
                cur++;
                totalSql.Append(cur < end ? ',' : ';');
            }

            var command = new CommandData(totalSql.ToString());
            return command;
        }


        public override string CreateCollectionParamsQuerySql(object fieldName, QueryCollectionPredicate predicate,
            IEnumerable<object> list)
        {
            if (predicate == QueryCollectionPredicate.In || predicate == QueryCollectionPredicate.NotIn)
            {
                return base.CreateCollectionParamsQuerySql(fieldName, predicate, list);
            }

            var op = GetQueryCollectionPredicate(predicate);

            var i = 0;
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {1} (", fieldName, op);
            foreach (var item in list)
            {
                if (i > 0)
                    sb.Append(" union all ");
                sb.AppendFormat("select {0}", item);
                i++;
            }

            sb.Append(")");
            return sb.ToString();
        }

        public override string CreateRandomOrderBySql(DataEntityMapping mapping, string aliasName, bool fullFieldName)
        {
            return "rand()";
        }

        public override string CreateIdentitySql(DataTableEntityMapping mapping, CreateSqlState state)
        {
            if (mapping.IdentityField != null)
            {
                return "select last_insert_id();";
            }
            else
            {
                return string.Empty;
            }
        }

        public override string CreateMatchSql(object field, bool starts, bool ends)
        {
            var sb = new StringBuilder();
            sb.Append("concat(");
            if (starts)
            {
                sb.AppendFormat("'{0}',", Wildcards);
            }

            sb.Append(field);
            if (ends)
            {
                sb.AppendFormat(",'{0}'", Wildcards);
            }

            sb.Append(")");
            return sb.ToString();
        }

        public override string CreateConcatSql(params object[] values)
        {
            var value1 = string.Join(",", values);
            var sql = $"concat({value1})";
            return sql;
        }

        public override string CreateDualConcatSql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"concat({field},{value})";
            }

            return $"concat({value},{field})";
        }

        public override string CreateDateSql(object field)
        {
            return $"date({field})";
        }

        public override string CreateDateTimeFormatSql(object field, string format)
        {
            var sqlFormat = string.IsNullOrEmpty(format) ? defaultDateTime : dateTimeFormater.FormatData(format);
            return $"date_format({field},'{sqlFormat}')";
        }

        public override string CreateLogSql(object field)
        {
            return $"ln({field})";
        }

        public override string CreateYearSql(object field)
        {
            return $"year({field})";
        }

        public override string CreateMonthSql(object field)
        {
            return $"month({field})";
        }

        public override string CreateDaySql(object field)
        {
            return $"day({field})";
        }

        public override string CreateHourSql(object field)
        {
            return $"hour({field})";
        }

        public override string CreateMinuteSql(object field)
        {
            return $"minute({field})";
        }

        public override string CreateSecondSql(object field)
        {
            return $"second({field})";
        }

        public override string CreateWeekSql(object field)
        {
            return $"week({field},7)";
        }

        public override string CreateWeekDaySql(object field)
        {
            return $"dayofweek({field})-1";
        }

        public override string CreateYearDaySql(object field)
        {
            return $"dayofyear({field})";
        }

        public override string CreateLengthSql(object field)
        {
            return $"length({field})";
        }

        public override string CreateSubStringSql(object field, object start, object size)
        {
            if (Equals(size, null))
            {
                return $"substring({field},{start}+1)";
            }

            return $"substring({field},{start}+1,{size})";
        }

        public override string CreateIndexOfSql(object field, object value, object startIndex)
        {
            if (Equals(startIndex, null))
            {
                return string.Format("locate({1},{0})-1", field, value);
            }

            return string.Format("locate({1},{0},{2}+1)-1", field, value, startIndex);
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

        public override string CreatePowerSql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"power({field},{value})";
            }
            else
            {
                return $"power({value},{field})";
            }
        }

        public override string CreatePowerSql(object left, object right)
        {
            return $"power({left},{right})";
        }

        public override string CreateLogSql(object field, object value)
        {
            return string.Format("log({1},{0})", field, value);
        }

        public override string CreateDataBaseTimeSql()
        {
            return "now()";
        }

        public override string ParameterPrefix => "?";
    }
}