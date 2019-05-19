using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Collections;

namespace Light.Data.Postgre
{
    class PostgreCommandFactory : CommandFactory
    {

        DateTimeFormater dateTimeFormater = new DateTimeFormater();

        readonly string defaultDateTime = "YYYY-MM-DD HH:MI:SS";

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
            CommandData data = base.CreateTruncateTableCommand(mapping, state);
            if (mapping.IdentityField != null) {
                string restartSeq = string.Format("alter sequence \"{0}\" restart;", GetIndentitySeq(mapping, state));
                data.CommandText += restartSeq;
            }
            return data;
        }

        public override string CreateBooleanQuerySql(object fieldName, bool isTrue)
        {
            return string.Format("{0}={1}", fieldName, isTrue ? "true" : "false");
        }

        public override string CreateDataFieldSql(string fieldName)
        {
            if (_strictMode) {
                return string.Format("\"{0}\"", fieldName);
            }
            else {
                return base.CreateDataFieldSql(fieldName);
            }
        }

        public override string CreateDataTableSql(string tableName)
        {
            if (_strictMode) {
                return string.Format("\"{0}\"", tableName);
            }
            else {
                return base.CreateDataTableSql(tableName);
            }
        }

        public override string CreateDividedSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("({0}::float/{1})", field, value);
            }
            else {
                return string.Format("({0}/{1}::float)", value, field);
            }
        }

        public override CommandData CreateSelectBaseCommand(DataEntityMapping mapping, string customSelect, QueryExpression query, OrderExpression order, Region region, CreateSqlState state)//, bool distinct)
        {
            CommandData command = base.CreateSelectBaseCommand(mapping, customSelect, query, order, region, state);
            if (region != null) {
                if (region.Start == 0) {
                    command.CommandText = string.Format("{0} limit {1}", command.CommandText, region.Size);
                }
                else {
                    command.CommandText = string.Format("{0} limit {2} offset {1}", command.CommandText, region.Start, region.Size);
                }
                command.InnerPage = true;
            }
            return command;
        }

        public override CommandData CreateSelectJoinTableBaseCommand(string customSelect, List<IJoinModel> modelList, QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            CommandData command = base.CreateSelectJoinTableBaseCommand(customSelect, modelList, query, order, region, state);
            if (region != null) {
                if (region.Start == 0) {
                    command.CommandText = string.Format("{0} limit {1}", command.CommandText, region.Size);
                }
                else {
                    command.CommandText = string.Format("{0} limit {2} offset {1}", command.CommandText, region.Start, region.Size);
                }
                command.InnerPage = true;
            }
            return command;
        }

        public override CommandData CreateAggregateTableCommand(DataEntityMapping mapping, AggregateSelector selector, AggregateGroupBy groupBy, QueryExpression query, QueryExpression having, OrderExpression order, Region region, CreateSqlState state)
        {
            CommandData command = base.CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order, region, state);
            if (region != null) {
                if (region.Start == 0) {
                    command.CommandText = string.Format("{0} limit {1}", command.CommandText, region.Size);
                }
                else {
                    command.CommandText = string.Format("{0} limit {2} offset {1}", command.CommandText, region.Start, region.Size);
                }
                command.InnerPage = true;
            }
            return command;
        }

        public override CommandData CreateBatchInsertCommand(DataTableEntityMapping mapping, IList entitys, bool refresh, CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0) {
                throw new ArgumentNullException(nameof(entitys));
            }
            int totalCount = entitys.Count;
            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            int insertLen = fields.Count;
            if (insertLen == 0) {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }
            string insertSql = null;
            string cachekey = null;
            if (state.Seed == 0) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_batchInsertCache.TryGetCommand(cachekey, out string cache)) {
                    insertSql = cache;
                }
            }
            if (insertSql == null) {
                string[] insertList = new string[insertLen];
                for (int i = 0; i < insertLen; i++) {
                    DataFieldMapping field = fields[i];
                    insertList[i] = CreateDataFieldSql(field.Name);
                }
                string insert = string.Join(",", insertList);
                insertSql = string.Format("insert into {0}({1})", CreateDataTableMappingSql(mapping, state), insert);
                if (cachekey != null) {
                    _batchInsertCache.SetCommand(cachekey, insertSql);
                }
            }
            StringBuilder totalSql = new StringBuilder();

            totalSql.AppendFormat("{0}values", insertSql);
            int cur = 0;
            int end = entitys.Count;
            foreach (object entity in entitys) {
                string[] valuesList = new string[insertLen];
                for (int i = 0; i < insertLen; i++) {
                    DataFieldMapping field = fields[i];
                    //object obj = field.Handler.Get(entity);
                    //object value = field.ToColumn(obj);
                    object value = field.GetInsertData(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }
                string values = string.Join(",", valuesList);
                totalSql.AppendFormat("({0})", values);
                cur++;
                if (cur < end) {
                    totalSql.Append(',');
                }
                else {
                    totalSql.Append(';');
                }
            }
            CommandData command = new CommandData(totalSql.ToString());
            return command;
        }

        public override string CreateCollectionParamsQuerySql(object fieldName, QueryCollectionPredicate predicate, IEnumerable<object> list)
        {
            if (predicate == QueryCollectionPredicate.In || predicate == QueryCollectionPredicate.NotIn) {
                return base.CreateCollectionParamsQuerySql(fieldName, predicate, list);
            }
            string op = GetQueryCollectionPredicate(predicate);

            int i = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1} (", fieldName, op);
            foreach (object item in list) {
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
            return "random()";
        }

        public override string CreateIdentitySql(DataTableEntityMapping mapping, CreateSqlState state)
        {
            if (mapping.IdentityField != null) {
                return string.Format("select currval('\"{0}\"');", GetIndentitySeq(mapping, state));
            }
            else {
                return string.Empty;
            }
        }

        private static string GetIndentitySeq(DataTableEntityMapping mapping, CreateSqlState state)
        {
            if (mapping.IdentityField == null) {
                throw new LightDataException(SR.NoIdentityField);
            }
            string seq;
            string postgreIdentity = mapping.ExtentParams.GetParamValue("PostgreIdentitySeq");
            if (!string.IsNullOrEmpty(postgreIdentity)) {
                seq = postgreIdentity;
            }
            else {
                if (!state.TryGetAliasTableName(mapping, out string name)) {
                    name = mapping.TableName;
                }
                seq = string.Format("{0}_{1}_seq", name, mapping.IdentityField.Name);
            }
            return seq;
        }

        public override string CreateMatchSql(object field, bool starts, bool ends)
        {
            StringBuilder sb = new StringBuilder();
            if (starts) {
                sb.AppendFormat("'{0}'||", _wildcards);
            }
            sb.Append(field);
            if (ends) {
                sb.AppendFormat("||'{0}'", _wildcards);
            }
            return sb.ToString();
        }

        public override string CreateBooleanQuerySql(object field, bool isTrue, bool isEqual, bool isReverse)
        {
            if (!isReverse) {
                return string.Format("{0}{2}{1}", field, isTrue ? "true" : "false", isEqual ? "=" : "!=");
            }
            else {
                return string.Format("{1}{2}{0}", field, isTrue ? "true" : "false", isEqual ? "=" : "!=");
            }
        }

        public override string CreateConcatSql(params object[] values)
        {
            string value1 = string.Join("||", values);
            string sql = string.Format("({0})", value1);
            return sql;
        }

        public override string CreateDualConcatSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("({0}||{1})", field, value);
            }
            else {
                return string.Format("({0}||{1})", value, field);
            }
        }

        public override string CreateDateSql(object field)
        {
            return string.Format("date({0})", field);
        }

        public override string CreateDateTimeFormatSql(object field, string format)
        {
            string sqlformat;
            if (string.IsNullOrEmpty(format)) {
                sqlformat = defaultDateTime;
            }
            else {
                sqlformat = dateTimeFormater.FormatData(format);
            }
            return string.Format("to_char({0},'{1}')", field, sqlformat);
        }

        public override string CreateTruncateSql(object field)
        {
            return string.Format("trunc({0}::numeric)", field);
        }

        public override string CreateLogSql(object field)
        {
            return string.Format("ln({0}::numeric)", field);
        }

        public override string CreateLogSql(object field, object value)
        {
            return string.Format("log({1}::numeric,{0}::numeric)", field, value);
        }

        public override string CreateLog10Sql(object field)
        {
            return string.Format("log({0}::numeric)", field);
        }

        public override string CreateExpSql(object field)
        {
            return string.Format("exp({0}::numeric)", field);
        }

        public override string CreatePowSql(object field, object value)
        {
            return string.Format("power({0}::numeric,{1}::numeric)", field, value);
        }

        public override string CreateSinSql(object field)
        {
            return string.Format("sin({0}::numeric)", field);
        }

        public override string CreateCosSql(object field)
        {
            return string.Format("cos({0}::numeric)", field);
        }

        public override string CreateAsinSql(object field)
        {
            return string.Format("asin({0}::numeric)", field);
        }

        public override string CreateAcosSql(object field)
        {
            return string.Format("acos({0}::numeric)", field);
        }

        public override string CreateTanSql(object field)
        {
            return string.Format("tan({0}::numeric)", field);
        }

        public override string CreateAtanSql(object field)
        {
            return string.Format("atan({0}::numeric)", field);
        }

        public override string CreateAtan2Sql(object field, object value)
        {
            return string.Format("atan2({0}::numeric,{1}::numeric)", field, value);
        }

        public override string CreateCeilingSql(object field)
        {
            return string.Format("ceiling({0}::numeric)", field);
        }

        public override string CreateFloorSql(object field)
        {
            return string.Format("floor({0}::numeric)", field);
        }

        public override string CreateRoundSql(object field, object value)
        {
            return string.Format("round({0}::numeric,{1}::int4)", field, value);
        }

        public override string CreateSqrtSql(object field)
        {
            return string.Format("Sqrt({0}::numeric)", field);
        }

        public override string CreateYearSql(object field)
        {
            return string.Format("extract(year from {0})::int4", field);
        }

        public override string CreateMonthSql(object field)
        {
            return string.Format("extract(month from {0})::int4", field);
        }

        public override string CreateDaySql(object field)
        {
            return string.Format("extract(day from {0})::int4", field);
        }

        public override string CreateHourSql(object field)
        {
            return string.Format("extract(hour from {0})::int4", field);
        }

        public override string CreateMinuteSql(object field)
        {
            return string.Format("extract(minute from {0})::int4", field);
        }

        public override string CreateSecondSql(object field)
        {
            return string.Format("extract(second from {0})::int4", field);
        }

        public override string CreateWeekSql(object field)
        {
            return string.Format("extract(week from {0})::int4", field);
        }

        public override string CreateWeekDaySql(object field)
        {
            return string.Format("extract(dow from {0})::int4", field);
        }

        public override string CreateYearDaySql(object field)
        {
            return string.Format("extract(doy from {0})::int4", field);
        }

        public override string CreateLengthSql(object field)
        {
            return string.Format("length({0})", field);
        }

        public override string CreateSubStringSql(object field, object start, object size)
        {
            if (object.Equals(size, null)) {
                return string.Format("substr({0},{1}+1)", field, start);
            }
            else {
                return string.Format("substr({0},{1}+1,{2})", field, start, size);
            }
        }

        public override string CreateIndexOfSql(object field, object value, object startIndex)
        {
            if (object.Equals(startIndex, null)) {
                return string.Format("strpos({0},{1})-1", field, value);
            }
            else {
                return string.Format("(case when strpos(substr({0},{2}+1),{1})>0 then strpos(substr({0},{2}+1),{1})+{2}-1 else -1 end)", field, value, startIndex);
            }
        }

        public override string CreateReplaceSql(object field, object oldValue, object newValue)
        {
            return string.Format("replace({0},{1},{2})", field, oldValue, newValue);
        }

        public override string CreateToLowerSql(object field)
        {
            return string.Format("lower({0})", field);
        }

        public override string CreateToUpperSql(object field)
        {
            return string.Format("upper({0})", field);
        }

        public override string CreateTrimSql(object field)
        {
            return string.Format("trim({0})", field);
        }

        public override string CreateDataBaseTimeSql()
        {
            return "current_time";
        }

        public override string ParameterPrefix => ":";
    }
}
