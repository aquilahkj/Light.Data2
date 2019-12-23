using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Mssql
{
    internal class MssqlCommandFactory : CommandFactory
    {
        private readonly Dictionary<string, string> dateTimeFormatDict = new Dictionary<string, string>();

        private readonly string defaultDateTime = "CONVERT(char(19), {0}, 120)";

        public MssqlCommandFactory()
        {
            dateTimeFormatDict.Add("yyyy-MM-dd hh:mm:ss", "CONVERT(char(19), {0}, 120)");
            dateTimeFormatDict.Add("yyyy-MM-dd", "CONVERT(char(10), {0}, 23)");
            dateTimeFormatDict.Add("MM/dd/yyyy", "CONVERT(char(10), {0}, 101)");
            dateTimeFormatDict.Add("yyyy.MM.dd", "CONVERT(char(10), {0}, 102)");
            dateTimeFormatDict.Add("dd/MM/yyyy", "CONVERT(char(10), {0}, 103)");
            dateTimeFormatDict.Add("dd.MM.yyyy", "CONVERT(char(10), {0}, 104)");
            dateTimeFormatDict.Add("dd-MM-yyyy", "CONVERT(char(10), {0}, 105)");
            dateTimeFormatDict.Add("dd MM yyyy", "CONVERT(char(10), {0}, 106)");
            dateTimeFormatDict.Add("MM dd, yyyy", "CONVERT(char(11), {0}, 107)");
            dateTimeFormatDict.Add("hh:mm:ss", "CONVERT(char(8), {0}, 108)");
            dateTimeFormatDict.Add("MM-dd-yyyy", "CONVERT(char(10), {0}, 110)");
            dateTimeFormatDict.Add("yyyy/MM/dd", "CONVERT(char(10), {0}, 111)");
            dateTimeFormatDict.Add("yyyyMMdd", "CONVERT(char(8), {0}, 112)");
            dateTimeFormatDict.Add("yyyyMM", "CONVERT(char(6), {0}, 112)");
            dateTimeFormatDict.Add("yyyy", "CONVERT(char(4), {0}, 112)");
            dateTimeFormatDict.Add("MM", "CONVERT(char(2), {0}, 101)");
            dateTimeFormatDict.Add("dd", "CONVERT(char(2), {0}, 103)");
            dateTimeFormatDict.Add("hh:mm", "CONVERT(char(5), {0}, 108)");

            dateTimeFormatDict.Add("yyyy-MM", "CONVERT(char(7), {0}, 23)");
            dateTimeFormatDict.Add("dd-MM", "CONVERT(char(5), {0}, 105)");
            dateTimeFormatDict.Add("MM-dd", "CONVERT(char(5), {0}, 110)");

            dateTimeFormatDict.Add("yyyy/MM", "CONVERT(char(7), {0}, 111)");
            dateTimeFormatDict.Add("dd/MM", "CONVERT(char(5), {0}, 103)");
            dateTimeFormatDict.Add("MM/dd", "CONVERT(char(5), {0}, 101)");

            dateTimeFormatDict.Add("yyyy.MM", "CONVERT(char(7), {0}, 102)");
            dateTimeFormatDict.Add("dd.MM", "CONVERT(char(5), {0}, 104)");

            dateTimeFormatDict.Add("dd MM", "CONVERT(char(5), {0}, 106)");
            dateTimeFormatDict.Add("MM dd", "CONVERT(char(5), {0}, 107)");

            _havingAlias = false;
            _orderbyAlias = true;
        }

        public override int MaxParameterCount => 2000;

        public override string CreateDataFieldSql(string fieldName)
        {
            if (_strictMode) {
                return string.Format("[{0}]", fieldName);
            }
            else {
                return base.CreateDataFieldSql(fieldName);
            }
        }

        public override string CreateDataTableSql(string tableName)
        {
            if (_strictMode) {
                return string.Format("[{0}]", tableName);
            }
            else {
                return base.CreateDataTableSql(tableName);
            }
        }

        public override string CreateAvgSql(object fieldName, bool isDistinct)
        {
            return string.Format("avg({1}convert(float,{0}))", fieldName, isDistinct ? "distinct " : "");
        }

        public override string CreateConditionAvgSql(string expressionSql, object fieldName, bool isDistinct)
        {
            return string.Format("avg({2}case when {0} then convert(float,{1}) else null end)", expressionSql, fieldName, isDistinct ? "distinct " : "");
        }

        public override string CreateMatchSql(object field, bool starts, bool ends)
        {
            var sb = new StringBuilder();
            if (starts) {
                sb.AppendFormat("'{0}'+", _wildcards);
            }
            sb.Append(field);
            if (ends) {
                sb.AppendFormat("+'{0}'", _wildcards);
            }
            return sb.ToString();
        }

        public override string CreateDateSql(object field)
        {
            return string.Format("cast({0} as date)", field);
        }

        public override string CreateDateTimeFormatSql(object field, string format)
        {
            string sqlFormat;
            if (string.IsNullOrEmpty(format)) {
                sqlFormat = defaultDateTime;
            }
            else if (!dateTimeFormatDict.TryGetValue(format, out sqlFormat)) {
                throw new NotSupportedException();
            }
            return string.Format(sqlFormat, field);
        }


        public override string CreateTruncateSql(object field)
        {
            return string.Format("cast({0} as int)", field);
        }

        public override string CreateAtan2Sql(object field, object value)
        {
            return string.Format("atn2({0},{1})", field, value);
        }

        public override string CreateYearSql(object field)
        {
            return string.Format("datepart(year,{0})", field);
        }

        public override string CreateMonthSql(object field)
        {
            return string.Format("datepart(month,{0})", field);
        }

        public override string CreateDaySql(object field)
        {
            return string.Format("datepart(day,{0})", field);
        }

        public override string CreateHourSql(object field)
        {
            return string.Format("datepart(hour,{0})", field);
        }

        public override string CreateMinuteSql(object field)
        {
            return string.Format("datepart(minute,{0})", field);
        }

        public override string CreateSecondSql(object field)
        {
            return string.Format("datepart(second,{0})", field);
        }

        public override string CreateWeekSql(object field)
        {
            return string.Format("datepart(week,{0})", field);
        }

        public override string CreateWeekDaySql(object field)
        {
            return string.Format("datepart(weekday,{0})-1", field);
        }

        public override string CreateYearDaySql(object field)
        {
            return string.Format("datepart(dayofyear,{0})", field);
        }

        public override string CreateLengthSql(object field)
        {
            return string.Format("len({0})", field);
        }

        public override string CreateSubStringSql(object field, object start, object size)
        {
            if (object.Equals(size, null)) {
                return string.Format("substring({0},{1}+1,len({0}))", field, start);
            }
            else {
                return string.Format("substring({0},{1}+1,{2})", field, start, size);
            }
        }

        public override string CreateIndexOfSql(object field, object value, object startIndex)
        {
            if (object.Equals(startIndex, null)) {
                return string.Format("charindex({0},{1})-1", value, field);
            }
            else {
                return string.Format("charindex({0},{1},{2}+1)-1", value, field, startIndex);
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
            return string.Format("rtrim(ltrim({0}))", field);
        }

        public override string CreateOutputNotSql(object value)
        {
            var sql = string.Format("case when not({0})=1 then 1 else 0 end", value);
            return sql;
        }
        
        public override string CreateDividedSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("convert(float,{0})/{1}", field, value);
            }
            else {
                return string.Format("convert(float,{0})/{1}", value, field);
            }
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

        public override string CreateDataBaseTimeSql()
        {
            return "getdate()";
        }

        public override CommandData CreateSelectInsertCommand(InsertSelector insertSelector, DataEntityMapping mapping, AggregateSelector selector, AggregateGroupBy groupBy, QueryExpression query, QueryExpression having, OrderExpression order, CreateSqlState state)
        {
            var selectCommandData = CreateAggregateTableCommand(mapping, selector, groupBy, query, having, null, null, state);
            var insertFields = insertSelector.GetInsertFields();
            var insertFieldNames = new string[insertFields.Length];
            for (var i = 0; i < insertFields.Length; i++) {
                insertFieldNames[i] = CreateDataFieldSql(insertFields[i].FieldName);
            }
            var insertString = string.Join(",", insertFieldNames);
            var selectString = insertSelector.CreateSelectString(this, false, state);
            var sql = string.Format("insert into {0}({1})select {2} from ({3}) as A", CreateDataTableMappingSql(insertSelector.InsertMapping, state), insertString, selectString, selectCommandData.CommandText);
            if (order != null) {
                state.UseFieldAlias = true;
                sql += GetOrderString(order, false, state);
                state.UseFieldAlias = false;
            }
            selectCommandData.CommandText = sql;
            return selectCommandData;
        }

        public override CommandData CreateSelectBaseCommand(DataEntityMapping mapping, string customSelect, QueryExpression query, OrderExpression order, Region region, CreateSqlState state)//, bool distinct)
        {
            if (region != null) {
                if (region.Start == 0) {
                    var sql = new StringBuilder();
                    sql.AppendFormat("select top {2} {0} from {1}", customSelect, CreateDataTableMappingSql(mapping, state), region.Size);
                    if (query != null) {
                        sql.Append(GetQueryString(query, false, state));
                    }
                    if (order != null) {
                        sql.Append(GetOrderString(order, false, state));
                    }
                    var commandData = new CommandData(sql.ToString());
                    commandData.InnerPage = true;
                    return commandData;
                }
                else {
                    if (order == null) {
                        order = CreatePrimaryKeyOrderExpression(mapping);
                    }
                    if (order != null) {
                        var rname = CreateDataFieldSql("R" + Guid.NewGuid().ToString("N"));
                        var sql = new StringBuilder();
                        sql.Append("select * from (");
                        var totalsize = region.Start + (long)region.Size;
                        sql.AppendFormat("select top {4} {0},row_number() over (order by {2}) as {3} from {1}", customSelect, CreateDataTableMappingSql(mapping, state), order.CreateSqlString(this, false, state), rname, totalsize);
                        if (query != null) {
                            sql.Append(GetQueryString(query, false, state));
                        }
                        sql.AppendFormat(") as RT where {0}>{1}", rname, region.Start);
                        var commandData = new CommandData(sql.ToString());
                        commandData.InnerPage = true;
                        return commandData;
                    }
                }
            }
            return base.CreateSelectBaseCommand(mapping, customSelect, query, order, region, state);
        }

        public override CommandData CreateSelectJoinTableBaseCommand(string customSelect, List<IJoinModel> modelList, QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            if (region != null) {
                if (region.Start == 0) {
                    var tables = new StringBuilder();
                    foreach (var model in modelList) {
                        if (model.Connect != null) {
                            tables.AppendFormat(" {0} ", _joinCollectionPredicateDict[model.Connect.Type]);
                        }
                        var modelsql = model.CreateSqlString(this, state);
                        tables.Append(modelsql);
                        if (model.Connect != null && model.Connect.On != null) {
                            tables.Append(GetOnString(model.Connect.On, state));
                        }
                    }
                    var sql = new StringBuilder();
                    sql.AppendFormat("select top {2} {0} from {1}", customSelect, tables, region.Size);
                    if (query != null) {
                        sql.Append(GetQueryString(query, true, state));
                    }
                    if (order != null) {
                        sql.Append(GetOrderString(order, true, state));
                    }
                    var command = new CommandData(sql.ToString());
                    command.InnerPage = true;
                    return command;
                }
                else {
                    if (order != null) {
                        var rname = CreateDataFieldSql("R" + Guid.NewGuid().ToString("N"));
                        var tables = new StringBuilder();
                        foreach (var model in modelList) {
                            if (model.Connect != null) {
                                tables.AppendFormat(" {0} ", _joinCollectionPredicateDict[model.Connect.Type]);
                            }
                            var modelsql = model.CreateSqlString(this, state);
                            tables.Append(modelsql);
                            if (model.Connect != null && model.Connect.On != null) {
                                tables.Append(GetOnString(model.Connect.On, state));
                            }
                        }
                        var sql = new StringBuilder();
                        sql.Append("select * from (");
                        var totalsize = region.Start + (long)region.Size;
                        sql.AppendFormat("select top {4} {0},row_number() over (order by {2}) as {3} from {1}", customSelect, tables, order.CreateSqlString(this, true, state), rname, totalsize);
                        if (query != null) {
                            sql.Append(GetQueryString(query, false, state));
                        }
                        sql.AppendFormat(") as RT where {0}>{1}", rname, region.Start);
                        var commandData = new CommandData(sql.ToString());
                        commandData.InnerPage = true;
                        return commandData;
                    }
                }
            }
            return base.CreateSelectJoinTableBaseCommand(customSelect, modelList, query, order, region, state);
        }

        public override CommandData CreateAggregateTableCommand(DataEntityMapping mapping, AggregateSelector selector, AggregateGroupBy groupBy, QueryExpression query, QueryExpression having, OrderExpression order, Region region, CreateSqlState state)
        {
            if (region != null) {
                if (region.Start == 0) {
                    var sql = new StringBuilder();
                    var selectString = selector.CreateSelectString(this, false, state);
                    sql.AppendFormat("select top {2} {0} from {1}", selectString, CreateDataTableMappingSql(mapping, state), region.Size);
                    if (query != null) {
                        sql.Append(GetQueryString(query, false, state));
                    }
                    if (groupBy != null) {
                        sql.Append(GetGroupByString(groupBy, false, state));
                    }
                    if (having != null) {
                        sql.Append(GetHavingString(having, false, state));
                    }
                    if (order != null) {
                        sql.Append(GetAggregateOrderString(order, false, state));
                    }
                    var command = new CommandData(sql.ToString());
                    command.InnerPage = true;
                    return command;
                }
                else {
                    if (order == null) {
                        order = CreateGroupByOrderExpression(groupBy);
                    }
                    if (order != null) {
                        var rname = CreateDataFieldSql("R" + Guid.NewGuid().ToString("N"));
                        var sql = new StringBuilder();
                        var selectString = selector.CreateSelectString(this, false, state);
                        sql.Append("select * from (");
                        var totalsize = region.Start + (long)region.Size;
                        sql.AppendFormat("select top {4} {0},row_number() over (order by {2}) as {3} from {1}", selectString, CreateDataTableMappingSql(mapping, state), order.CreateSqlString(this, false, state), rname, totalsize);
                        if (query != null) {
                            sql.Append(GetQueryString(query, false, state));
                        }
                        if (groupBy != null) {
                            sql.Append(GetGroupByString(groupBy, false, state));
                        }
                        if (having != null) {
                            sql.Append(GetHavingString(having, false, state));
                        }
                        sql.AppendFormat(") as RT where {0}>{1}", rname, region.Start);
                        var commandData = new CommandData(sql.ToString());
                        commandData.InnerPage = true;
                        return commandData;
                    }
                }
            }
            return base.CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order, region, state);
        }

        public override string ParameterPrefix => "@";

        public override string CreateRandomOrderBySql(DataEntityMapping mapping, string aliasName, bool fullFieldName)
        {
            return "newid()";
        }

        public override string CreateIdentitySql(DataTableEntityMapping mapping, CreateSqlState state)
        {
            if (mapping.IdentityField != null) {
                return "select @@Identity;";
            }
            else {
                return string.Empty;
            }
        }
    }
}