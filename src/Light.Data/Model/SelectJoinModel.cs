using System;
using System.Text;

namespace Light.Data
{
	class SelectJoinModel : IJoinModel
	{
		readonly SelectModel _model;

		public SelectModel Model {
			get {
				return _model;
			}
		}

		readonly JoinConnect _connect;

		public JoinConnect Connect {
			get {
				return _connect;
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

        bool _noDataSetEntityNull;

        public bool NoDataSetEntityNull {
            get {
                return _noDataSetEntityNull;
            }
            set {
                _noDataSetEntityNull = value;
            }
        }

        public SelectJoinModel (SelectModel model, string aliasTableName, JoinConnect connect, QueryExpression query, OrderExpression order, JoinSetting setting)
		{
			this._model = model;
			this._connect = connect;
			this._query = query;
			this._order = order;
			this._aliasTableName = aliasTableName;
			this._joinMapping = model.JoinTableMapping;
            if ((setting & JoinSetting.QueryDistinct) == JoinSetting.QueryDistinct) {
                _distinct = true;
            }
            if ((setting & JoinSetting.NoDataSetEntityNull) == JoinSetting.NoDataSetEntityNull) {
                _noDataSetEntityNull = true;
            }
        }

		public string CreateSqlString (CommandFactory factory, CreateSqlState state)
		{
			StringBuilder sb = new StringBuilder ();
			CommandData command = factory.CreateSelectCommand (_model.EntityMapping, _model.CreateSelector (), _query, _order, _distinct, null, state);
            string aliasName = _aliasTableName;// ?? factory.CreateDataTableMappingSql(_model.EntityMapping, state); 
			sb.Append (factory.CreateAliasQuerySql (command.CommandText, aliasName));
			return sb.ToString ();
		}
	}
}
