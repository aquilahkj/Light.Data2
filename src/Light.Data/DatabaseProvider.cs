using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Light.Data
{
    internal abstract class DatabaseProvider
    {
        protected CommandFactory _factory;

        public string ConfigName { get; }

        protected DatabaseProvider(string configName, ConfigParamSet configParams)
        {
            this.ConfigName = configName;
            var batchInsertCount = configParams.GetParamValue("batchInsertCount");
            if (batchInsertCount != null) {
                if (int.TryParse(batchInsertCount, out var value) && value > 0)
                    _batchInsertCount = value;
            }

            var batchUpdateCount = configParams.GetParamValue("batchUpdateCount");
            if (batchUpdateCount != null) {
                if (int.TryParse(batchUpdateCount, out var value) && value > 0)
                    _batchUpdateCount = value;
            }

            var batchDeleteCount = configParams.GetParamValue("batchDeleteCount");
            if (batchDeleteCount != null) {
                if (int.TryParse(batchDeleteCount, out var value) && value > 0)
                    _batchDeleteCount = value;
            }

            var timeout = configParams.GetParamValue("timeout");
            if (timeout != null) {
                if (int.TryParse(batchInsertCount, out var value) && value > 0)
                    _commandTimeout = value;
            }
        }

        public virtual string ParameterPrefix => _factory.ParameterPrefix;

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
        /// <param name="dataType">Data type.</param>
        /// <param name="commandType">Command type.</param>
        public abstract IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction, Type dataType, CommandType commandType);

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>The factory.</value>
        public CommandFactory Factory => _factory;

        private int _commandTimeout = 60000;

        public int CommandTimeout {
            get => _commandTimeout;
            set {
                if (value > 0) {
                    _commandTimeout = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(CommandTimeout));
                }
            }
        }

        private int _batchInsertCount = 100;

        public int BatchInsertCount {
            get => _batchInsertCount;
            set {
                if (value >= 0) {
                    _batchInsertCount = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(BatchInsertCount));
                }
            }
        }

        private int _batchUpdateCount = 100;

        public int BatchUpdateCount {
            get => _batchUpdateCount;
            set {
                if (value >= 0) {
                    _batchUpdateCount = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(BatchUpdateCount));
                }
            }
        }

        private int _batchDeleteCount = 100;

        public int BatchDeleteCount {
            get => _batchDeleteCount;
            set {
                if (value >= 0) {
                    _batchDeleteCount = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(BatchDeleteCount));
                }
            }
        }

        public virtual QueryCommand QueryEntityData(DataContext context, DataEntityMapping mapping, ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region)
        {
            var relationMap = mapping.GetRelationMap();
            if (selector == null) {
                selector = relationMap.GetDefaultSelector();
            }
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateSelectDataCommand(mapping, relationMap, selector, query, order, distinct, region, state);
            var command = commandData.CreateCommand(this, state);
            var queryState = new QueryState();
            queryState.SetRelationMap(relationMap);
            queryState.SetSelector(selector);
            var queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage,
                State = queryState
            };
            return queryCommand;
        }

        public virtual QueryCommand QueryJoinData(DataContext context, DataMapping mapping, ISelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct, Region region)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateSelectJoinTableCommand(selector, models, query, order, distinct, region, state);
            var command = commandData.CreateCommand(this, state);
            var queryState = new QueryState();
            foreach (var model in models) {
                if (model.NoDataSetEntityNull) {
                    queryState.SetNoDataSetNull(model.AliasTableName);
                }
            }
            queryState.SetSelector(selector);
            var queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage,
                State = queryState
            };
            return queryCommand;
        }

        public virtual QueryCommand QuerySingleField(DataContext context, DataFieldInfo fieldInfo, QueryExpression query, OrderExpression order, bool distinct, Region region)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateSelectSingleFieldCommand(fieldInfo, query, order, distinct, region, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand QueryDynamicAggregate(DataContext context, AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order, Region region)
        {
            var state = new CreateSqlState(context);
            var selector = model.GetSelector();
            var groupBy = model.GetGroupBy();
            var commandData = _factory.CreateAggregateTableCommand(model.EntityMapping, selector, groupBy, query, having, order, region, state);
            var command = commandData.CreateCommand(this, state);
            var queryState = new QueryState();
            queryState.SetSelector(selector);
            var queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage,
                State = queryState
            };
            return queryCommand;
        }

        public virtual QueryCommand AggregateCount(DataContext context, DataEntityMapping mapping, QueryExpression query)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateAggregateCountCommand(mapping, query, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand AggregateJoinTableCount(DataContext context, List<IJoinModel> models, QueryExpression query)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateAggregateJoinCountCommand(models, query, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand Aggregate(DataContext context, DataFieldInfo field, AggregateType aggregateType, QueryExpression query, bool distinct)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateAggregateFunctionCommand(field, aggregateType, query, distinct, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand Exists(DataContext context, DataEntityMapping mapping, QueryExpression query)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateExistsCommand(mapping, query, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand TruncateTable(DataContext context, DataTableEntityMapping mapping)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateTruncateTableCommand(mapping, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command,
                InnerPage = commandData.InnerPage
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectInsert(DataContext context, DataTableEntityMapping insertMapping, DataEntityMapping selectMapping, QueryExpression query, OrderExpression order)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateSelectInsertCommand(insertMapping, selectMapping, query, order, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectInsert(DataContext context, InsertSelector selector, DataEntityMapping mapping, QueryExpression query, OrderExpression order, bool distinct)
        {
            var relationMap = mapping.GetRelationMap();
            CommandData commandData;
            var state = new CreateSqlState(context);
            if (mapping.HasJoinRelateModel) {
                QueryExpression subQuery = null;
                QueryExpression mainQuery = null;
                OrderExpression subOrder = null;
                OrderExpression mainOrder = null;
                if (query != null) {
                    if (query.MultiQuery) {
                        mainQuery = query;
                    }
                    else {
                        subQuery = query;
                    }
                }
                if (order != null) {
                    if (order.MultiOrder) {
                        mainOrder = order;
                    }
                    else {
                        subOrder = order;
                    }
                }
                var models = relationMap.CreateJoinModels(subQuery, subOrder);
                commandData = _factory.CreateSelectInsertCommand(selector, models, mainQuery, mainOrder, distinct, state);
            }
            else {
                commandData = _factory.CreateSelectInsertCommand(selector, mapping, query, order, distinct, state);
            }
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectInsertWithJoinTable(DataContext context, InsertSelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateSelectInsertCommand(selector, models, query, order, distinct, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectInsertWithAggregate(DataContext context, InsertSelector selector, AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order)
        {
            var state = new CreateSqlState(context);
            var aselector = model.GetSelector();
            var groupBy = model.GetGroupBy();
            var commandData = _factory.CreateSelectInsertCommand(selector, model.EntityMapping, aselector, groupBy, query, having, order, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand QueryDelete(DataContext context, DataTableEntityMapping mapping, QueryExpression query)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateMassDeleteCommand(mapping, query, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand QueryUpdate(DataContext context, DataTableEntityMapping mapping, MassUpdator updator, QueryExpression query)
        {
            var state = new CreateSqlState(context);
            var commandData = _factory.CreateMassUpdateCommand(mapping, updator, query, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand Insert(DataContext context, DataTableEntityMapping mapping, object data, bool refresh, bool updateIdentity)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateBaseInsertCommand(mapping, data, refresh, updateIdentity, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command,
                IdentitySql = commandData.IdentitySql
            };
            return queryCommand;
        }

        public virtual QueryCommand InsertIdentiy(DataContext context, DataTableEntityMapping mapping)
        {
            var state = new CreateSqlState(context);
            CommandData commandData = null;
            if (mapping.IdentityField != null) {
                commandData = _factory.CreateIdentityCommand(mapping, state);
                var command = commandData.CreateCommand(this);
                var queryCommand = new QueryCommand() {
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
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateBaseUpdateCommand(mapping, data, refresh, state);
            if (commandData == null) {
                return null;
            }
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand Delete(DataContext context, DataTableEntityMapping mapping, object data)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateBaseDeleteCommand(mapping, data, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand BatchInsertWithIdentity(DataContext context, DataTableEntityMapping mapping, IList datas, bool refresh)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateBatchInsertWithIdentityCommand(mapping, datas, refresh, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand BatchInsert(DataContext context, DataTableEntityMapping mapping, IList datas, bool refresh)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateBatchInsertCommand(mapping, datas, refresh, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand BatchUpdate(DataContext context, DataTableEntityMapping mapping, IList datas, bool refresh)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateBatchUpdateCommand(mapping, datas, refresh, state);
            if (commandData == null) {
                return null;
            }
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand BatchDelete(DataContext context, DataTableEntityMapping mapping, IList datas)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateBatchDeleteCommand(mapping, datas, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectById(DataContext context, DataTableEntityMapping mapping, object id)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateSelectByIdCommand(mapping, id, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand SelectByKey(DataContext context, DataTableEntityMapping mapping, object[] keys)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateSelectByKeyCommand(mapping, keys, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand ExistsByKey(DataContext context, DataTableEntityMapping mapping, object[] keys)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateExistsByKeyCommand(mapping, keys, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

        public virtual QueryCommand DeleteByKey(DataContext context, DataTableEntityMapping mapping, object[] keys)
        {
            var state = new CreateSqlState(context, false);
            var commandData = _factory.CreateDeleteKeyCommand(mapping, keys, state);
            var command = commandData.CreateCommand(this, state);
            var queryCommand = new QueryCommand() {
                Command = command
            };
            return queryCommand;
        }

    }
}



