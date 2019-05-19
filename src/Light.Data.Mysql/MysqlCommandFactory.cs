using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data.Mysql
{
    class MysqlCommandFactory : CommandFactory
    {

        DateTimeFormater dateTimeFormater = new DateTimeFormater();

        readonly string defaultDateTime = "%Y-%m-%d %H:%i:%S";

        public MysqlCommandFactory()
        {
            dateTimeFormater.YearFormat = "%Y";
            dateTimeFormater.MonthFormat = "%m";
            dateTimeFormater.DayFormat = "%d";
            dateTimeFormater.HourFormat = "%H";
            dateTimeFormater.MinuteFormat = "%i";
            dateTimeFormater.SecondFormat = "%S";

            _havingAlias = true;
            _orderbyAlias = true;
        }

        public override string CreateDataFieldSql(string fieldName)
        {
            if (_strictMode) {
                return string.Format("`{0}`", fieldName);
            }
            else {
                return base.CreateDataFieldSql(fieldName);
            }
        }

        public override string CreateDataTableSql(string tableName)
        {
            if (_strictMode) {
                return string.Format("`{0}`", tableName);
            }
            else {
                return base.CreateDataTableSql(tableName);
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
                    command.CommandText = string.Format("{0} limit {1},{2}", command.CommandText, region.Start, region.Size);
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
                    command.CommandText = string.Format("{0} limit {1},{2}", command.CommandText, region.Start, region.Size);
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
                    command.CommandText = string.Format("{0} limit {1},{2}", command.CommandText, region.Start, region.Size);
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
            return "rand()";
        }

        public override string CreateIdentitySql(DataTableEntityMapping mapping, CreateSqlState state)
        {
            if (mapping.IdentityField != null) {
                return "select last_insert_id();";
            }
            else {
                return string.Empty;
            }
        }

        public override string CreateMatchSql(object field, bool starts, bool ends)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("concat(");
            if (starts) {
                sb.AppendFormat("'{0}',", _wildcards);
            }
            sb.Append(field);
            if (ends) {
                sb.AppendFormat(",'{0}'", _wildcards);
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override string CreateConcatSql(params object[] values)
        {
            string value1 = string.Join(",", values);
            string sql = string.Format("concat({0})", value1);
            return sql;
        }

        public override string CreateDualConcatSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("concat({0},{1})", field, value);
            }
            else {
                return string.Format("concat({0},{1})", value, field);
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
            return string.Format("date_format({0},'{1}')", field, sqlformat);
        }

        public override string CreateLogSql(object field)
        {
            return string.Format("ln({0})", field);
        }

        public override string CreateYearSql(object field)
        {
            return string.Format("year({0})", field);
        }

        public override string CreateMonthSql(object field)
        {
            return string.Format("month({0})", field);
        }

        public override string CreateDaySql(object field)
        {
            return string.Format("day({0})", field);
        }

        public override string CreateHourSql(object field)
        {
            return string.Format("hour({0})", field);
        }

        public override string CreateMinuteSql(object field)
        {
            return string.Format("minute({0})", field);
        }

        public override string CreateSecondSql(object field)
        {
            return string.Format("second({0})", field);
        }

        public override string CreateWeekSql(object field)
        {
            return string.Format("week({0},7)", field);
        }

        public override string CreateWeekDaySql(object field)
        {
            return string.Format("dayofweek({0})-1", field);
        }

        public override string CreateYearDaySql(object field)
        {
            return string.Format("dayofyear({0})", field);
        }

        public override string CreateLengthSql(object field)
        {
            return string.Format("length({0})", field);
        }

        public override string CreateSubStringSql(object field, object start, object size)
        {
            if (object.Equals(size, null)) {
                return string.Format("substring({0},{1}+1)", field, start);
            }
            else {
                return string.Format("substring({0},{1}+1,{2})", field, start, size);
            }
        }

        public override string CreateIndexOfSql(object field, object value, object startIndex)
        {
            if (object.Equals(startIndex, null)) {
                return string.Format("locate({1},{0})-1", field, value);
            }
            else {
                return string.Format("locate({1},{0},{2}+1)-1", field, value, startIndex);
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

        public override string CreatePowerSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("power({0},{1})", field, value);
            }
            else {
                return string.Format("power({0},{1})", value, field);
            }
        }

        public override string CreatePowerSql(object left, object right)
        {
            return string.Format("power({0},{1})", left, right);
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
