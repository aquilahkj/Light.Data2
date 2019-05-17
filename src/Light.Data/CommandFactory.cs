using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;

namespace Light.Data
{
    abstract class CommandFactory
    {
        protected string _wildcards = "%";

        protected bool _havingAlias;

        protected bool _orderbyAlias;

        protected Dictionary<QueryPredicate, string> _queryPredicateDict = new Dictionary<QueryPredicate, string>();

        protected Dictionary<QueryCollectionPredicate, string> _queryCollectionPredicateDict = new Dictionary<QueryCollectionPredicate, string>();

        protected Dictionary<JoinType, string> _joinCollectionPredicateDict = new Dictionary<JoinType, string>();


        protected void InitialPredicate()
        {
            _queryPredicateDict[QueryPredicate.Eq] = "=";
            _queryPredicateDict[QueryPredicate.Gt] = ">";
            _queryPredicateDict[QueryPredicate.GtEq] = ">=";
            _queryPredicateDict[QueryPredicate.Lt] = "<";
            _queryPredicateDict[QueryPredicate.LtEq] = "<=";
            _queryPredicateDict[QueryPredicate.NotEq] = "!=";

            _queryCollectionPredicateDict[QueryCollectionPredicate.In] = "in";
            _queryCollectionPredicateDict[QueryCollectionPredicate.NotIn] = "not in";
            _queryCollectionPredicateDict[QueryCollectionPredicate.GtAll] = "> all";
            _queryCollectionPredicateDict[QueryCollectionPredicate.LtAll] = "< all";
            _queryCollectionPredicateDict[QueryCollectionPredicate.GtAny] = "> any";
            _queryCollectionPredicateDict[QueryCollectionPredicate.LtAny] = "< any";
            _queryCollectionPredicateDict[QueryCollectionPredicate.GtEqAll] = ">= all";
            _queryCollectionPredicateDict[QueryCollectionPredicate.LtEqAll] = "<= all";
            _queryCollectionPredicateDict[QueryCollectionPredicate.GtEqAny] = ">= any";
            _queryCollectionPredicateDict[QueryCollectionPredicate.LtEqAny] = "<= any";

            _joinCollectionPredicateDict[JoinType.InnerJoin] = "inner join";
            _joinCollectionPredicateDict[JoinType.LeftJoin] = "left join";
            _joinCollectionPredicateDict[JoinType.RightJoin] = "right join";
        }

        protected CommandCache _batchInsertCache = new CommandCache();

        protected CommandCache _baseInsertCache = new CommandCache();

        protected CommandCache _baseUpdateCache = new CommandCache();

        protected CommandCache _baseDeleteCache = new CommandCache();

        protected CommandCache _selectByIdCache = new CommandCache();

        protected CommandCache _selectByKeyCache = new CommandCache();

        protected CommandCache _existsByKeyCache = new CommandCache();

        protected CommandCache _deleteByKeyCache = new CommandCache();

        public abstract string ParameterPrefix { get; }

        public virtual string GetJoinPredicate(JoinType joinType)
        {
            return _joinCollectionPredicateDict[joinType];
        }

        protected string GetQueryPredicate(QueryPredicate predicate)
        {
            return _queryPredicateDict[predicate];
        }

        protected string GetQueryCollectionPredicate(QueryCollectionPredicate predicate)
        {
            return _queryCollectionPredicateDict[predicate];
        }

        protected OrderExpression CreatePrimaryKeyOrderExpression(DataEntityMapping mapping)
        {
            OrderExpression order = null;
            if (mapping is DataTableEntityMapping tableMapping && tableMapping.HasPrimaryKey) {
                foreach (DataFieldMapping fieldMapping in tableMapping.PrimaryKeyFields) {
                    DataFieldOrderExpression keyOrder = new DataFieldOrderExpression(new DataFieldInfo(fieldMapping), OrderType.ASC);
                    order = OrderExpression.Concat(order, keyOrder);
                }
            }
            return order;
        }

        protected OrderExpression CreateGroupByOrderExpression(AggregateGroupBy groupBy)
        {
            OrderExpression order = null;
            if (groupBy != null && groupBy.FieldCount > 0) {
                order = new DataFieldOrderExpression(groupBy[0], OrderType.ASC);
            }
            return order;
        }

        protected OrderExpression CreateJoinModelListOrderExpression(List<IJoinModel> modelList)
        {
            OrderExpression order = null;
            foreach (IJoinModel model in modelList) {
                if (model.Order != null) {
                    order = OrderExpression.Concat(order, model.Order.CreateAliasTableNameOrder(model.AliasTableName));
                }
            }
            return order;
        }

        protected CommandFactory()
        {
            InitialPredicate();
        }

        protected bool _strictMode = true;

        public void SetStrictMode(bool strictMode)
        {
            _strictMode = strictMode;
        }

        internal virtual string Null {
            get {
                return "null";
            }
        }

        #region 增删改操作命令

