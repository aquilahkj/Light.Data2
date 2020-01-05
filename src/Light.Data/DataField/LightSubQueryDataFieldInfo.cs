namespace Light.Data
{
    internal class LightSubQueryDataFieldInfo : LightDataFieldInfo, IDataFieldInfoConvert
    {
        private readonly QueryExpression _expression;

        private readonly DataFieldInfo _selectField;

        private readonly DataFieldInfo _field;

        private readonly QueryCollectionPredicate _predicate;

        public LightSubQueryDataFieldInfo(DataEntityMapping mapping, DataFieldInfo field, DataFieldInfo selectField,
            QueryCollectionPredicate predicate, QueryExpression expression)
            : base(mapping)
        {
            _field = field;
            _selectField = selectField;
            _predicate = predicate;
            _expression = expression;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            var sql = state.GetDataSql(this, isFullName);
            if (sql != null)
            {
                return sql;
            }

            var tableName = factory.CreateDataTableMappingSql(TableMapping, state);
            var selectField = _selectField.CreateSqlString(factory, true, state);

            var field = _field.CreateSqlString(factory, isFullName, state);

            string query = null;
            if (_expression != null)
            {
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