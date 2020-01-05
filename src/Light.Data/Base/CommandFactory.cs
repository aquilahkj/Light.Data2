using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Light.Data
{
    internal abstract class CommandFactory
    {
        protected string Wildcards { get; } = "%";

        protected bool HavingAlias { get; set; }

        protected bool OrderByAlias { get; set; }

        protected readonly Dictionary<QueryPredicate, string> _queryPredicateDict =
            new Dictionary<QueryPredicate, string>();

        protected readonly Dictionary<QueryCollectionPredicate, string> _queryCollectionPredicateDict =
            new Dictionary<QueryCollectionPredicate, string>();

        protected readonly Dictionary<JoinType, string> _joinCollectionPredicateDict =
            new Dictionary<JoinType, string>();


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

        protected readonly CommandCache _batchInsertCache = new CommandCache();

        protected readonly CommandCache _baseInsertCache = new CommandCache();

        protected readonly CommandCache _baseUpdateCache = new CommandCache();

        protected readonly CommandCache _baseDeleteCache = new CommandCache();

        protected readonly CommandCache _selectByIdCache = new CommandCache();

        protected readonly CommandCache _selectByKeyCache = new CommandCache();

        protected readonly CommandCache _existsByKeyCache = new CommandCache();

        protected readonly CommandCache _deleteByKeyCache = new CommandCache();

        public abstract string ParameterPrefix { get; }

        public virtual int MaxParameterCount => int.MaxValue;

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
            if (mapping is DataTableEntityMapping tableMapping && tableMapping.HasPrimaryKey)
            {
                foreach (var fieldMapping in tableMapping.PrimaryKeyFields)
                {
                    var keyOrder = new DataFieldOrderExpression(new DataFieldInfo(fieldMapping), OrderType.ASC);
                    order = OrderExpression.Concat(order, keyOrder);
                }
            }

            return order;
        }

        protected OrderExpression CreateGroupByOrderExpression(AggregateGroupBy groupBy)
        {
            OrderExpression order = null;
            if (groupBy != null && groupBy.FieldCount > 0)
            {
                order = new DataFieldOrderExpression(groupBy[0], OrderType.ASC);
            }

            return order;
        }

        protected OrderExpression CreateJoinModelListOrderExpression(List<IJoinModel> modelList)
        {
            OrderExpression order = null;
            foreach (var model in modelList)
            {
                if (model.Order != null)
                {
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

        internal virtual string Null => "null";

        public virtual bool SupportBatchInsertIdentity => false;

        #region 增删改操作命令

        public virtual CommandData CreateSelectByIdCommand(DataTableEntityMapping mapping, object id,
            CreateSqlState state)
        {
            if (!mapping.HasIdentity)
            {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            if (id == null)
            {
                throw new LightDataException(SR.KeyNotAllowNull);
            }

            string cacheKey = null;
            if (state.Seed == 0 && !state.UseDirectNull)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_selectByIdCache.TryGetCommand(cacheKey, out var cache))
                {
                    var command1 = new CommandData(cache);
                    var field = mapping.IdentityField;
                    state.AddDataParameter(this, id, field.DBType, field.ObjectType);
                    return command1;
                }
            }

            var idField = mapping.IdentityField.DefaultFieldInfo;
            QueryExpression queryExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, idField, id);
            var relationMap = mapping.GetRelationMap();
            var selector = relationMap.GetDefaultSelector();
            var region = new Region(0, 1);
            var command = CreateSelectDataCommand(mapping, relationMap, selector, queryExpression, null, false, region,
                state);
            if (cacheKey != null)
            {
                _selectByIdCache.SetCommand(cacheKey, command.CommandText);
            }

            return command;
        }

        public virtual CommandData CreateSelectByKeyCommand(DataTableEntityMapping mapping, object[] keys,
            CreateSqlState state)
        {
            if (keys.Length != mapping.PrimaryKeyCount)
            {
                throw new LightDataException(string.Format(SR.NotMatchPrimaryKeyField, mapping.ObjectType));
            }

            foreach (var key in keys)
            {
                if (key == null)
                {
                    throw new LightDataException(SR.KeyNotAllowNull);
                }
            }

            string cacheKey = null;
            if (state.Seed == 0 && !state.UseDirectNull)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_selectByKeyCache.TryGetCommand(cacheKey, out var cache))
                {
                    var command1 = new CommandData(cache);
                    var i = 0;
                    foreach (var field in mapping.PrimaryKeyFields)
                    {
                        state.AddDataParameter(this, keys[i], field.DBType, field.ObjectType);
                        i++;
                    }

                    return command1;
                }
            }

            if (!mapping.HasPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }

            QueryExpression queryExpression = null;
            var j = 0;
            foreach (var fieldMapping in mapping.PrimaryKeyFields)
            {
                var info = fieldMapping.DefaultFieldInfo;
                QueryExpression keyExpression =
                    new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, keys[j]);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                j++;
            }

            var relationMap = mapping.GetRelationMap();
            var selector = relationMap.GetDefaultSelector();
            var region = new Region(0, 1);
            var command = CreateSelectDataCommand(mapping, relationMap, selector, queryExpression, null, false, region,
                state);
            if (cacheKey != null)
            {
                _selectByKeyCache.SetCommand(cacheKey, command.CommandText);
            }

            return command;
        }

        public virtual CommandData CreateExistsByKeyCommand(DataTableEntityMapping mapping, object[] keys,
            CreateSqlState state)
        {
            if (keys.Length != mapping.PrimaryKeyCount)
            {
                throw new LightDataException(string.Format(SR.NotMatchPrimaryKeyField, mapping.ObjectType));
            }

            foreach (var key in keys)
            {
                if (key == null)
                {
                    throw new LightDataException(SR.KeyNotAllowNull);
                }
            }

            string cacheKey = null;
            if (state.Seed == 0 && !state.UseDirectNull)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_existsByKeyCache.TryGetCommand(cacheKey, out var cache))
                {
                    var command1 = new CommandData(cache);
                    var i = 0;
                    foreach (var field in mapping.PrimaryKeyFields)
                    {
                        state.AddDataParameter(this, keys[i], field.DBType, field.ObjectType);
                        i++;
                    }

                    return command1;
                }
            }

            if (!mapping.HasPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }

            QueryExpression queryExpression = null;
            var j = 0;
            foreach (var fieldMapping in mapping.PrimaryKeyFields)
            {
                var info = fieldMapping.DefaultFieldInfo;
                QueryExpression keyExpression =
                    new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, keys[j]);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                j++;
            }

            var command = CreateExistsCommand(mapping, queryExpression, state);
            if (cacheKey != null)
            {
                _existsByKeyCache.SetCommand(cacheKey, command.CommandText);
            }

            return command;
        }

        public virtual CommandData CreateDeleteKeyCommand(DataTableEntityMapping mapping, object[] keys,
            CreateSqlState state)
        {
            if (keys.Length != mapping.PrimaryKeyCount)
            {
                throw new LightDataException(string.Format(SR.NotMatchPrimaryKeyField, mapping.ObjectType));
            }

            foreach (var key in keys)
            {
                if (key == null)
                {
                    throw new LightDataException(SR.KeyNotAllowNull);
                }
            }

            string cacheKey = null;
            if (state.Seed == 0 && !state.UseDirectNull)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_deleteByKeyCache.TryGetCommand(cacheKey, out var cache))
                {
                    var command1 = new CommandData(cache);
                    var i = 0;
                    foreach (var field in mapping.PrimaryKeyFields)
                    {
                        state.AddDataParameter(this, keys[i], field.DBType, field.ObjectType);
                        i++;
                    }

                    return command1;
                }
            }

            if (!mapping.HasPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }

            QueryExpression queryExpression = null;
            var j = 0;
            foreach (var fieldMapping in mapping.PrimaryKeyFields)
            {
                var info = fieldMapping.DefaultFieldInfo;
                QueryExpression keyExpression =
                    new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, keys[j]);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                j++;
            }

            var command = CreateMassDeleteCommand(mapping, queryExpression, state);
            if (cacheKey != null)
            {
                _deleteByKeyCache.SetCommand(cacheKey, command.CommandText);
            }

            return command;
        }

        public virtual CommandData CreateBaseInsertCommand(DataTableEntityMapping mapping, object entity, bool refresh,
            bool updateIdentity, CreateSqlState state)
        {
            string cacheKey = null;
            if (state.Seed == 0 && !state.UseDirectNull)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_baseInsertCache.TryGetCommand(cacheKey, out var cache))
                {
                    var command1 = new CommandData(cache);
                    foreach (var field in mapping.CreateFieldList)
                    {
                        var value = field.ToInsert(entity, refresh);
                        state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                    }

                    if (updateIdentity && mapping.IdentityField != null)
                    {
                        var identitySql = CreateIdentitySql(mapping, state);
                        if (!string.IsNullOrEmpty(identitySql))
                        {
                            command1.CommandText = command1.CommandText + ";" + identitySql;
                            command1.IdentitySql = true;
                        }
                    }

                    return command1;
                }
            }

            IList<DataFieldMapping> fields = mapping.CreateFieldList;
            var insertLen = fields.Count;
            if (insertLen == 0)
            {
                throw new LightDataException(string.Format(SR.NotContainNonIdentityKeyFields, mapping.ObjectType));
            }

            var insertList = new string[insertLen];
            var valuesList = new string[insertLen];
            for (var i = 0; i < insertLen; i++)
            {
                var field = fields[i];
                var value = field.ToInsert(entity, refresh);
                insertList[i] = CreateDataFieldSql(field.Name);
                valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
            }

            var insert = string.Join(",", insertList);
            var values = string.Join(",", valuesList);
            var sql = $"insert into {CreateDataTableMappingSql(mapping, state)}({insert})values({values})";

            var command = new CommandData(sql);
            if (cacheKey != null)
            {
                _baseInsertCache.SetCommand(cacheKey, command.CommandText);
            }

            if (updateIdentity && mapping.IdentityField != null)
            {
                var identitySql = CreateIdentitySql(mapping, state);
                if (!string.IsNullOrEmpty(identitySql))
                {
                    command.CommandText = command.CommandText + ";" + identitySql;
                    command.IdentitySql = true;
                }
            }

            return command;
        }

        public virtual CommandData CreateBaseUpdateCommand(DataTableEntityMapping mapping, object entity, bool refresh,
            CreateSqlState state)
        {
            if (!mapping.HasPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }

            IList<DataFieldMapping> columnFields;
            object[] keys = null;
            var defaultUpdate = false;
            if (mapping.IsDataTableEntity && entity is DataTableEntity tableEntity)
            {
                keys = tableEntity.GetRawPrimaryKeys();
                var updateFieldNames = tableEntity.GetUpdateFields();
                if (updateFieldNames != null && updateFieldNames.Length > 0)
                {
                    var updateFields = new List<DataFieldMapping>();
                    foreach (var name in updateFieldNames)
                    {
                        var fm = mapping.FindDataEntityField(name);
                        if (fm == null)
                        {
                            throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedField,
                                mapping.ObjectType, name));
                        }

                        if (fm is PrimitiveFieldMapping pfm && pfm.IsPrimaryKey && keys == null)
                        {
                            throw new LightDataException(string.Format(SR.UpdateFieldIsPrimaryKeyField,
                                mapping.ObjectType, name));
                        }

                        if ((fm.FunctionControl & FunctionControl.Update) == FunctionControl.Update)
                        {
                            updateFields.Add(fm);
                        }
                    }

                    foreach (var tm in mapping.AutoUpdateFieldList)
                    {
                        if (!updateFields.Contains(tm) &&
                            (tm.FunctionControl & FunctionControl.Update) == FunctionControl.Update)
                        {
                            updateFields.Add(tm);
                        }
                    }

                    columnFields = updateFields;
                }
                else
                {
                    if (keys == null)
                    {
                        columnFields = mapping.UpdateFieldList;
                        defaultUpdate = true;
                    }
                    else
                    {
                        var updateFields = new List<DataFieldMapping>();
                        updateFields.AddRange(mapping.PrimaryKeyFields);
                        updateFields.AddRange(mapping.UpdateFieldList);
                        columnFields = updateFields;
                    }
                }
            }
            else
            {
                columnFields = mapping.UpdateFieldList;
                defaultUpdate = true;
            }

            string cacheKey = null;
            if (defaultUpdate && state.Seed == 0 && !state.UseDirectNull)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_baseUpdateCache.TryGetCommand(cacheKey, out var cache))
                {
                    var command1 = new CommandData(cache);
                    foreach (var field in columnFields)
                    {
                        // object value;
                        // if (field.IsTimeStamp)
                        // {
                        //     value = field.GetTimeStamp(entity, refresh);
                        // }
                        // else
                        // {
                        //     var obj = field.Handler.Get(entity);
                        //     value = field.ToParameter(obj);
                        // }
                        var value = field.ToUpdate(entity, refresh);
                        state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                    }

                    foreach (var field in mapping.PrimaryKeyFields)
                    {
                        var obj = field.Handler.Get(entity);
                        var value = field.ToParameter(obj);
                        state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                    }

                    return command1;
                }
            }

            if (columnFields.Count == 0)
            {
                return null;
            }

            IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;
            var keyLen = keyFields.Count;
            var updateLen = columnFields.Count;

            var updateList = new string[updateLen];
            var whereList = new string[keyLen];
            for (var i = 0; i < updateLen; i++)
            {
                var field = columnFields[i];
                // object value;
                // if (field.IsTimeStamp)
                // {
                //     value = field.GetTimeStamp(entity, refresh);
                // }
                // else
                // {
                //     var obj = field.Handler.Get(entity);
                //     value = field.ToParameter(obj);
                // }
                var value = field.ToUpdate(entity, refresh);
                updateList[i] =
                    $"{CreateDataFieldSql(field.Name)}={state.AddDataParameter(this, value, field.DBType, field.ObjectType)}";
            }

            for (var i = 0; i < keyLen; i++)
            {
                var field = keyFields[i];
                var obj = keys == null ? field.Handler.Get(entity) : keys[i];
                //object obj = field.Handler.Get(entity);
                var value = field.ToParameter(obj);
                whereList[i] =
                    $"{CreateDataFieldSql(field.Name)}={state.AddDataParameter(this, value, field.DBType, field.ObjectType)}";
            }

            var updateString = string.Join(",", updateList);
            var whereString = string.Join(" and ", whereList);
            var sql = $"update {CreateDataTableMappingSql(mapping, state)} set {updateString} where {whereString}";
            var command = new CommandData(sql);
            if (cacheKey != null)
            {
                _baseUpdateCache.SetCommand(cacheKey, command.CommandText);
            }

            return command;
        }


        public virtual CommandData CreateBaseDeleteCommand(DataTableEntityMapping mapping, object entity,
            CreateSqlState state)
        {
            string cacheKey = null;
            if (state.Seed == 0 && !state.UseDirectNull)
            {
                cacheKey = CommandCache.CreateKey(mapping, state);
                if (_baseDeleteCache.TryGetCommand(cacheKey, out var cache))
                {
                    var command1 = new CommandData(cache);
                    foreach (var field in mapping.PrimaryKeyFields)
                    {
                        var obj = field.Handler.Get(entity);
                        var value = field.ToParameter(obj);
                        state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                    }

                    return command1;
                }
            }

            if (!mapping.HasPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }

            IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;
            var keyLen = keyFields.Count;
            var whereList = new string[keyLen];
            for (var i = 0; i < keyLen; i++)
            {
                var field = keyFields[i];
                var obj = field.Handler.Get(entity);
                var value = field.ToParameter(obj);
                whereList[i] =
                    $"{CreateDataFieldSql(field.Name)}={state.AddDataParameter(this, value, field.DBType, field.ObjectType)}";
            }

            var whereString = string.Join(" and ", whereList);
            var sql = $"delete from {CreateDataTableMappingSql(mapping, state)} where {whereString}";
            var command = new CommandData(sql);
            if (cacheKey != null)
            {
                _baseDeleteCache.SetCommand(cacheKey, command.CommandText);
            }

            return command;
        }

        public virtual CommandData CreateTruncateTableCommand(DataTableEntityMapping mapping, CreateSqlState state)
        {
            var sql = $"truncate table {CreateDataTableMappingSql(mapping, state)};";
            var command = new CommandData(sql);
            return command;
        }

        #endregion

        #region 主命令语句块

        public virtual string GetGroupByString(AggregateGroupBy groupBy, bool isFullField, CreateSqlState state)
        {
            string queryString = null;
            if (groupBy.FieldCount > 0)
            {
                queryString = $" group by {groupBy.CreateGroupByString(this, isFullField, state)}";
            }

            return queryString;
        }

        public virtual string GetHavingString(QueryExpression query, bool isFullField, CreateSqlState state)
        {
            string queryString;
            if (HavingAlias)
            {
                state.UseFieldAlias = true;
                queryString = $" having {query.CreateSqlString(this, isFullField, state)}";
                state.UseFieldAlias = false;
            }
            else
            {
                queryString = $" having {query.CreateSqlString(this, isFullField, state)}";
            }

            return queryString;
        }

        public virtual string GetQueryString(QueryExpression query, bool isFullField, CreateSqlState state)
        {
            var queryString = $" where {query.CreateSqlString(this, isFullField, state)}";
            return queryString;
        }

        public virtual string GetOrderString(OrderExpression order, bool isFullField, CreateSqlState state)
        {
            var orderString = $" order by {order.CreateSqlString(this, isFullField, state)}";
            return orderString;
        }

        public virtual string GetAggregateOrderString(OrderExpression orderExpression, bool isFullField,
            CreateSqlState state)
        {
            state.UseFieldAlias = true;
            var orderString = $" order by {orderExpression.CreateSqlString(this, isFullField, state)}";
            state.UseFieldAlias = false;
            return orderString;
        }

        public virtual string GetOnString(DataFieldExpression onExpression, CreateSqlState state)
        {
            var onString = $" on {onExpression.CreateSqlString(this, true, state)}";
            return onString;
        }

        public virtual CommandData CreateSelectCommand(DataEntityMapping mapping, ISelector selector,
            QueryExpression query, OrderExpression order, bool distinct, Region region, CreateSqlState state)
        {
            var selectString = selector == null ? "*" : selector.CreateSelectString(this, false, state);

            if (distinct)
            {
                selectString = CreateDistinctSql() + selectString;
            }

            var commandData = CreateSelectBaseCommand(mapping, selectString, query, order, region, state);
            return commandData;
        }

        public virtual CommandData CreateSelectDataCommand(DataEntityMapping mapping, RelationMap relationMap,
            ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region,
            CreateSqlState state)
        {
            CommandData commandData;
            if (mapping.HasJoinRelateModel)
            {
                QueryExpression subQuery = null;
                QueryExpression mainQuery = null;
                OrderExpression subOrder = null;
                OrderExpression mainOrder = null;
                if (query != null)
                {
                    if (query.MultiQuery)
                    {
                        mainQuery = query;
                    }
                    else
                    {
                        subQuery = query;
                    }
                }

                if (order != null)
                {
                    if (order.MultiOrder)
                    {
                        mainOrder = order;
                    }
                    else
                    {
                        subOrder = order;
                    }
                }

                var models = relationMap.CreateJoinModels(subQuery, subOrder);
                commandData =
                    CreateSelectJoinTableCommand(selector, models, mainQuery, mainOrder, distinct, region, state);
            }
            else
            {
                commandData = CreateSelectCommand(mapping, selector, query, order, distinct, region, state);
            }

            return commandData;
        }

        public virtual CommandData CreateSelectSingleFieldCommand(DataFieldInfo fieldInfo, QueryExpression query,
            OrderExpression order, bool distinct, Region region, CreateSqlState state)
        {
            var selectString = fieldInfo.CreateSqlString(this, false, state);
            if (distinct)
            {
                selectString = CreateDistinctSql() + selectString;
            }

            return CreateSelectBaseCommand(fieldInfo.TableMapping, selectString, query, order, region, state);
        }

        public virtual CommandData CreateSelectBaseCommand(DataEntityMapping mapping, string customSelect,
            QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            var sql = new StringBuilder();
            sql.Append($"select {customSelect} from {CreateDataTableMappingSql(mapping, state)}");
            if (query != null)
            {
                sql.Append(GetQueryString(query, false, state));
            }

            if (order != null)
            {
                sql.Append(GetOrderString(order, false, state));
            }

            var commandData = new CommandData(sql.ToString());
            return commandData;
        }

        public virtual CommandData CreateSelectJoinTableCommand(ISelector selector, List<IJoinModel> modelList,
            QueryExpression query, OrderExpression order, bool distinct, Region region, CreateSqlState state)
        {
            var selectString = selector.CreateSelectString(this, true, state);
            if (distinct)
            {
                selectString = CreateDistinctSql() + selectString;
            }

            var subOrder = CreateJoinModelListOrderExpression(modelList);
            if (subOrder != null)
            {
                order = OrderExpression.Concat(subOrder, order);
            }

            var commandData = CreateSelectJoinTableBaseCommand(selectString, modelList, query, order, region, state);
            return commandData;
        }

        public virtual CommandData CreateSelectJoinTableBaseCommand(string customSelect, List<IJoinModel> modelList,
            QueryExpression query, OrderExpression order, Region region, CreateSqlState state)
        {
            var tables = new StringBuilder();
            foreach (var model in modelList)
            {
                if (model.Connect != null)
                {
                    tables.AppendFormat(" {0} ", _joinCollectionPredicateDict[model.Connect.Type]);
                }

                var modelSql = model.CreateSqlString(this, state);
                tables.Append(modelSql);
                if (model.Connect?.On != null)
                {
                    tables.Append(GetOnString(model.Connect.On, state));
                }
            }

            var sql = new StringBuilder();
            sql.Append($"select {customSelect} from {tables}");
            if (query != null)
            {
                sql.Append(GetQueryString(query, true, state));
            }

            if (order != null)
            {
                sql.Append(GetOrderString(order, true, state));
            }

            var command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateAggregateTableCommand(DataEntityMapping mapping, AggregateSelector selector,
            AggregateGroupBy groupBy, QueryExpression query, QueryExpression having, OrderExpression order,
            Region region, CreateSqlState state)
        {
            var sql = new StringBuilder();
            var selectString = selector.CreateSelectString(this, false, state);
            sql.Append($"select {selectString} from {CreateDataTableMappingSql(mapping, state)}");
            if (query != null)
            {
                sql.Append(GetQueryString(query, false, state));
            }

            if (groupBy != null)
            {
                sql.Append(GetGroupByString(groupBy, false, state));
            }

            if (having != null)
            {
                sql.Append(GetHavingString(having, false, state));
            }

            if (order != null)
            {
                sql.Append(GetAggregateOrderString(order, false, state));
            }

            var command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateExistsCommand(DataEntityMapping mapping, QueryExpression query,
            CreateSqlState state)
        {
            var region = new Region(0, 1);
            return CreateSelectBaseCommand(mapping, "1", query, null, region, state);
        }

        public virtual CommandData CreateAggregateFunctionCommand(DataFieldInfo field, AggregateType aggregateType,
            QueryExpression query, bool distinct, CreateSqlState state)
        {
            var mapping = field.TableMapping;
            var fieldSql = field.CreateSqlString(this, false, state);
            string function = null;
            switch (aggregateType)
            {
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

        public virtual CommandData CreateAggregateCountCommand(DataEntityMapping mapping, QueryExpression query,
            CreateSqlState state)
        {
            var select = CreateCountAllSql();
            return CreateSelectBaseCommand(mapping, select, query, null, null, state);
        }

        public virtual CommandData CreateAggregateJoinCountCommand(List<IJoinModel> modelList, QueryExpression query,
            CreateSqlState state)
        {
            var select = CreateCountAllSql();
            return CreateSelectJoinTableBaseCommand(select, modelList, query, null, null, state);
        }

        public virtual CommandData CreateMassDeleteCommand(DataTableEntityMapping mapping, QueryExpression query,
            CreateSqlState state)
        {
            var sql = new StringBuilder();
            sql.Append($"delete from {CreateDataTableMappingSql(mapping, state)}");
            if (query != null)
            {
                sql.Append(GetQueryString(query, false, state));
            }

            var command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateMassUpdateCommand(DataTableEntityMapping mapping, MassUpdator updator,
            QueryExpression query, CreateSqlState state)
        {
            var sql = new StringBuilder();
            var setString = updator.CreateSqlString(this, false, state);
            sql.Append($"update {CreateDataTableMappingSql(mapping, state)} set {setString}");
            if (query != null)
            {
                sql.Append(GetQueryString(query, false, state));
            }

            var command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateSelectInsertCommand(DataTableEntityMapping insertTableMapping,
            DataEntityMapping selectMapping, QueryExpression query, OrderExpression order, CreateSqlState state)
        {
            var sql = new StringBuilder();
            ReadOnlyCollection<DataFieldMapping> insertFields;
            ReadOnlyCollection<DataFieldMapping> selectFields;
            if (insertTableMapping.HasIdentity)
            {
                if (selectMapping is DataTableEntityMapping selectTableEntityMapping &&
                    selectTableEntityMapping.HasIdentity)
                {
                    if (insertTableMapping.FieldCount == selectTableEntityMapping.FieldCount &&
                        insertTableMapping.IdentityField.PositionOrder ==
                        selectTableEntityMapping.IdentityField.PositionOrder)
                    {
                        insertFields = insertTableMapping.NoIdentityFields;
                        selectFields = selectTableEntityMapping.NoIdentityFields;
                    }
                    else
                    {
                        throw new LightDataException(SR.SelectFieldsCountNotEqualInsertFieldCount);
                    }
                }
                else
                {
                    if (insertTableMapping.FieldCount == selectMapping.FieldCount + 1)
                    {
                        insertFields = insertTableMapping.NoIdentityFields;
                        selectFields = selectMapping.DataEntityFields;
                    }
                    else
                    {
                        throw new LightDataException(SR.SelectFieldsCountNotEqualInsertFieldCount);
                    }
                }
            }
            else
            {
                if (insertTableMapping.FieldCount == selectMapping.FieldCount)
                {
                    insertFields = insertTableMapping.DataEntityFields;
                    selectFields = selectMapping.DataEntityFields;
                }
                else
                {
                    throw new LightDataException(SR.SelectFieldsCountNotEqualInsertFieldCount);
                }
            }

            var insertFieldNames = new string[insertFields.Count];
            for (var i = 0; i < insertFields.Count; i++)
            {
                insertFieldNames[i] = CreateDataFieldSql(insertFields[i].Name);
            }

            var insertString = string.Join(",", insertFieldNames);

            var selectFieldNames = new string[selectFields.Count];
            for (var i = 0; i < insertFields.Count; i++)
            {
                selectFieldNames[i] = CreateDataFieldSql(selectFields[i].Name);
            }

            var selectString = string.Join(",", selectFieldNames);
            sql.Append($"insert into {CreateDataTableMappingSql(insertTableMapping, state)}({insertString})");
            sql.Append($"select {selectString} from {CreateDataTableMappingSql(selectMapping, state)}");
            if (query != null)
            {
                sql.Append(GetQueryString(query, false, state));
            }

            if (order != null)
            {
                sql.Append(GetOrderString(order, false, state));
            }

            var command = new CommandData(sql.ToString());
            return command;
        }

        public virtual CommandData CreateSelectInsertCommand(InsertSelector insertSelector, DataEntityMapping mapping,
            QueryExpression query, OrderExpression order, bool distinct, CreateSqlState state)
        {
            var selectCommandData = CreateSelectCommand(mapping, insertSelector, query, order, distinct, null, state);
            var insertFields = insertSelector.GetInsertFields();
            var insertFieldNames = new string[insertFields.Length];
            for (var i = 0; i < insertFields.Length; i++)
            {
                insertFieldNames[i] = CreateDataFieldSql(insertFields[i].FieldName);
            }

            var insertString = string.Join(",", insertFieldNames);
            var sql = $"insert into {CreateDataTableMappingSql(insertSelector.InsertMapping, state)}({insertString})";
            selectCommandData.CommandText = sql + selectCommandData.CommandText;
            return selectCommandData;
        }

        public virtual CommandData CreateSelectInsertCommand(InsertSelector insertSelector, List<IJoinModel> modelList,
            QueryExpression query, OrderExpression order, bool distinct, CreateSqlState state)
        {
            var selectCommandData =
                CreateSelectJoinTableCommand(insertSelector, modelList, query, order, distinct, null, state);
            var insertFields = insertSelector.GetInsertFields();
            var insertFieldNames = new string[insertFields.Length];
            for (var i = 0; i < insertFields.Length; i++)
            {
                insertFieldNames[i] = CreateDataFieldSql(insertFields[i].FieldName);
            }

            var insertString = string.Join(",", insertFieldNames);
            var sql = $"insert into {CreateDataTableMappingSql(insertSelector.InsertMapping, state)}({insertString})";
            selectCommandData.CommandText = sql + selectCommandData.CommandText;
            return selectCommandData;
        }

        public virtual CommandData CreateSelectInsertCommand(InsertSelector insertSelector, DataEntityMapping mapping,
            AggregateSelector selector, AggregateGroupBy groupBy, QueryExpression query, QueryExpression having,
            OrderExpression order, CreateSqlState state)
        {
            var selectCommandData =
                CreateAggregateTableCommand(mapping, selector, groupBy, query, having, order, null, state);
            var insertFields = insertSelector.GetInsertFields();
            var insertFieldNames = new string[insertFields.Length];
            for (var i = 0; i < insertFields.Length; i++)
            {
                insertFieldNames[i] = CreateDataFieldSql(insertFields[i].FieldName);
            }

            var insertString = string.Join(",", insertFieldNames);
            var selectString = insertSelector.CreateSelectString(this, false, state);
            var sql =
                $"insert into {CreateDataTableMappingSql(insertSelector.InsertMapping, state)}({insertString})select {selectString} from ({selectCommandData.CommandText}) as A";
            selectCommandData.CommandText = sql;
            return selectCommandData;
        }

        public virtual CommandData CreateBatchInsertWithIdentityCommand(DataTableEntityMapping mapping, IList entitys,
            bool refresh, CreateSqlState state)
        {
            throw new NotSupportedException();
        }


        public virtual CommandData CreateBatchInsertCommand(DataTableEntityMapping mapping, IList entitys, bool refresh,
            CreateSqlState state)
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
            foreach (var entity in entitys)
            {
                var valuesList = new string[insertLen];
                for (var i = 0; i < insertLen; i++)
                {
                    var field = fields[i];
                    //object obj = field.Handler.Get(entity);
                    //object value = field.ToColumn(obj);
                    var value = field.ToInsert(entity, refresh);
                    valuesList[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }

                var values = string.Join(",", valuesList);
                totalSql.AppendFormat("{0}values({1});", insertSql, values);
            }

            var command = new CommandData(totalSql.ToString());
            return command;
        }

        public virtual CommandData CreateBatchUpdateCommand(DataTableEntityMapping mapping, IList entitys, bool refresh,
            CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0)
            {
                throw new ArgumentNullException(nameof(entitys));
            }

            if (!mapping.HasPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            //if (mapping.UpdateFieldList.Count == 0 && !mapping.IsDataTableEntity) {
            //    throw new LightDataException(string.Format(SR.NotContainNonPrimaryKeyFields, mapping.ObjectType));
            //}

            IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;
            var keyLen = keyFields.Count;

            var createCount = 0;

            var totalSql = new StringBuilder();

            foreach (var entity in entitys)
            {
                IList<DataFieldMapping> columnFields;
                object[] keys = null;
                if (mapping.IsDataTableEntity && entity is DataTableEntity tableEntity)
                {
                    keys = tableEntity.GetRawPrimaryKeys();
                    var updateFieldNames = tableEntity.GetUpdateFields();
                    if (updateFieldNames != null && updateFieldNames.Length > 0)
                    {
                        var updateFields = new List<DataFieldMapping>();
                        foreach (var name in updateFieldNames)
                        {
                            var fm = mapping.FindDataEntityField(name);
                            if (fm == null)
                            {
                                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedField,
                                    mapping.ObjectType, name));
                            }

                            if (fm is PrimitiveFieldMapping pfm && pfm.IsPrimaryKey && keys == null)
                            {
                                throw new LightDataException(string.Format(SR.UpdateFieldIsPrimaryKeyField,
                                    mapping.ObjectType, name));
                            }

                            if ((fm.FunctionControl & FunctionControl.Update) == FunctionControl.Update)
                            {
                                updateFields.Add(fm);
                            }
                        }

                        foreach (var tm in mapping.AutoUpdateFieldList)
                        {
                            if (!updateFields.Contains(tm) &&
                                (tm.FunctionControl & FunctionControl.Update) == FunctionControl.Update)
                            {
                                updateFields.Add(tm);
                            }
                        }

                        columnFields = updateFields;
                    }
                    else
                    {
                        if (keys == null)
                        {
                            columnFields = mapping.UpdateFieldList;
                        }
                        else
                        {
                            var updateFields = new List<DataFieldMapping>();
                            updateFields.AddRange(mapping.PrimaryKeyFields);
                            updateFields.AddRange(mapping.UpdateFieldList);
                            columnFields = updateFields;
                        }
                    }
                }
                else
                {
                    columnFields = mapping.UpdateFieldList;
                }

                if (columnFields.Count == 0)
                {
                    continue;
                }

                var updateLen = columnFields.Count;
                var updateList = new string[updateLen];
                var whereList = new string[keyLen];
                for (var i = 0; i < updateLen; i++)
                {
                    var field = columnFields[i];
                    // object value;
                    // if (field.IsTimeStamp)
                    // {
                    //     value = field.GetTimeStamp(entity, refresh);
                    // }
                    // else
                    // {
                    //     var obj = field.Handler.Get(entity);
                    //     value = field.ToParameter(obj);
                    // }
                    var value = field.ToUpdate(entity, refresh);
                    updateList[i] =
                        $"{CreateDataFieldSql(field.Name)}={state.AddDataParameter(this, value, field.DBType, field.ObjectType)}";
                }

                for (var i = 0; i < keyLen; i++)
                {
                    var field = keyFields[i];
                    var obj = keys == null ? field.Handler.Get(entity) : keys[i];
                    //object obj = field.Handler.Get(entity);
                    var value = field.ToParameter(obj);
                    whereList[i] =
                        $"{CreateDataFieldSql(field.Name)}={state.AddDataParameter(this, value, field.DBType, field.ObjectType)}";
                }

                var updateString = string.Join(",", updateList);
                var whereString = string.Join(" and ", whereList);
                totalSql.Append(
                    $"update {CreateDataTableMappingSql(mapping, state)} set {updateString} where {whereString};");
                createCount++;
            }

            if (createCount == 0)
            {
                return null;
            }

            var command = new CommandData(totalSql.ToString());
            return command;
        }

        public virtual CommandData CreateBatchDeleteCommand(DataTableEntityMapping mapping, IList entitys,
            CreateSqlState state)
        {
            if (entitys == null || entitys.Count == 0)
            {
                throw new ArgumentNullException(nameof(entitys));
            }

            if (!mapping.HasPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }

            IList<DataFieldMapping> keyFields = mapping.PrimaryKeyFields;

            var keyLen = keyFields.Count;

            var totalSql = new StringBuilder();
            if (keyFields.Count == 1)
            {
                var field = keyFields[0];
                var keys = new string[entitys.Count];
                for (var i = 0; i < entitys.Count; i++)
                {
                    var entity = entitys[i];
                    var obj = field.Handler.Get(entity);
                    var value = field.ToParameter(obj);
                    keys[i] = state.AddDataParameter(this, value, field.DBType, field.ObjectType);
                }

                totalSql.Append(
                    $"delete from {CreateDataTableMappingSql(mapping, state)} where {CreateDataFieldSql(field.Name)} in ({string.Join(",", keys)});");
            }
            else
            {
                foreach (var entity in entitys)
                {
                    var whereList = new string[keyLen];
                    for (var i = 0; i < keyLen; i++)
                    {
                        var field = keyFields[i];
                        var obj = field.Handler.Get(entity);
                        var value = field.ToParameter(obj);
                        whereList[i] =
                            $"{CreateDataFieldSql(field.Name)}={state.AddDataParameter(this, value, field.DBType, field.ObjectType)}";
                    }

                    var whereString = string.Join(" and ", whereList);
                    totalSql.Append($"delete from {CreateDataTableMappingSql(mapping, state)} where {whereString};");
                }
            }

            var command = new CommandData(totalSql.ToString());
            return command;
        }

        public virtual CommandData CreateIdentityCommand(DataTableEntityMapping mapping, CreateSqlState state)
        {
            var sql = CreateIdentitySql(mapping, state);
            if (!string.IsNullOrEmpty(sql))
            {
                var command = new CommandData(sql);
                return command;
            }

            return null;
        }

        #endregion

        #region 基本语句块

        public virtual string CreateConcatExpressionSql(string expressionString1, string expressionString2,
            ConcatOperatorType operatorType)
        {
            return string.Format("({0} {2} {1})", expressionString1, expressionString2,
                operatorType.ToString().ToLower());
        }

        public virtual string CreateConcatExpressionSql(string[] expressionStrings)
        {
            return string.Join(",", expressionStrings);
        }

        public virtual string CreateSingleParamSql(object fieldName, QueryPredicate predicate, bool isReverse,
            string name)
        {
            var op = GetQueryPredicate(predicate);
            var ret = isReverse
                ? string.Concat(fieldName, op, name)
                : string.Concat(name, op, fieldName);

            return ret;
        }

        public virtual string CreateRelationTableSql(object fieldName, QueryPredicate predicate, bool isReverse,
            string relationFieldName)
        {
            var op = GetQueryPredicate(predicate);
            var ret = isReverse
                ? string.Concat(relationFieldName, op, fieldName)
                : string.Concat(fieldName, op, relationFieldName);

            return ret;
        }

        public virtual string CreateCollectionParamsQuerySql(object fieldName, QueryCollectionPredicate predicate,
            IEnumerable<object> list)
        {
            var op = GetQueryCollectionPredicate(predicate);

            var i = 0;
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {1} (", fieldName, op);
            foreach (var item in list)
            {
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
            return string.Format("{2}exists (select 1 from {0} where {1})", queryTableName, whereString,
                isNot ? "not " : string.Empty);
        }

        public virtual string CreateNotQuerySql(string whereString)
        {
            return $"not({whereString})";
        }

        public virtual string CreateSubQuerySql(object fieldName, QueryCollectionPredicate predicate,
            string queryFieldName, string queryTableName, string whereString)
        {
            var sb = new StringBuilder();
            var op = GetQueryCollectionPredicate(predicate);
            sb.AppendFormat("{0} {3} (select {1} from {2}", fieldName, queryFieldName, queryTableName, op);
            if (!string.IsNullOrEmpty(whereString))
            {
                sb.AppendFormat(" where {0}", whereString);
            }

            sb.Append(")");
            return sb.ToString();
        }

        public virtual string CreateBetweenParamsQuerySql(object fieldName, bool isNot, string fromParam,
            string toParam)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {3}between {1} and {2}", fieldName, fromParam, toParam, isNot ? string.Empty : "not ");
            return sb.ToString();
        }

        public virtual string CreateSingleParamSql(object left, QueryPredicate predicate, object right)
        {
            var op = GetQueryPredicate(predicate);
            var sql = string.Format("{0}{2}{1}", left, right, op);
            return sql;
        }

        public virtual string CreateBooleanQuerySql(object field, bool isTrue, bool isEqual, bool isReverse)
        {
            if (!isReverse)
            {
                return string.Format("{0}{2}{1}", field, isTrue ? "1" : "0", isEqual ? "=" : "!=");
            }

            return string.Format("{1}{2}{0}", field, isTrue ? "1" : "0", isEqual ? "=" : "!=");
        }

        public virtual string CreateNotSql(object value)
        {
            var sql = $"not({value})";
            return sql;
        }

        public virtual string CreateOutputNotSql(object value)
        {
            var sql = $"not({value})";
            return sql;
        }

        public virtual string CreateConcatSql(params object[] values)
        {
            var value1 = string.Join("+", values);
            var sql = $"({value1})";
            return sql;
        }

        public virtual string CreateLikeMatchQuerySql(object left, object right, bool starts, bool ends, bool isNot)
        {
            var value1 = CreateMatchSql(right.ToString(), starts, ends);
            var sql = string.Format("{0} {2}like {1}", left, value1, isNot ? "not " : string.Empty);
            return sql;
        }

        public virtual string CreateCollectionMatchQuerySql(object fieldName, bool isReverse, bool starts, bool ends,
            bool isNot, IEnumerable<object> list)
        {
            var i = 0;
            var sb = new StringBuilder();

            foreach (string item in list)
            {
                if (i > 0)
                {
                    sb.Append(isNot ? " and " : " or ");
                }

                if (!isReverse)
                {
                    var value1 = CreateMatchSql(item, starts, ends);
                    sb.AppendFormat("{0} {2}like {1}", fieldName, value1, isNot ? "not " : string.Empty);
                }
                else
                {
                    sb.AppendFormat("{1} {2}like {0}", fieldName, item, isNot ? "not " : string.Empty);
                }

                i++;
            }

            if (i > 1)
            {
                sb.Insert(0, "(");
                sb.Append(")");
            }

            return sb.ToString();
        }

        public virtual string CreateNullQuerySql(object fieldName, bool isNull)
        {
            return $"{fieldName} is{(isNull ? string.Empty : " not")} null";
        }

        public virtual string CreateBooleanQuerySql(object fieldName, bool isTrue)
        {
            return $"{fieldName}={(isTrue ? "1" : "0")}";
        }

        public virtual string CreateOrderBySql(object fieldName, OrderType orderType)
        {
            return $"{fieldName} {orderType.ToString().ToLower()}";
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
            return $"count(case when {expressionSql} then 1 else null end)";
        }

        public virtual string CreateConditionCountSql(string expressionSql, object fieldName, bool isDistinct)
        {
            return string.Format("count({2}case when {0} then {1} else null end)", expressionSql, fieldName,
                CreateDistinctSql(isDistinct));
        }

        public virtual string CreateCountSql(object fieldName, bool isDistinct)
        {
            return string.Format("count({1}{0})", fieldName, CreateDistinctSql(isDistinct));
        }

        public virtual string CreateSumSql(object fieldName, bool isDistinct)
        {
            return string.Format("sum({1}{0})", fieldName, CreateDistinctSql(isDistinct));
        }

        public virtual string CreateConditionSumSql(string expressionSql, object fieldName, bool isDistinct)
        {
            return string.Format("sum({2}case when {0} then {1} else null end)", expressionSql, fieldName,
                CreateDistinctSql(isDistinct));
        }

        public virtual string CreateAvgSql(object fieldName, bool isDistinct)
        {
            return string.Format("avg({1}{0})", fieldName, CreateDistinctSql(isDistinct));
        }

        public virtual string CreateConditionAvgSql(string expressionSql, object fieldName, bool isDistinct)
        {
            return string.Format("avg({2}case when {0} then {1} else null end)",
                expressionSql, fieldName, CreateDistinctSql(isDistinct));
        }

        public virtual string CreateMaxSql(object fieldName)
        {
            return $"max({fieldName})";
        }

        public virtual string CreateConditionMaxSql(string expressionSql, object fieldName)
        {
            return $"max(case when {expressionSql} then {fieldName} else null end)";
        }

        public virtual string CreateMinSql(object fieldName)
        {
            return $"min({fieldName})";
        }

        public virtual string CreateConditionMinSql(string expressionSql, object fieldName)
        {
            return $"min(case when {expressionSql} then {fieldName} else null end)";
        }

        public virtual string CreateAliasFieldSql(string field, string alias)
        {
            return $"{field} as {CreateDataFieldSql(alias)}";
        }

        public virtual string CreateAliasTableSql(string field, string alias)
        {
            return $"{field} as {CreateDataFieldSql(alias)}";
        }

        public virtual string CreateAliasQuerySql(string query, string alias)
        {
            return $"({query}) as {CreateDataFieldSql(alias)}";
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
            if (state.TryGetAliasTableName(mapping, out var name))
            {
                return CreateDataTableSql(name);
            }

            return CreateDataTableSql(mapping.TableName);
        }

        public virtual string CreateFullDataFieldSql(DataEntityMapping mapping, string fieldName, CreateSqlState state)
        {
            return $"{CreateDataTableMappingSql(mapping, state)}.{CreateDataFieldSql(fieldName)}";
        }

        public virtual string CreateFullDataFieldSql(string tableName, string fieldName)
        {
            return $"{CreateDataTableSql(tableName)}.{CreateDataFieldSql(fieldName)}";
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
            if (forward)
            {
                return $"({field}+{value})";
            }

            return $"({value}+{field})";
        }

        public virtual string CreatePlusSql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"({field}+{value})";
            }

            return $"({value}+{field})";
        }

        public virtual string CreateMinusSql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"({field}-{value})";
            }

            return $"({value}-{field})";
        }

        public virtual string CreateMultiplySql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"({field}*{value})";
            }

            return $"({value}*{field})";
        }

        public virtual string CreateDividedSql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"({field}/{value})";
            }

            return $"({value}/{field})";
        }

        public virtual string CreateModSql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"({field}%{value})";
            }

            return $"({value}%{field})";
        }

        public virtual string CreatePowerSql(object field, object value, bool forward)
        {
            if (forward)
            {
                return $"({field}^{value})";
            }

            return $"({value}^{field})";
        }

        public virtual string CreatePlusSql(object left, object right)
        {
            return $"({left}+{right})";
        }

        public virtual string CreateMinusSql(object left, object right)
        {
            return $"({left}-{right})";
        }

        public virtual string CreateMultiplySql(object left, object right)
        {
            return $"({left}*{right})";
        }

        public virtual string CreateDividedSql(object left, object right)
        {
            return $"({left}/{right})";
        }

        public virtual string CreateModSql(object left, object right)
        {
            return $"({left}%{right})";
        }

        public virtual string CreatePowerSql(object left, object right)
        {
            return $"({left}^{right})";
        }

        public virtual string CreateCastStringSql(object field, string format)
        {
            throw new NotSupportedException();
        }

        public virtual string CreateAbsSql(object field)
        {
            return $"abs({field})";
        }

        public virtual string CreateSignSql(object field)
        {
            return $"sign({field})";
        }

        public virtual string CreateLogSql(object field)
        {
            return $"log({field})";
        }

        public virtual string CreateLogSql(object field, object value)
        {
            return $"log({field},{value})";
        }

        public virtual string CreateLog10Sql(object field)
        {
            return $"log10({field})";
        }

        public virtual string CreateExpSql(object field)
        {
            return $"exp({field})";
        }

        public virtual string CreatePowSql(object field, object value)
        {
            return $"power({field},{value})";
        }

        public virtual string CreateSinSql(object field)
        {
            return $"sin({field})";
        }

        public virtual string CreateCosSql(object field)
        {
            return $"cos({field})";
        }

        public virtual string CreateAsinSql(object field)
        {
            return $"asin({field})";
        }

        public virtual string CreateAcosSql(object field)
        {
            return $"acos({field})";
        }

        public virtual string CreateTanSql(object field)
        {
            return $"tan({field})";
        }

        public virtual string CreateAtanSql(object field)
        {
            return $"atan({field})";
        }

        public virtual string CreateAtan2Sql(object field, object value)
        {
            return $"atan2({field},{value})";
        }

        public virtual string CreateCeilingSql(object field)
        {
            return $"ceiling({field})";
        }

        public virtual string CreateFloorSql(object field)
        {
            return $"floor({field})";
        }

        public virtual string CreateRoundSql(object field, object value)
        {
            return $"round({field},{value})";
        }

        public virtual string CreateTruncateSql(object field)
        {
            return $"truncate({field},0)";
        }

        public virtual string CreateSqrtSql(object field)
        {
            return $"Sqrt({field})";
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
            return $"case when {querySql} then {ifTrue} else {IfFalse} end";
        }

        #endregion


        public virtual string CreateJoinOnMatchSql(string leftField, QueryPredicate predicate, string rightField)
        {
            var sb = new StringBuilder();
            var op = GetQueryPredicate(predicate);
            sb.AppendFormat("{0}{2}{1}", leftField, rightField, op);
            return sb.ToString();
        }

        public virtual string CreateParamName(string name)
        {
            if (!name.StartsWith(ParameterPrefix, StringComparison.Ordinal))
            {
                return ParameterPrefix + name;
            }

            return name;
        }

        public virtual string CreateDistinctSql(bool isDistinct)
        {
            return isDistinct ? CreateDistinctSql() : "";
        }

        public virtual string CreateDistinctSql()
        {
            return "distinct ";
        }

        public virtual string CreateBooleanConstantSql(bool value)
        {
            return value ? "1=1" : "1=0";
        }

        public virtual string CreateStringWrap(object value)
        {
            return $"'{value}'";
        }

        public virtual string CreateUpdateAssign(Tuple<string, string>[] values)
        {
            var sb = new StringBuilder();
            var len = values.Length;
            for (var i = 0; i < len; i++)
            {
                var (field, value) = values[i];
                sb.Append(i < len - 1 ? $"{field}={value}," : $"{field}={value}");
            }

            return sb.ToString();
        }

        public virtual string CreateSelectFieldConcat(IEnumerable<string> values)
        {
            return string.Join(",", values);
        }
    }
}