using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Light.Data
{
    abstract class DatabaseProvider
    {
        protected CommandFactory _factory;

        private string configName;

        public string ConfigName {
            get {
                return configName;
            }
        }

        protected DatabaseProvider(string configName, ConfigParamSet configParams)
        {
            this.configName = configName;
            string batchInsertCount = configParams.GetParamValue("batchInsertCount");
            if (batchInsertCount != null) {
                if (int.TryParse(batchInsertCount, out int value) && value > 0)
                    _batchInsertCount = value;
            }

            string batchUpdateCount = configParams.GetParamValue("batchUpdateCount");
            if (batchUpdateCount != null) {
                if (int.TryParse(batchUpdateCount, out int value) && value > 0)
                    _batchUpdateCount = value;
            }

            string batchDeleteCount = configParams.GetParamValue("batchDeleteCount");
            if (batchDeleteCount != null) {
                if (int.TryParse(batchDeleteCount, out int value) && value > 0)
                    _batchDeleteCount = value;
            }

            string timeout = configParams.GetParamValue("timeout");
            if (timeout != null) {
                if (int.TryParse(batchInsertCount, out int value) && value > 0)
                    _commandTimeout = value;
            }
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>The connection.</returns>
        public abstract DbConnection CreateConnection();

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>The connection.</returns>
        /// <param name="connectionString">Connection string.</param>
        public abstract DbConnection CreateConnection(string connectionString);

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>The command.</returns>
        /// <param name="sql">Sql.</param>
        public abstract DbCommand CreateCommand(string sql);

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>The command.</returns>
        public abstract DbCommand CreateCommand();

        /// <summary>
        /// Creates the data adapter.
        /// </summary>
        /// <returns>return data adapter</returns>
        public abstract DataAdapter CreateDataAdapter(DbCommand command);

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <returns>The parameter.</returns>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="dbType">Db type.</param>
        /// <param name="direction">Direction.</param>
        public abstract IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction, Type dataType);

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>The factory.</value>
        public CommandFactory Factory {
            get {
                return _factory;
            }
        }

        int _commandTimeout = 60000;

        public int CommandTimeout {
            get {
                return _commandTimeout;
            }
            set {
                if (value > 0) {
                    _commandTimeout = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(CommandTimeout));
                }
            }
        }

        int _batchInsertCount;

        public int BatchInsertCount {
            get {
                return _batchInsertCount;
            }
            set {
                if (value > 0) {
                    _batchInsertCount = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(BatchInsertCount));
                }
            }
        }

        int _batchUpdateCount;

        public int BatchUpdateCount {
            get {
                return _batchUpdateCount;
            }
            set {
                if (value > 0) {
                    _batchUpdateCount = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(BatchUpdateCount));
                }
            }
        }

        int _batchDeleteCount;

        public int BatchDeleteCount {
            get {
                return _batchDeleteCount;
            }
            set {
                if (value > 0) {
                    _batchDeleteCount = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(BatchDeleteCount));
                }
            }
        }

        /// <summary>
        /// Formats the stored procedure parameter.
        /// </summary>
        /// <param name="dataParameter">Data parmeter.</param>
        public abstract void FormatStoredProcedureParameter(IDataParameter dataParameter);




        public virtual QueryCommand QueryEntityData(DataContext context, DataEntityMapping mapping, ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region)
        {
            RelationMap relationMap = mapping.GetRelationMap();
            if (selector == null) {
                selector = relationMap.GetDefaultSelector();
            }
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateSelectDataCommand(mapping, relationMap, selector, query, order, distinct, region, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryState queryState = new QueryState();
            queryState.SetRelationMap(relationMap);
            queryState.SetSelector(selector);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage,
                State = queryState
            };
            return queryCommand;
        }

        public virtual QueryCommand QueryJoinData(DataContext context, DataMapping mapping, ISelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct, Region region)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateSelectJoinTableCommand(selector, models, query, order, distinct, region, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryState queryState = new QueryState();
            foreach (IJoinModel model in models) {
                if (model.NoDataSetEntityNull) {
                    queryState.SetNoDataSetNull(model.AliasTableName);
                }
            }
            queryState.SetSelector(selector);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage,
                State = queryState
            };
            return queryCommand;
        }

        public virtual QueryCommand QuerySingleField(DataContext context, DataFieldInfo fieldInfo, QueryExpression query, OrderExpression order, bool distinct, Region region)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateSelectSingleFieldCommand(fieldInfo, query, order, distinct, region, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand QueryDynamicAggregate(DataContext context, AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order, Region region)
        {
            CreateSqlState state = new CreateSqlState(context);
            AggregateSelector selector = model.GetSelector();
            AggregateGroupBy groupBy = model.GetGroupBy();
            CommandData commandData = _factory.CreateAggregateTableCommand(model.EntityMapping, selector, groupBy, query, having, order, region, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryState queryState = new QueryState();
            queryState.SetSelector(selector);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage,
                State = queryState
            };
            return queryCommand;
        }

        public virtual QueryCommand AggregateCount(DataContext context, DataEntityMapping mapping, QueryExpression query)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateAggregateCountCommand(mapping, query, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand AggregateJoinTableCount(DataContext context, List<IJoinModel> models, QueryExpression query)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateAggregateJoinCountCommand(models, query, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand Aggregate(DataContext context, DataFieldInfo field, AggregateType aggregateType, QueryExpression query, bool distinct)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateAggregateFunctionCommand(field, aggregateType, query, distinct, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand Exists(DataContext context, DataEntityMapping mapping, QueryExpression query)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateExistsCommand(mapping, query, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand TruncateTable(DataContext context, DataTableEntityMapping mapping)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateTruncateTableCommand(mapping, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectInsert(DataContext context, DataTableEntityMapping insertMapping, DataEntityMapping selectMapping, QueryExpression query, OrderExpression order)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateSelectInsertCommand(insertMapping, selectMapping, query, order, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectInsert(DataContext context, InsertSelector selector, DataEntityMapping mapping, QueryExpression query, OrderExpression order, bool distinct)
        {
            RelationMap relationMap = mapping.GetRelationMap();
            CommandData commandData;
            CreateSqlState state = new CreateSqlState(context);
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
                commandData = _factory.CreateSelectInsertCommand(selector, models, mainQuery, mainOrder, distinct, state);
            }
            else {
                commandData = _factory.CreateSelectInsertCommand(selector, mapping, query, order, distinct, state);
            }
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectInsertWithJoinTable(DataContext context, InsertSelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateSelectInsertCommand(selector, models, query, order, distinct, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectInsertWithAggregate(DataContext context, InsertSelector selector, AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order)
        {
            CreateSqlState state = new CreateSqlState(context);
            AggregateSelector aselector = model.GetSelector();
            AggregateGroupBy groupBy = model.GetGroupBy();
            CommandData commandData = _factory.CreateSelectInsertCommand(selector, model.EntityMapping, aselector, groupBy, query, having, order, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand QueryDelete(DataContext context, DataTableEntityMapping mapping, QueryExpression query)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateMassDeleteCommand(mapping, query, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand QueryUpdate(DataContext context, DataTableEntityMapping mapping, MassUpdator updator, QueryExpression query)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = _factory.CreateMassUpdateCommand(mapping, updator, query, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand Insert(DataContext context, DataTableEntityMapping mapping, object data, bool refresh)
        {
            CreateSqlState state = new CreateSqlState(context, false);
            CommandData commandData = _factory.CreateBaseInsertCommand(mapping, data, refresh, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand InsertIdentiy(DataContext context, DataTableEntityMapping mapping)
        {
            CreateSqlState state = new CreateSqlState(context);
            CommandData commandData = null;
            if (mapping.IdentityField != null) {
                commandData = _factory.CreateIdentityCommand(mapping, state);
                DbCommand command = commandData.CreateCommand(this);
                QueryCommand queryCommand = new QueryCommand() {
                    Command = command
                };
                return queryCommand;
            }
            else {
                return null;
            }
        }

        public virtual void UpdateDataIdentity(DataTableEntityMapping mapping, object data, object id)
        {
            if (!Equals(id, null)) {
                if (id.GetType() != mapping.IdentityField.ObjectType) {
                    id = Convert.ChangeType(id, mapping.IdentityField.ObjectType);
                }
                mapping.IdentityField.Handler.Set(data, id);
            }
        }

        public virtual QueryCommand Update(DataContext context, DataTableEntityMapping mapping, object data, bool refresh)
        {
            CreateSqlState state = new CreateSqlState(context, false);
            CommandData commandData = _factory.CreateBaseUpdateCommand(mapping, data, refresh, state);
            if (commandData == null) {
                return null;
            }
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand Delete(DataContext context, DataTableEntityMapping mapping, object data)
        {
            CreateSqlState state = new CreateSqlState(context, false);
            CommandData commandData = _factory.CreateBaseDeleteCommand(mapping, data, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand BatchInsert(DataContext context, DataTableEntityMapping mapping, IList datas, bool refresh)
        {
            CreateSqlState state = new CreateSqlState(context, false);
            CommandData commandData = _factory.CreateBatchInsertCommand(mapping, datas, refresh, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand BatchUpdate(DataContext context, DataTableEntityMapping mapping, IList datas, bool refresh)
        {
            CreateSqlState state = new CreateSqlState(context, false);
            CommandData commandData = _factory.CreateBatchUpdateCommand(mapping, datas, refresh, state);
            if (commandData == null) {
                return null;
            }
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand BatchDelete(DataContext context, DataTableEntityMapping mapping, IList datas)
        {
            CreateSqlState state = new CreateSqlState(context, false);
            CommandData commandData = _factory.CreateBatchDeleteCommand(mapping, datas, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectById(DataContext context, DataTableEntityMapping mapping, object id)
        {
            CreateSqlState state = new CreateSqlState(context, false);
            CommandData commandData = _factory.CreateSelectByIdCommand(mapping, id, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectByKey(DataContext context, DataTableEntityMapping mapping, object[] keys)
        {
            CreateSqlState state = new CreateSqlState(context, false);
            CommandData commandData = _factory.CreateSelectByKeyCommand(mapping, keys, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand ExistsByKey(DataContext context, DataTableEntityMapping mapping, object[] keys)
        {
            CreateSqlState state = new CreateSqlState(context, false);
            CommandData commandData = _factory.CreateExistsByKeyCommand(mapping, keys, state);
            DbCommand command = commandData.CreateCommand(this, state);
            QueryCommand queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

    }
}



