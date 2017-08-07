using System;
namespace Light.Data
{
    class LightSubQueryDataFieldInfo : LightDataFieldInfo, IDataFieldInfoConvert
    {
        readonly QueryExpression _expression;

        readonly DataFieldInfo _selectField;

        readonly DataFieldInfo _field;

        readonly QueryCollectionPredicate _predicate;

        public LightSubQueryDataFieldInfo(DataEntityMapping mapping, DataFieldInfo field, DataFieldInfo selectField, QueryCollectionPredicate predicate, QueryExpression expression)
            : base(mapping)
        {
            _field = field;
            _selectField = selectField;
            _predicate = predicate;
            _expression = expression;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            string sql = state.GetDataSql(this, isFullName);
            if (sql != null) {
                return sql;
            }

            string tableName = factory.CreateDataTableMappingSql(TableMapping, state);
            string selectField = _selectField.CreateSqlString(factory, true, state);

            string field = _field.CreateSqlString(factory, isFullName, state);

            string query = null;
            if (_expression != null) {
                query = _expression.CreateSqlString(factory, true, state);
            }
            sql = factory.CreateSubQuerySql(field, _predicate, selectField, tableName, query);

            state.SetDataSql(this, isFullName, sql);
            return sql;
        }

        public QueryExpression ConvertToExpression()
        {
            return new LightSubQueryExpression(this);
        }
    }
}
