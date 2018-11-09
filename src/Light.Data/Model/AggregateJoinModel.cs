using System;
using System.Text;

namespace Light.Data
{
    class AggregateJoinModel : IJoinModel
    {
        readonly JoinConnect _connect;

        public JoinConnect Connect {
            get {
                return _connect;
            }
        }

        readonly AggregateModel _model;

        public AggregateModel Model {
            get {
                return _model;
            }
        }

        readonly IJoinTableMapping _joinMapping;

        public IJoinTableMapping JoinMapping {
            get {
                return _joinMapping;
            }
        }

        readonly QueryExpression _query;

        public QueryExpression Query {
            get {
                return _query;
            }
        }

        readonly OrderExpression _order;

        public OrderExpression Order {
            get {
                return _order;
            }
        }

        readonly QueryExpression _having;

        public QueryExpression Having {
            get {
                return _having;
            }
        }

        string _aliasTableName;

        public string AliasTableName {
            get {
                return _aliasTableName;
            }
        }

        bool _noDataSetEntityNull;

        public bool NoDataSetEntityNull {
            get {
                return _noDataSetEntityNull;
            }
            set {
                _noDataSetEntityNull = value;
            }
        }

        public AggregateJoinModel(AggregateModel model, string aliasTableName, JoinConnect connect, QueryExpression query, QueryExpression having, OrderExpression order, JoinSetting setting)
        {
            this._model = model;
            this._connect = connect;
            this._query = query;
            this._having = having;
            this._order = order;
            this._aliasTableName = aliasTableName;
            this._joinMapping = model.OutputMapping;
            if ((setting & JoinSetting.NoDataSetEntityNull) == JoinSetting.NoDataSetEntityNull) {
                _noDataSetEntityNull = true;
            }
        }

        public string CreateSqlString(CommandFactory factory, CreateSqlState state)
        {
            StringBuilder sb = new StringBuilder();
            CommandData command = factory.CreateAggregateTableCommand(_model.EntityMapping, _model.GetSelector(), Model.GetGroupBy(), _query, _having, null, null, state);
            string aliasName = _aliasTableName;// ?? factory.CreateDataTableMappingSql(_model.EntityMapping, state);
            sb.Append(factory.CreateAliasQuerySql(command.CommandText, aliasName));
            return sb.ToString();
        }
    }
}