        public virtual CommandData CreateSelectByIdCommand(DataTableEntityMapping mapping, object id, CreateSqlState state)
        {
            if (!mapping.HasIdentity) {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }
            if (id == null) {
                throw new LightDataException(SR.KeyNotAllowNull);
            }
            string cachekey = null;
            if (state.Seed == 0 && !state.UseDirectNull) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_selectByIdCache.TryGetCommand(cachekey, out string cache)) {
                    CommandData command1 = new CommandData(cache);
                    DataFieldMapping field = mapping.IdentityField;
                    state.AddDataParameter(this, id, field.DBType, DataParameterMode.Input, field.ObjectType);
                    return command1;
                }
            }
            DataFieldInfo idfield = mapping.IdentityField.DefaultFieldInfo;
            QueryExpression queryExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, idfield, id);
            RelationMap relationMap = mapping.GetRelationMap();
            ISelector selector = relationMap.GetDefaultSelector();
            Region region = new Region(0, 1);
            CommandData command = CreateSelectDataCommand(mapping, relationMap, selector, queryExpression, null, false, region, state);
            if (cachekey != null) {
                _selectByIdCache.SetCommand(cachekey, command.CommandText);
            }
            return command;
        }

        public virtual CommandData CreateSelectByKeyCommand(DataTableEntityMapping mapping, object[] keys, CreateSqlState state)
        {
            if (keys.Length != mapping.PrimaryKeyCount) {
                throw new LightDataException(string.Format(SR.NotMatchPrimaryKeyField, mapping.ObjectType));
            }
            foreach (object key in keys) {
                if (key == null) {
                    throw new LightDataException(SR.KeyNotAllowNull);
                }
            }
            string cachekey = null;
            if (state.Seed == 0 && !state.UseDirectNull) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_selectByKeyCache.TryGetCommand(cachekey, out string cache)) {
                    CommandData command1 = new CommandData(cache);
                    int i = 0;
                    foreach (DataFieldMapping field in mapping.PrimaryKeyFields) {
                        state.AddDataParameter(this, keys[i], field.DBType, DataParameterMode.Input, field.ObjectType);
                        i++;
                    }
                    return command1;
                }
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            QueryExpression queryExpression = null;
            int j = 0;
            foreach (DataFieldMapping fieldMapping in mapping.PrimaryKeyFields) {
                DataFieldInfo info = fieldMapping.DefaultFieldInfo;
                QueryExpression keyExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, keys[j]);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                j++;
            }
            RelationMap relationMap = mapping.GetRelationMap();
            ISelector selector = relationMap.GetDefaultSelector();
            Region region = new Region(0, 1);
            CommandData command = CreateSelectDataCommand(mapping, relationMap, selector, queryExpression, null, false, region, state);
            if (cachekey != null) {
                _selectByKeyCache.SetCommand(cachekey, command.CommandText);
            }
            return command;
        }

        public virtual CommandData CreateExistsByKeyCommand(DataTableEntityMapping mapping, object[] keys, CreateSqlState state)
        {
            if (keys.Length != mapping.PrimaryKeyCount) {
                throw new LightDataException(string.Format(SR.NotMatchPrimaryKeyField, mapping.ObjectType));
            }
            foreach (object key in keys) {
                if (key == null) {
                    throw new LightDataException(SR.KeyNotAllowNull);
                }
            }
            string cachekey = null;
            if (state.Seed == 0 && !state.UseDirectNull) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_existsByKeyCache.TryGetCommand(cachekey, out string cache)) {
                    CommandData command1 = new CommandData(cache);
                    int i = 0;
                    foreach (DataFieldMapping field in mapping.PrimaryKeyFields) {
                        state.AddDataParameter(this, keys[i], field.DBType, DataParameterMode.Input, field.ObjectType);
                        i++;
                    }
                    return command1;
                }
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            QueryExpression queryExpression = null;
            int j = 0;
            foreach (DataFieldMapping fieldMapping in mapping.PrimaryKeyFields) {
                DataFieldInfo info = fieldMapping.DefaultFieldInfo;
                QueryExpression keyExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, keys[j]);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                j++;
            }
            CommandData command = CreateExistsCommand(mapping, queryExpression, state);
            if (cachekey != null) {
                _existsByKeyCache.SetCommand(cachekey, command.CommandText);
            }
            return command;
        }

        public virtual CommandData CreateDeleteKeyCommand(DataTableEntityMapping mapping, object[] keys, CreateSqlState state)
        {
            if (keys.Length != mapping.PrimaryKeyCount) {
                throw new LightDataException(string.Format(SR.NotMatchPrimaryKeyField, mapping.ObjectType));
            }
            foreach (object key in keys) {
                if (key == null) {
                    throw new LightDataException(SR.KeyNotAllowNull);
                }
            }
            string cachekey = null;
            if (state.Seed == 0 && !state.UseDirectNull) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_deleteByKeyCache.TryGetCommand(cachekey, out string cache)) {
                    CommandData command1 = new CommandData(cache);
                    int i = 0;
                    foreach (DataFieldMapping field in mapping.PrimaryKeyFields) {
                        state.AddDataParameter(this, keys[i], field.DBType, DataParameterMode.Input, field.ObjectType);
                        i++;
                    }
                    return command1;
                }
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            QueryExpression queryExpression = null;
            int j = 0;
            foreach (DataFieldMapping fieldMapping in mapping.PrimaryKeyFields) {
                DataFieldInfo info = fieldMapping.DefaultFieldInfo;
                QueryExpression keyExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, keys[j]);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                j++;
            }
            CommandData command = CreateMassDeleteCommand(mapping, queryExpression, state);
            if (cachekey != null) {
                _deleteByKeyCache.SetCommand(cachekey, command.CommandText);
            }
            return command;
        }

        public virtual CommandData CreateBaseInsertCommand(DataTableEntityMapping mapping, object entity, bool refresh, CreateSqlState state)
        {
            string cachekey = null;
            if (state.Seed == 0 && !state.UseDirectNull) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_baseInsertCache.TryGetCommand(cachekey, out string cache)) {
                    CommandData command1 = new CommandData(cache);
                    foreach (DataFieldMapping field in mapping.CreateFieldList) {
                        object value = field.GetInsertData(entity, refresh);
                        state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType);
                    }
                    return command1;
                }
            }

            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            int insertLen = fields.Count;
            if (insertLen == 0) {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            string[] insertList = new string[insertLen];
            string[] valuesList = new string[insertLen];
            for (int i = 0; i < insertLen; i++) {
                DataFieldMapping field = fields[i];
                object value = field.GetInsertData(entity, refresh);
                insertList[i] = CreateDataFieldSql(field.Name);
                valuesList[i] = state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType);
            }
            string insert = string.Join(",", insertList);
            string values = string.Join(",", valuesList);
            string sql = string.Format("insert into {0}({1})values({2})", CreateDataTableMappingSql(mapping, state), insert, values);
            CommandData command = new CommandData(sql);
            if (cachekey != null) {
                _baseInsertCache.SetCommand(cachekey, command.CommandText);
            }
            return command;
        }

        public virtual CommandData CreateBaseUpdateCommand(DataTableEntityMapping mapping, object entity, bool refresh, CreateSqlState state)
        {
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }

            IList<DataFieldMapping> columnFields;
            object[] keys = null;
            DataTableEntity tableEntity = null;
            bool defaultUpdate = false;
            if (mapping.IsDataTableEntity) {
                tableEntity = entity as DataTableEntity;
                keys = tableEntity.GetRawPrimaryKeys();
                string[] updatefieldNames = tableEntity.GetUpdateFields();
                if (updatefieldNames != null && updatefieldNames.Length > 0) {
                    List<DataFieldMapping> updateFields = new List<DataFieldMapping>();
                    foreach (string name in updatefieldNames) {
                        DataFieldMapping fm = mapping.FindDataEntityField(name);
                        if (fm == null) {
                            throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedField, mapping.ObjectType, name));
                        }
                        if (fm is PrimitiveFieldMapping pfm && pfm.IsPrimaryKey && keys == null) {
                            throw new LightDataException(string.Format(SR.UpdateFieldIsPrimaryKeyField, mapping.ObjectType, name));
                        }
                        if ((fm.FunctionControl & FunctionControl.Update) == FunctionControl.Update) {
                            updateFields.Add(fm);
                        }
                    }
                    foreach (DataFieldMapping tm in mapping.TimeStampFieldList) {
                        if (!updateFields.Contains(tm) && (tm.FunctionControl & FunctionControl.Update) == FunctionControl.Update) {
                            updateFields.Add(tm);
                        }
                    }
                    columnFields = updateFields;
                }
                else {
                    if (keys == null) {
                        columnFields = mapping.UpdateFieldList;
                        defaultUpdate = true;
                    }
                    else {
                        List<DataFieldMapping> updateFields = new List<DataFieldMapping>();
                        updateFields.AddRange(mapping.PrimaryKeyFields);
                        updateFields.AddRange(mapping.UpdateFieldList);
                        columnFields = updateFields;
                    }
                }
            }
            else {
                tableEntity = null;
                columnFields = mapping.UpdateFieldList;
                defaultUpdate = true;
            }

            string cachekey = null;
            if (defaultUpdate && state.Seed == 0 && !state.UseDirectNull) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_baseUpdateCache.TryGetCommand(cachekey, out string cache)) {
                    CommandData command1 = new CommandData(cache);
                    foreach (DataFieldMapping field in columnFields) {
                        object value;
                        if (field.IsTimeStamp) {
                            value = field.GetTimeStamp(entity, refresh);
                        }
                        else {
                            object obj = field.Handler.Get(entity);
                            value = field.ToParameter(obj);
                        }
                        state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType);
                    }
                    foreach (DataFieldMapping field in mapping.PrimaryKeyFields) {
                        object obj = field.Handler.Get(entity);
                        object value = field.ToParameter(obj);
                        state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType);
                    }
                    return command1;
                }
            }

            if (columnFields.Count == 0) {
                return null;
            }
            IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;
            int keyLen = keyFields.Count;
            int updateLen = columnFields.Count;

            string[] updateList = new string[updateLen];
            string[] whereList = new string[keyLen];
            for (int i = 0; i < updateLen; i++) {
                DataFieldMapping field = columnFields[i];
                object value;
                if (field.IsTimeStamp) {
                    value = field.GetTimeStamp(entity, refresh);
                }
                else {
                    object obj = field.Handler.Get(entity);
                    value = field.ToParameter(obj);
                }
                updateList[i] = string.Format("{0}={1}", CreateDataFieldSql(field.Name), state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType));
            }
            for (int i = 0; i < keyLen; i++) {
                DataFieldMapping field = keyFields[i];
                object obj = keys == null ? field.Handler.Get(entity) : keys[i];
                //object obj = field.Handler.Get(entity);
                object value = field.ToParameter(obj);
                whereList[i] = string.Format("{0}={1}", CreateDataFieldSql(field.Name), state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType));
            }

            string update = string.Join(",", updateList);
            string where = string.Join(" and ", whereList);
            string sql = string.Format("update {0} set {1} where {2}", CreateDataTableMappingSql(mapping, state), update, where);
            CommandData command = new CommandData(sql);
            if (cachekey != null) {
                _baseUpdateCache.SetCommand(cachekey, command.CommandText);
            }
            return command;
        }

        public virtual CommandData CreateBaseDeleteCommand(DataTableEntityMapping mapping, object entity, CreateSqlState state)
        {
            string cachekey = null;
            if (state.Seed == 0 && !state.UseDirectNull) {
                cachekey = CommandCache.CreateKey(mapping, state);
                if (_baseDeleteCache.TryGetCommand(cachekey, out string cache)) {
                    CommandData command1 = new CommandData(cache);
                    foreach (DataFieldMapping field in mapping.PrimaryKeyFields) {
                        object obj = field.Handler.Get(entity);
                        object value = field.ToParameter(obj);
                        state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType);
                    }
                    return command1;
                }
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;
            int keyLen = keyFields.Count;
            string[] whereList = new string[keyLen];
            for (int i = 0; i < keyLen; i++) {
                DataFieldMapping field = keyFields[i];
                object obj = field.Handler.Get(entity);
                object value = field.ToParameter(obj);
                whereList[i] = string.Format("{0}={1}", CreateDataFieldSql(field.Name), state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType));
            }
            string where = string.Join(" and ", whereList);
            string sql = string.Format("delete from {0} where {1}", CreateDataTableMappingSql(mapping, state), where);
            CommandData command = new CommandData(sql);
            if (cachekey != null) {
                _baseDeleteCache.SetCommand(cachekey, command.CommandText);
            }
            return command;
        }

        public virtual CommandData CreateTruncateTableCommand(DataTableEntityMapping mapping, CreateSqlState state)
        {
            string sql = string.Format("truncate table {0};", CreateDataTableMappingSql(mapping, state));
            CommandData command = new CommandData(sql);
            return command;
        }

        #endregion

        #region 主命令语句块

        public virtual string GetGroupByString(AggregateGroupBy groupby, bool isFullField, CreateSqlState state)
        {
            string queryString = null;
            if (groupby.FieldCount > 0) {
                queryString = string.Format(" group by {0}", groupby.CreateGroupByString(this, isFullField, state));
            }
            return queryString;
        }

        public virtual string GetHavingString(QueryExpression query, bool isFullField, CreateSqlState state)
        {
            string queryString = null;
            if (_havingAlias) {
                state.UseFieldAlias = true;
                queryString = string.Format(" having {0}", query.CreateSqlString(this, isFullField, state));
                state.UseFieldAlias = false;
            }
            else {
                queryString = string.Format(" having {0}", query.CreateSqlString(this, isFullField, state));
            }
            return queryString;
        }

        public virtual string GetQueryString(QueryExpression query, bool isFullField, CreateSqlState state)
        {
            string queryString = string.Format(" where {0}", query.CreateSqlString(this, isFullField, state));
            return queryString;
        }

        public virtual string GetOrderString(OrderExpression order, bool isFullField, CreateSqlState state)
        {
            string orderString = string.Format(" order by {0}", order.CreateSqlString(this, isFullField, state));
            return orderString;
        }

        public virtual string GetAggregateOrderString(OrderExpression order, bool isFullField, CreateSqlState state)
        {
            state.UseFieldAlias = true;
            string orderString = string.Format(" order by {0}", order.CreateSqlString(this, isFullField, state));
            state.UseFieldAlias = false;
            return orderString;
        }

        public virtual string GetOnString(DataFieldExpression on, CreateSqlState state)
        {
            string onString = string.Format(" on {0}", on.CreateSqlString(this, true, state));
            return onString;
        }

        public virtual CommandData CreateSelectCommand(DataEntityMapping mapping, ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region, CreateSqlState state)
        {
            string select;
            if (selector == null) {
                select = "*";
            }
            else {
                select = selector.CreateSelectString(this, false, state);
            }
            if (distinct) {
                select = "distinct " + select;
            }
            CommandData commandData = this.CreateSelectBaseCommand(mapping, select, query, order, region, state);
            return commandData;
        }

        public virtual CommandData CreateSelectDataCommand(DataEntityMapping mapping, RelationMap relationMap, ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region, CreateSqlState state)
        {
            CommandData commandData;
            if (mapping.HasJoinRelateModel) {
                QueryExpression subQuery = null;
                QueryExpression mainQuery = null;
                OrderExpression subOrder = null;
                OrderExpression mainOrder = null;
                if (query != null) {
                    if (query.MutliQuery) {
                        mainQuery = query;
                    }
                    else {
                        subQuery = query;
                    }
                }
                if (order != null) {
                    if (order.MutliOrder) {
                        mainOrder = order;
                    }
                    else {
                        subOrder = order;
                    }
                }
                List<IJoinModel> models = relationMap.CreateJoinModels(subQuery, subOrder);
                commandData = CreateSelectJoinTableCommand(selector, models, mainQuery, mainOrder, distinct, region, state);
            }
            else {
                commandData = CreateSelectCommand(mapping, selector, query, order, distinct, region, state);
            }
            return commandData;
        }

        public virtual CommandData CreateSelectSingleFieldCommand(DataFieldInfo fieldinfo, QueryExpression query, OrderExpression order, bool distinct, Region region, CreateSqlState state)
        {
            string select = fieldinfo.CreateSqlString(this, false, state);
            if (distinct) {
                select = "distinct " + select;
            }
            return CreateSelectBaseCommand(fieldinfo.TableMapping, select, query, order, region, state);
        }

        public virtual CommandData CreateSelectBaseCommand(DataEntityMapping mapping, string customSelect, QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select {0} from {1}", customSelect, CreateDataTableMappingSql(mapping, state));
            if (query != null) {
                sql.Append(GetQueryString(query, false, state));
            }
            if (order != null) {
                sql.Append(GetOrderString(order, false, state));
            }
            CommandData commandData = new CommandData(sql.ToString());
            return commandData;
        }

        public virtual CommandData CreateSelectJoinTableCommand(ISelector selector, List<IJoinModel> modelList, QueryExpression query, OrderExpression order, bool distinct, Region region, CreateSqlState state)
        {
            string selectString = selector.CreateSelectString(this, true, state);
            if (distinct) {
                selectString = "distinct " + selectString;
            }
            OrderExpression subOrder = CreateJoinModelListOrderExpression(modelList);
            if (subOrder != null) {
                order = OrderExpression.Concat(subOrder, order);
            }
            CommandData commandData = CreateSelectJoinTableBaseCommand(selectString, modelList, query, order, region, state);
            return commandData;
        }

        public virtual CommandData CreateSelectJoinTableBaseCommand(string customSelect, List<IJoinModel> modelList, QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            StringBuilder tables = new StringBuilder();
            foreach (IJoinModel model in modelList) {
                if (model.Connect != null) {
                    tables.AppendFormat(" {0} ", _joinCollectionPredicateDict[model.Connect.Type]);
                }
                string modelsql = model.CreateSqlString(this, state);
                tables.Append(modelsql);
                if (model.Connect != null && model.Connect.On != null) {
                    tables.Append(GetOnString(model.Connect.On, state));
                }
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select {0} from {1}", customSelect, tables);
            if (query != null) {
                sql.Append(GetQueryString(query, true, state));
            }
            if (order != null) {
                sql.Append(GetOrderString(order, true, state));
            }
            CommandData command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateAggregateTableCommand(DataEntityMapping mapping, AggregateSelector selector, AggregateGroupBy groupBy, QueryExpression query, QueryExpression having, OrderExpression order, Region region, CreateSqlState state)
        {
            StringBuilder sql = new StringBuilder();
            string selectString = selector.CreateSelectString(this, false, state);
            sql.AppendFormat("select {0} from {1}", selectString, CreateDataTableMappingSql(mapping, state));
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
            CommandData command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateExistsCommand(DataEntityMapping mapping, QueryExpression query, CreateSqlState state)
        {
            Region region = new Region(0, 1);
            return this.CreateSelectBaseCommand(mapping, "1", query, null, region, state);
        }

        public virtual CommandData CreateAggregateFunctionCommand(DataFieldInfo field, AggregateType aggregateType, QueryExpression query, bool distinct, CreateSqlState state)
        {
            DataEntityMapping mapping = field.TableMapping;
            string fieldSql = field.CreateSqlString(this, false, state);
            string function = null;
            switch (aggregateType) {
                case AggregateType.COUNT:
                    function = CreateCountSql(fieldSql, distinct);
                    break;
                case AggregateType.SUM:
                    function = CreateSumSql(fieldSql, distinct);
                    break;
                case AggregateType.AVG:
                    function = CreateAvgSql(fieldSql, distinct);
                    break;
                case AggregateType.MAX:
                    function = CreateMaxSql(fieldSql);
                    break;
                case AggregateType.MIN:
                    function = CreateMinSql(fieldSql);
                    break;
            }
            return CreateSelectBaseCommand(mapping, function, query, null, null, state);
        }

        public virtual CommandData CreateAggregateCountCommand(DataEntityMapping mapping, QueryExpression query, CreateSqlState state)
        {
            string select = CreateCountAllSql();
            return CreateSelectBaseCommand(mapping, select, query, null, null, state);
        }

        public virtual CommandData CreateAggregateJoinCountCommand(List<IJoinModel> modelList, QueryExpression query, CreateSqlState state)
        {
            string select = CreateCountAllSql();
            return CreateSelectJoinTableBaseCommand(select, modelList, query, null, null, state);
        }

        public virtual CommandData CreateMassDeleteCommand(DataTableEntityMapping mapping, QueryExpression query, CreateSqlState state)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("delete from {0}", CreateDataTableMappingSql(mapping, state));
            if (query != null) {
                sql.Append(GetQueryString(query, false, state));
            }
            CommandData command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateMassUpdateCommand(DataTableEntityMapping mapping, MassUpdator updator, QueryExpression query, CreateSqlState state)
        {
            StringBuilder sql = new StringBuilder();
            string setString = updator.CreateSqlString(this, false, state);
            sql.AppendFormat("update {0} set {1}", CreateDataTableMappingSql(mapping, state), setString);
            if (query != null) {
                sql.Append(GetQueryString(query, false, state));
            }
            CommandData command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateSelectInsertCommand(DataTableEntityMapping insertTableMapping, DataEntityMapping selectMapping, QueryExpression query, OrderExpression order, CreateSqlState state)
        {
            StringBuilder sql = new StringBuilder();
            string insertString;
            string selectString;
            ReadOnlyCollection<DataFieldMapping> insertFields;
            ReadOnlyCollection<DataFieldMapping> selectFields;
            if (insertTableMapping.HasIdentity) {
                if (selectMapping is DataTableEntityMapping selectTableEntityMapping && selectTableEntityMapping.HasIdentity) {
                    if (insertTableMapping.FieldCount == selectTableEntityMapping.FieldCount && insertTableMapping.IdentityField.PositionOrder == selectTableEntityMapping.IdentityField.PositionOrder) {
                        insertFields = insertTableMapping.NoIdentityFields;
                        selectFields = selectTableEntityMapping.NoIdentityFields;
                    }
                    else {
                        throw new LightDataException(SR.SelectFieldsCountNotEquidInsertFieldCount);
                    }
                }
                else {
                    if (insertTableMapping.FieldCount == selectMapping.FieldCount + 1) {
                        insertFields = insertTableMapping.NoIdentityFields;
                        selectFields = selectMapping.DataEntityFields;
                    }
                    else {
                        throw new LightDataException(SR.SelectFieldsCountNotEquidInsertFieldCount);
                    }
                }
            }
            else {
                if (insertTableMapping.FieldCount == selectMapping.FieldCount) {
                    insertFields = insertTableMapping.DataEntityFields;
                    selectFields = selectMapping.DataEntityFields;
                }
                else {
                    throw new LightDataException(SR.SelectFieldsCountNotEquidInsertFieldCount);
                }
            }

            string[] insertFieldNames = new string[insertFields.Count];
            for (int i = 0; i < insertFields.Count; i++) {
                insertFieldNames[i] = CreateDataFieldSql(insertFields[i].Name);
            }
            insertString = string.Join(",", insertFieldNames);

            string[] selectFieldNames = new string[selectFields.Count];
            for (int i = 0; i < insertFields.Count; i++) {
                selectFieldNames[i] = CreateDataFieldSql(selectFields[i].Name);
            }
            selectString = string.Join(",", selectFieldNames);
            sql.AppendFormat("insert into {0}({1})", CreateDataTableMappingSql(insertTableMapping, state), insertString);
            sql.AppendFormat("select {0} from {1}", selectString, CreateDataTableMappingSql(selectMapping, state));
            if (query != null) {
                sql.Append(GetQueryString(query, false, state));
            }
            if (order != null) {
                sql.Append(GetOrderString(order, false, state));
            }
            CommandData command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateSelectInsertCommand(InsertSelector insertSelector, DataEntityMapping mapping, QueryExpression query, OrderExpression order, bool distinct, CreateSqlState state)
        {
            CommandData selectCommandData = CreateSelectCommand(mapping, insertSelector, query, order, distinct, null, state);
            DataFieldInfo[] insertFields = insertSelector.GetInsertFields();
            string[] insertFieldNames = new string[insertFields.Length];
            for (int i = 0; i < insertFields.Length; i++) {
                insertFieldNames[i] = CreateDataFieldSql(insertFields[i].FieldName);
            }
            string insertString = string.Join(",", insertFieldNames);
            string sql = string.Format("insert into {0}({1})", CreateDataTableMappingSql(insertSelector.InsertMapping, state), insertString);
            selectCommandData.CommandText = sql + selectCommandData.CommandText;
            return selectCommandData;
        }

        public virtual CommandData CreateSelectInsertCommand(InsertSelector insertSelector, List<IJoinModel> modelList, QueryExpression query, OrderExpression order, bool distinct, CreateSqlState state)
        {
            CommandData selectCommandData = CreateSelectJoinTableCommand(insertSelector, modelList, query, order, distinct, null, state);
            DataFieldInfo[] insertFields = insertSelector.GetInsertFields();
            string[] insertFieldNames = new string[insertFields.Length];
            for (int i = 0; i < insertFields.Length; i++) {
                insertFieldNames[i] = CreateDataFieldSql(insertFields[i].FieldName);
            }
            string insertString = string.Join(",", insertFieldNames);
            string sql = string.Format("insert into {0}({1})", CreateDataTableMappingSql(insertSelector.InsertMapping, state), insertString);
            selectCommandData.CommandText = sql + selectCommandData.CommandText;
            return selectCommandData;
        }

        public virtual CommandData CreateSelectInsertCommand(InsertSelector insertSelector, DataEntityMapping mapping, AggregateSelector selector, AggregateGroupBy groupBy, QueryExpression query, QueryExpression having, OrderExpression order, CreateSqlState state)
        {
            CommandData selectCommandData = CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order, null, state);
            DataFieldInfo[] insertFields = insertSelector.GetInsertFields();
            string[] insertFieldNames = new string[insertFields.Length];
            for (int i = 0; i < insertFields.Length; i++) {
                insertFieldNames[i] = CreateDataFieldSql(insertFields[i].FieldName);
            }
            string insertString = string.Join(",", insertFieldNames);
            string selectString = insertSelector.CreateSelectString(this, false, state);
            string sql = string.Format("insert into {0}({1})select {2} from ({3}) as A", CreateDataTableMappingSql(insertSelector.InsertMapping, state), insertString, selectString, selectCommandData.CommandText);
            selectCommandData.CommandText = sql;
            return selectCommandData;
        }

        public virtual CommandData CreateBatchInsertCommand(DataTableEntityMapping mapping, IList entitys, bool refresh, CreateSqlState state)
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
            foreach (var entity in entitys) {
                string[] valuesList = new string[insertLen];
                for (int i = 0; i < insertLen; i++) {
                    DataFieldMapping field = fields[i];
                    //object obj = field.Handler.Get(entity);
                    //object value = field.ToColumn(obj);
                    object value = field.GetInsertData(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType);
                }
                string values = string.Join(",", valuesList);
                totalSql.AppendFormat("{0}values({1});", insertSql, values);
            }
            CommandData command = new CommandData(totalSql.ToString());
            return command;
        }

        //public virtual Tuple<CommandData, int> CreateBatchInsertCommand(DataTableEntityMapping mapping, IList entitys, int start, int batchCount, bool refresh, CreateSqlState state)
        //{
        //    if (entitys == null || entitys.Count == 0) {
        //        throw new ArgumentNullException(nameof(entitys));
        //    }
        //    int totalCount = entitys.Count;
        //    IList<DataFieldMapping> fields = mapping.CreateFieldList;
        //    int insertLen = fields.Count;
        //    if (insertLen == 0) {
        //        throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
        //    }
        //    string insertSql = null;
        //    string cachekey = null;
        //    if (state.Seed == 0) {
        //        cachekey = CommandCache.CreateKey(mapping, state);
        //        if (_batchInsertCache.TryGetCommand(cachekey, out string cache)) {
        //            insertSql = cache;
        //        }
        //    }
        //    if (insertSql == null) {
        //        string[] insertList = new string[insertLen];
        //        for (int i = 0; i < insertLen; i++) {
        //            DataFieldMapping field = fields[i];
        //            insertList[i] = CreateDataFieldSql(field.Name);
        //        }
        //        string insert = string.Join(",", insertList);
        //        insertSql = string.Format("insert into {0}({1})", CreateDataTableMappingSql(mapping, state), insert);
        //        if (cachekey != null) {
        //            _batchInsertCache.SetCommand(cachekey, insertSql);
        //        }
        //    }
        //    int createCount = 0;

        //    StringBuilder totalSql = new StringBuilder();
        //    int end = start + batchCount;
        //    if (end > totalCount) {
        //        end = totalCount;
        //    }
        //    for (int index = start; index < end; index++) {
        //        object entity = entitys[index];
        //        string[] valuesList = new string[insertLen];
        //        for (int i = 0; i < insertLen; i++) {
        //            DataFieldMapping field = fields[i];
        //            //object obj = field.Handler.Get(entity);
        //            //object value = field.ToColumn(obj);
        //            object value = field.GetInsertData(entity, refresh);
        //            valuesList[i] = state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType);
        //        }
        //        string values = string.Join(",", valuesList);
        //        totalSql.AppendFormat("{0}values({1});", insertSql, values);
        //        createCount++;
        //    }
        //    CommandData command = new CommandData(totalSql.ToString());
        //    Tuple<CommandData, int> result = new Tuple<CommandData, int>(command, createCount);
        //    return result;
        //}


        public virtual CommandData CreateBatchUpdateCommand(DataTableEntityMapping mapping, IList entitys, bool refresh, CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0) {
                throw new ArgumentNullException(nameof(entitys));
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            //if (mapping.UpdateFieldList.Count == 0 && !mapping.IsDataTableEntity) {
            //    throw new LightDataException(string.Format(SR.NotContainNonPrimaryKeyFields, mapping.ObjectType));
            //}

            IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;
            int keyLen = keyFields.Count;

            int totalCount = entitys.Count;
            int createCount = 0;

            StringBuilder totalSql = new StringBuilder();

            foreach (var entity in entitys) {
                IList<DataFieldMapping> columnFields;
                object[] keys = null;
                DataTableEntity tableEntity = null;
                if (mapping.IsDataTableEntity) {
                    tableEntity = entity as DataTableEntity;
                    keys = tableEntity.GetRawPrimaryKeys();
                    string[] updatefieldNames = tableEntity.GetUpdateFields();
                    if (updatefieldNames != null && updatefieldNames.Length > 0) {
                        List<DataFieldMapping> updateFields = new List<DataFieldMapping>();
                        foreach (string name in updatefieldNames) {
                            DataFieldMapping fm = mapping.FindDataEntityField(name);
                            if (fm == null) {
                                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedField, mapping.ObjectType, name));
                            }
                            if (fm is PrimitiveFieldMapping pfm && pfm.IsPrimaryKey && keys == null) {
                                throw new LightDataException(string.Format(SR.UpdateFieldIsPrimaryKeyField, mapping.ObjectType, name));
                            }
                            if ((fm.FunctionControl & FunctionControl.Update) == FunctionControl.Update) {
                                updateFields.Add(fm);
                            }
                        }
                        foreach (DataFieldMapping tm in mapping.TimeStampFieldList) {
                            if (!updateFields.Contains(tm) && (tm.FunctionControl & FunctionControl.Update) == FunctionControl.Update) {
                                updateFields.Add(tm);
                            }
                        }
                        columnFields = updateFields;
                    }
                    else {
                        if (keys == null) {
                            columnFields = mapping.UpdateFieldList;
                        }
                        else {
                            List<DataFieldMapping> updateFields = new List<DataFieldMapping>();
                            updateFields.AddRange(mapping.PrimaryKeyFields);
                            updateFields.AddRange(mapping.UpdateFieldList);
                            columnFields = updateFields;
                        }
                    }
                }
                else {
                    tableEntity = null;
                    columnFields = mapping.UpdateFieldList;
                }
                if (columnFields.Count == 0) {
                    continue;
                }
                int updateLen = columnFields.Count;
                string[] updateList = new string[updateLen];
                string[] whereList = new string[keyLen];
                for (int i = 0; i < updateLen; i++) {
                    DataFieldMapping field = columnFields[i];
                    object value;
                    if (field.IsTimeStamp) {
                        value = field.GetTimeStamp(entity, refresh);
                    }
                    else {
                        object obj = field.Handler.Get(entity);
                        value = field.ToParameter(obj);
                    }
                    updateList[i] = string.Format("{0}={1}", CreateDataFieldSql(field.Name), state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType));
                }
                for (int i = 0; i < keyLen; i++) {
                    DataFieldMapping field = keyFields[i];
                    object obj = keys == null ? field.Handler.Get(entity) : keys[i];
                    //object obj = field.Handler.Get(entity);
                    object value = field.ToParameter(obj);
                    whereList[i] = string.Format("{0}={1}", CreateDataFieldSql(field.Name), state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType));
                }

                string update = string.Join(",", updateList);
                string where = string.Join(" and ", whereList);
                totalSql.AppendFormat("update {0} set {1} where {2};", CreateDataTableMappingSql(mapping, state), update, where);
                createCount++;
            }
            if (createCount == 0) {
                return null;
            }
            CommandData command = new CommandData(totalSql.ToString());
            return command;
        }



        //public virtual Tuple<CommandData, int> CreateBatchUpdateCommand(DataTableEntityMapping mapping, IList entitys, int start, int batchCount, bool refresh, CreateSqlState state)
        //{
        //    if (entitys == null || entitys.Count == 0) {
        //        throw new ArgumentNullException(nameof(entitys));
        //    }
        //    if (!mapping.HasPrimaryKey) {
        //        throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
        //    }
        //    //if (mapping.UpdateFieldList.Count == 0 && !mapping.IsDataTableEntity) {
        //    //    throw new LightDataException(string.Format(SR.NotContainNonPrimaryKeyFields, mapping.ObjectType));
        //    //}

        //    IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;
        //    int keyLen = keyFields.Count;

        //    int totalCount = entitys.Count;
        //    int createCount = 0;

        //    StringBuilder totalSql = new StringBuilder();
        //    int end = start + batchCount;
        //    if (end > totalCount) {
        //        end = totalCount;
        //    }
        //    for (int index = start; index < end; index++) {
        //        createCount++;
        //        object entity = entitys[index];
        //        IList<DataFieldMapping> columnFields;
        //        object[] keys = null;
        //        DataTableEntity tableEntity = null;
        //        if (mapping.IsDataTableEntity) {
        //            tableEntity = entity as DataTableEntity;
        //            keys = tableEntity.GetRawPrimaryKeys();
        //            string[] updatefieldNames = tableEntity.GetUpdateFields();
        //            if (updatefieldNames != null && updatefieldNames.Length > 0) {
        //                List<DataFieldMapping> updateFields = new List<DataFieldMapping>();
        //                foreach (string name in updatefieldNames) {
        //                    DataFieldMapping fm = mapping.FindDataEntityField(name);
        //                    if (fm == null) {
        //                        throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedField, mapping.ObjectType, name));
        //                    }
        //                    if (fm is PrimitiveFieldMapping pfm && pfm.IsPrimaryKey && keys == null) {
        //                        throw new LightDataException(string.Format(SR.UpdateFieldIsPrimaryKeyField, mapping.ObjectType, name));
        //                    }
        //                    if ((fm.FunctionControl & FunctionControl.Update) == FunctionControl.Update) {
        //                        updateFields.Add(fm);
        //                    }
        //                }
        //                foreach (DataFieldMapping tm in mapping.TimeStampFieldList) {
        //                    if (!updateFields.Contains(tm) && (tm.FunctionControl & FunctionControl.Update) == FunctionControl.Update) {
        //                        updateFields.Add(tm);
        //                    }
        //                }
        //                columnFields = updateFields;
        //            }
        //            else {
        //                if (keys == null) {
        //                    columnFields = mapping.UpdateFieldList;
        //                }
        //                else {
        //                    List<DataFieldMapping> updateFields = new List<DataFieldMapping>();
        //                    updateFields.AddRange(mapping.PrimaryKeyFields);
        //                    updateFields.AddRange(mapping.UpdateFieldList);
        //                    columnFields = updateFields;
        //                }
        //            }
        //        }
        //        else {
        //            tableEntity = null;
        //            columnFields = mapping.UpdateFieldList;
        //        }
        //        if (columnFields.Count == 0) {
        //            continue;
        //        }
        //        int updateLen = columnFields.Count;
        //        string[] updateList = new string[updateLen];
        //        string[] whereList = new string[keyLen];
        //        for (int i = 0; i < updateLen; i++) {
        //            DataFieldMapping field = columnFields[i];
        //            object value;
        //            if (field.IsTimeStamp) {
        //                value = field.GetTimeStamp(entity, refresh);
        //            }
        //            else {
        //                object obj = field.Handler.Get(entity);
        //                value = field.ToParameter(obj);
        //            }
        //            updateList[i] = string.Format("{0}={1}", CreateDataFieldSql(field.Name), state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType));
        //        }
        //        for (int i = 0; i < keyLen; i++) {
        //            DataFieldMapping field = keyFields[i];
        //            object obj = keys == null ? field.Handler.Get(entity) : keys[i];
        //            //object obj = field.Handler.Get(entity);
        //            object value = field.ToParameter(obj);
        //            whereList[i] = string.Format("{0}={1}", CreateDataFieldSql(field.Name), state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType));
        //        }

        //        string update = string.Join(",", updateList);
        //        string where = string.Join(" and ", whereList);
        //        totalSql.AppendFormat("update {0} set {1} where {2};", CreateDataTableMappingSql(mapping, state), update, where);

        //    }
        //    if (totalSql.Length == 0) {
        //        return new Tuple<CommandData, int>(null, createCount);
        //    }
        //    CommandData command = new CommandData(totalSql.ToString());
        //    Tuple<CommandData, int> result = new Tuple<CommandData, int>(command, createCount);
        //    return result;
        //}

        public virtual CommandData CreateBatchDeleteCommand(DataTableEntityMapping mapping, IList entitys, CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0) {
                throw new ArgumentNullException(nameof(entitys));
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }

            IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;
            int keyLen = keyFields.Count;
            StringBuilder totalSql = new StringBuilder();
            foreach (object entity in entitys) {
                string[] whereList = new string[keyLen];
                for (int i = 0; i < keyLen; i++) {
                    DataFieldMapping field = keyFields[i];
                    object obj = field.Handler.Get(entity);
                    object value = field.ToParameter(obj);
                    whereList[i] = string.Format("{0}={1}", CreateDataFieldSql(field.Name), state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType));
                }
                string where = string.Join(" and ", whereList);
                totalSql.AppendFormat("delete from {0} where {1};", CreateDataTableMappingSql(mapping, state), where);

            }
            CommandData command = new CommandData(totalSql.ToString());
            return command;
        }

        //public virtual Tuple<CommandData, int> CreateBatchDeleteCommand(DataTableEntityMapping mapping, IList entitys, int start, int batchCount, CreateSqlState state)
        //{
        //    if (entitys == null || entitys.Count == 0) {
        //        throw new ArgumentNullException(nameof(entitys));
        //    }
        //    if (!mapping.HasPrimaryKey) {
        //        throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
        //    }

        //    IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;
        //    int keyLen = keyFields.Count;

        //    int totalCount = entitys.Count;
        //    int createCount = 0;

        //    StringBuilder totalSql = new StringBuilder();
        //    int end = start + batchCount;
        //    if (end > totalCount) {
        //        end = totalCount;
        //    }
        //    for (int index = start; index < end; index++) {
        //        object entity = entitys[index];
        //        string[] whereList = new string[keyLen];
        //        for (int i = 0; i < keyLen; i++) {
        //            DataFieldMapping field = keyFields[i];
        //            object obj = field.Handler.Get(entity);
        //            object value = field.ToParameter(obj);
        //            whereList[i] = string.Format("{0}={1}", CreateDataFieldSql(field.Name), state.AddDataParameter(this, value, field.DBType, DataParameterMode.Input, field.ObjectType));
        //        }
        //        string where = string.Join(" and ", whereList);
        //        totalSql.AppendFormat("delete from {0} where {1};", CreateDataTableMappingSql(mapping, state), where);
        //        createCount++;
        //    }
        //    CommandData command = new CommandData(totalSql.ToString());
        //    Tuple<CommandData, int> result = new Tuple<CommandData, int>(command, createCount);
        //    return result;
        //}

        public virtual CommandData CreateIdentityCommand(DataTableEntityMapping mapping, CreateSqlState state)
        {
            string sql = CreateIdentitySql(mapping, state);
            if (!string.IsNullOrEmpty(sql)) {
                CommandData command = new CommandData(sql);
                return command;
            }
            else {
                return null;
            }
        }


        #endregion

        #region 基本语句块

        public virtual string CreateConcatExpressionSql(string expressionString1, string expressionString2, ConcatOperatorType operatorType)
        {
            return string.Format("({0} {2} {1})", expressionString1, expressionString2, operatorType.ToString().ToLower());
        }

        public virtual string CreateConcatExpressionSql(string[] expressionStrings)
        {
            return string.Join(",", expressionStrings);
        }

        public virtual string CreateSingleParamSql(object fieldName, QueryPredicate predicate, bool isReverse, string name)
        {
            StringBuilder sb = new StringBuilder();
            string op = GetQueryPredicate(predicate);
            if (!isReverse) {
                sb.AppendFormat("{0}{2}{1}", fieldName, name, op);
            }
            else {
                sb.AppendFormat("{1}{2}{0}", fieldName, name, op);
            }
            return sb.ToString();
        }

        public virtual string CreateRelationTableSql(object fieldName, QueryPredicate predicate, bool isReverse, string relationFieldName)
        {
            StringBuilder sb = new StringBuilder();
            string op = GetQueryPredicate(predicate);
            if (!isReverse) {
                sb.AppendFormat("{0}{2}{1}", fieldName, relationFieldName, op);
            }
            else {
                sb.AppendFormat("{1}{2}{0}", fieldName, relationFieldName, op);
            }
            return sb.ToString();
        }

        public virtual string CreateCollectionParamsQuerySql(object fieldName, QueryCollectionPredicate predicate, IEnumerable<object> list)
        {
            string op = GetQueryCollectionPredicate(predicate);

            int i = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1} (", fieldName, op);
            foreach (object item in list) {
                if (i > 0)
                    sb.Append(",");
                sb.Append(item);
                i++;
            }
            sb.Append(")");
            return sb.ToString();
        }

        public virtual string CreateExistsQuerySql(string queryTableName, string whereString, bool isNot)
        {
            return string.Format("{2}exists (select 1 from {0} where {1})", queryTableName, whereString, isNot ? "not " : string.Empty);
        }

        public virtual string CreateNotQuerySql(string whereString)
        {
            return string.Format("not({0})", whereString);
        }

        public virtual string CreateSubQuerySql(object fieldName, QueryCollectionPredicate predicate, string queryfieldName, string queryTableName, string whereString)
        {
            StringBuilder sb = new StringBuilder();
            string op = GetQueryCollectionPredicate(predicate);
            sb.AppendFormat("{0} {3} (select {1} from {2}", fieldName, queryfieldName, queryTableName, op);
            if (!string.IsNullOrEmpty(whereString)) {
                sb.AppendFormat(" where {0}", whereString);
            }
            sb.Append(")");
            return sb.ToString();
        }

        public virtual string CreateBetweenParamsQuerySql(object fieldName, bool isNot, string fromParam, string toParam)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {3}between {1} and {2}", fieldName, fromParam, toParam, isNot ? string.Empty : "not ");
            return sb.ToString();
        }

        public virtual string CreateSingleParamSql(object left, QueryPredicate predicate, object right)
        {
            string op = GetQueryPredicate(predicate);
            string sql = string.Format("{0}{2}{1}", left, right, op);
            return sql;
        }

        public virtual string CreateBooleanQuerySql(object field, bool isTrue, bool isEqual, bool isReverse)
        {
            if (!isReverse) {
                return string.Format("{0}{2}{1}", field, isTrue ? "1" : "0", isEqual ? "=" : "!=");
            }
            else {
                return string.Format("{1}{2}{0}", field, isTrue ? "1" : "0", isEqual ? "=" : "!=");
            }
        }

        public virtual string CreateNotSql(object value)
        {
            string sql = string.Format("not({0})", value);
            return sql;
        }

        public virtual string CreateConcatSql(params object[] values)
        {
            string value1 = string.Join("+", values);
            string sql = string.Format("({0})", value1);
            return sql;
        }

        public virtual string CreateLikeMatchQuerySql(object left, object right, bool starts, bool ends, bool isNot)
        {
            string value1 = CreateMatchSql(right.ToString(), starts, ends);
            string sql = string.Format("{0} {2}like {1}", left, value1, isNot ? "not " : string.Empty);
            return sql;
        }

        public virtual string CreateCollectionMatchQuerySql(object fieldName, bool isReverse, bool starts, bool ends, bool isNot, IEnumerable<object> list)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();

            foreach (string item in list) {
                if (i > 0) {
                    if (isNot) {
                        sb.Append(" and ");
                    }
                    else {
                        sb.Append(" or ");
                    }
                }
                if (!isReverse) {
                    string value1 = CreateMatchSql(item, starts, ends);
                    sb.AppendFormat("{0} {2}like {1}", fieldName, value1, isNot ? "not " : string.Empty);
                }
                else {
                    sb.AppendFormat("{1} {2}like {0}", fieldName, item, isNot ? "not " : string.Empty);
                }
                i++;
            }
            if (i > 1) {
                sb.Insert(0, "(");
                sb.Append(")");
            }
            return sb.ToString();
        }

        public virtual string CreateNullQuerySql(object fieldName, bool isNull)
        {
            return string.Format("{0} is{1} null", fieldName, isNull ? string.Empty : " not");
        }

        public virtual string CreateBooleanQuerySql(object fieldName, bool isTrue)
        {
            return string.Format("{0}={1}", fieldName, isTrue ? "1" : "0");
        }

        public virtual string CreateOrderBySql(object fieldName, OrderType orderType)
        {
            return string.Format("{0} {1}", fieldName, orderType.ToString().ToLower());
        }

        public virtual string CreateRandomOrderBySql(DataEntityMapping mapping, string aliasName, bool fullFieldName)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateIdentitySql(DataTableEntityMapping mapping, CreateSqlState state)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateSelectAllSql()
        {
            return "*";
        }

        public virtual string CreateSelectExistsSql()
        {
            return "1";
        }

        public virtual string CreateCountAllSql()
        {
            return "count(1)";
        }

        public virtual string CreateCountAllSql(string expressionSql)
        {
            return string.Format("count(case when {0} then 1 else null end)", expressionSql);
        }

        public virtual string CreateConditionCountSql(string expressionSql, object fieldName, bool isDistinct)
        {
            return string.Format("count({2}case when {0} then {1} else null end)", expressionSql, fieldName, isDistinct ? "distinct " : "");
        }

        public virtual string CreateCountSql(object fieldName, bool isDistinct)
        {
            return string.Format("count({1}{0})", fieldName, isDistinct ? "distinct " : "");
        }

        public virtual string CreateSumSql(object fieldName, bool isDistinct)
        {
            return string.Format("sum({1}{0})", fieldName, isDistinct ? "distinct " : "");
        }

        public virtual string CreateConditionSumSql(string expressionSql, object fieldName, bool isDistinct)
        {
            return string.Format("sum({2}case when {0} then {1} else null end)", expressionSql, fieldName, isDistinct ? "distinct " : "");
        }

        public virtual string CreateAvgSql(object fieldName, bool isDistinct)
        {
            return string.Format("avg({1}{0})", fieldName, isDistinct ? "distinct " : "");
        }

        public virtual string CreateConditionAvgSql(string expressionSql, object fieldName, bool isDistinct)
        {
            return string.Format("avg({2}case when {0} then {1} else null end)", expressionSql, fieldName, isDistinct ? "distinct " : "");
        }

        public virtual string CreateMaxSql(object fieldName)
        {
            return string.Format("max({0})", fieldName);
        }

        public virtual string CreateConditionMaxSql(string expressionSql, object fieldName)
        {
            return string.Format("max(case when {0} then {1} else null end)", expressionSql, fieldName);
        }

        public virtual string CreateMinSql(object fieldName)
        {
            return string.Format("min({0})", fieldName);
        }

        public virtual string CreateConditionMinSql(string expressionSql, object fieldName)
        {
            return string.Format("min(case when {0} then {1} else null end)", expressionSql, fieldName);
        }

        public virtual string CreateAliasFieldSql(string field, string alias)
        {
            return string.Format("{0} as {1}", field, CreateDataFieldSql(alias));
        }

        public virtual string CreateAliasTableSql(string field, string alias)
        {
            return string.Format("{0} as {1}", field, CreateDataFieldSql(alias));
        }

        public virtual string CreateAliasQuerySql(string query, string alias)
        {
            return string.Format("({0}) as {1}", query, CreateDataFieldSql(alias));
        }

        public virtual string CreateDataFieldSql(string fieldName)
        {
            return fieldName;
        }

        public virtual string CreateDataTableSql(string tableName)
        {
            return tableName;
        }

        public string CreateDataTableMappingSql(DataEntityMapping mapping, CreateSqlState state)
        {
            if (state.TryGetAliasTableName(mapping, out string name)) {
                return CreateDataTableSql(name);
            }
            else {
                return CreateDataTableSql(mapping.TableName);
            }
        }

        public virtual string CreateFullDataFieldSql(DataEntityMapping mapping, string fieldName, CreateSqlState state)
        {
            return string.Format("{0}.{1}", CreateDataTableMappingSql(mapping, state), CreateDataFieldSql(fieldName));
        }

        public virtual string CreateFullDataFieldSql(string tableName, string fieldName)
        {
            return string.Format("{0}.{1}", CreateDataTableSql(tableName), CreateDataFieldSql(fieldName));
        }

        public virtual string CreateMatchSql(object field, bool starts, bool ends)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateDateSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateDateTimeFormatSql(object field, string format)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateYearSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateMonthSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateDaySql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateHourSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateMinuteSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateSecondSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateWeekSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateWeekDaySql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateYearDaySql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateLengthSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateSubStringSql(object field, object start, object size)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateIndexOfSql(object field, object value, object startIndex)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateReplaceSql(object field, object oldValue, object newValue)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateToLowerSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateToUpperSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateTrimSql(object field)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateDataBaseTimeSql()
        {
            throw new NotSupportedException();
        }

        public virtual string CreateNullSql()
        {
            return "null";
        }

        public virtual string CreateNumberSql(object value)
        {
            return value.ToString();
        }

        public virtual string CreateDualConcatSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("({0}+{1})", field, value);
            }
            else {
                return string.Format("({0}+{1})", value, field);
            }
        }

        public virtual string CreatePlusSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("({0}+{1})", field, value);
            }
            else {
                return string.Format("({0}+{1})", value, field);
            }
        }

        public virtual string CreateMinusSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("({0}-{1})", field, value);
            }
            else {
                return string.Format("({0}-{1})", value, field);
            }
        }

        public virtual string CreateMultiplySql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("({0}*{1})", field, value);
            }
            else {
                return string.Format("({0}*{1})", value, field);
            }
        }

        public virtual string CreateDividedSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("({0}/{1})", field, value);
            }
            else {
                return string.Format("({0}/{1})", value, field);
            }
        }

        public virtual string CreateModSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("({0}%{1})", field, value);
            }
            else {
                return string.Format("({0}%{1})", value, field);
            }
        }

        public virtual string CreatePowerSql(object field, object value, bool forward)
        {
            if (forward) {
                return string.Format("({0}^{1})", field, value);
            }
            else {
                return string.Format("({0}^{1})", value, field);
            }
        }

        public virtual string CreatePlusSql(object left, object right)
        {
            return string.Format("({0}+{1})", left, right);
        }

        public virtual string CreateMinusSql(object left, object right)
        {
            return string.Format("({0}-{1})", left, right);
        }

        public virtual string CreateMultiplySql(object left, object right)
        {
            return string.Format("({0}*{1})", left, right);
        }

        public virtual string CreateDividedSql(object left, object right)
        {
            return string.Format("({0}/{1})", left, right);
        }

        public virtual string CreateModSql(object left, object right)
        {
            return string.Format("({0}%{1})", left, right);
        }

        public virtual string CreatePowerSql(object left, object right)
        {
            return string.Format("({0}^{1})", left, right);
        }

        public virtual string CreateCastStringSql(object field, string format)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateAbsSql(object field)
        {
            return string.Format("abs({0})", field);
        }

        public virtual string CreateSignSql(object field)
        {
            return string.Format("sign({0})", field);
        }

        public virtual string CreateLogSql(object field)
        {
            return string.Format("log({0})", field);
        }

        public virtual string CreateLogSql(object field, object value)
        {
            return string.Format("log({0},{1})", field, value);
        }

        public virtual string CreateLog10Sql(object field)
        {
            return string.Format("log10({0})", field);
        }

        public virtual string CreateExpSql(object field)
        {
            return string.Format("exp({0})", field);
        }

        public virtual string CreatePowSql(object field, object value)
        {
            return string.Format("power({0},{1})", field, value);
        }

        public virtual string CreateSinSql(object field)
        {
            return string.Format("sin({0})", field);
        }

        public virtual string CreateCosSql(object field)
        {
            return string.Format("cos({0})", field);
        }

        public virtual string CreateAsinSql(object field)
        {
            return string.Format("asin({0})", field);
        }

        public virtual string CreateAcosSql(object field)
        {
            return string.Format("acos({0})", field);
        }

        public virtual string CreateTanSql(object field)
        {
            return string.Format("tan({0})", field);
        }

        public virtual string CreateAtanSql(object field)
        {
            return string.Format("atan({0})", field);
        }

        public virtual string CreateAtan2Sql(object field, object value)
        {
            return string.Format("atan2({0},{1})", field, value);
        }

        public virtual string CreateCeilingSql(object field)
        {
            return string.Format("ceiling({0})", field);
        }

        public virtual string CreateFloorSql(object field)
        {
            return string.Format("floor({0})", field);
        }

        public virtual string CreateRoundSql(object field, object value)
        {
            return string.Format("round({0},{1})", field, value);
        }

        public virtual string CreateTruncateSql(object field)
        {
            return string.Format("truncate({0},0)", field);
        }

        public virtual string CreateSqrtSql(object field)
        {
            return string.Format("Sqrt({0})", field);
        }

        public virtual string CreateMaxSql(object left, object right)
        {
            return string.Format("(case when {0}>{1} then {0} else {1} end)", left, right);
        }

        public virtual string CreateMinSql(object left, object right)
        {
            return string.Format("(case when {0}<{1} then {0} else {1} end)", left, right);
        }

        public virtual string CreateConditionSql(string querySql, object ifTrue, object IfFalse)
        {
            return string.Format("case when {0} then {1} else {2} end", querySql, ifTrue, IfFalse);
        }

        #endregion


        public virtual string CreateJoinOnMatchSql(string leftField, QueryPredicate predicate, string rightField)
        {
            StringBuilder sb = new StringBuilder();
            string op = GetQueryPredicate(predicate);
            sb.AppendFormat("{0}{2}{1}", leftField, rightField, op);
            return sb.ToString();
        }

        public virtual string CreateParamName(string name)
        {
            if (!name.StartsWith(ParameterPrefix, StringComparison.Ordinal)) {
                return ParameterPrefix + name;
            }
            else {
                return name;
            }
        }

        public virtual string CreateStringWrap(object value)
        {
            return string.Format("'{0}'", value);
        }
    }
}