namespace Light.Data
{
    class LightInQueryDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
    {
        bool _isTrue;

        readonly QueryExpression _expression;

        readonly DataFieldInfo _selectField;

        readonly DataFieldInfo _field;

        public LightInQueryDataFieldInfo(DataEntityMapping mapping, DataFieldInfo field, DataFieldInfo selectField, QueryExpression expression, bool isTrue)
            : base(mapping)
        {
            _field = field;
            _selectField = selectField;
            _expression = expression;
            _isTrue = isTrue;
        }

        public void SetNot()
        {
            _isTrue = !_isTrue;
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
            QueryCollectionPredicate op = _isTrue ? QueryCollectionPredicate.In : QueryCollectionPredicate.NotIn;
            sql = factory.CreateSubQuerySql(field, op, selectField, tableName, query);

            state.SetDataSql(this, isFullName, sql);
            return sql;
        }

        public QueryExpression ConvertToExpression()
        {
            return new LightInQueryExpression(this);
        }
    }
}
