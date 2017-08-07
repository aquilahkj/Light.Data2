
using System;
using System.Text;

namespace Light.Data
{
    class EntityJoinModel : IJoinModel
    {
        readonly JoinConnect _connect;

        public JoinConnect Connect {
            get {
                return _connect;
            }
        }

        readonly DataEntityMapping _mapping;

        public DataEntityMapping Mapping {
            get {
                return _mapping;
            }
        }

        readonly IJoinTableMapping _joinMapping;

        public IJoinTableMapping JoinMapping {
            get {
                return _joinMapping;
            }
        }

        QueryExpression _query;

        public QueryExpression Query {
            get {
                return _query;
            }
            set {
                _query = value;
            }
        }

        OrderExpression _order;

        public OrderExpression Order {
            get {
                return _order;
            }
            set {
                _order = value;
            }
        }

        bool _distinct;

        public bool Distinct {
            get {
                return _distinct;
            }
            set {
                _distinct = value;
            }
        }

        readonly string _aliasTableName;

        public string AliasTableName {
            get {
                return _aliasTableName;
            }
        }

        public EntityJoinModel(DataEntityMapping mapping, string aliasTableName, JoinConnect connect, QueryExpression query, OrderExpression order)
        {
            this._mapping = mapping;
            //this._selector = AllSelector.Value;
            this._connect = connect;
            this._query = query;
            this._order = order;
            this._aliasTableName = aliasTableName;
            this._joinMapping = mapping;
        }

        public string CreateSqlString(CommandFactory factory, CreateSqlState state)
        {
            StringBuilder sb = new StringBuilder();
            if (_query != null || _order != null || _distinct) {
                CommandData command = factory.CreateSelectCommand(_mapping, AllSelector.Value, _query, _order, _distinct, null, state);
                string aliasName = _aliasTableName;// ?? factory.CreateDataTableMappingSql(_mapping, state);
                sb.Append(factory.CreateAliasQuerySql(command.CommandText, aliasName));
            } else {
                if (_aliasTableName != null) {
                    sb.Append(factory.CreateAliasTableSql(factory.CreateDataTableMappingSql(_mapping, state), _aliasTableName));
                } else {
                    sb.Append(factory.CreateDataTableMappingSql(_mapping, state));
                }
            }
            return sb.ToString();
        }
    }
}

