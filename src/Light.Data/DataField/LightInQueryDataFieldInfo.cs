namespace Light.Data
{
    internal class LightInQueryDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
    {
        private bool _isTrue;

        private readonly QueryExpression _expression;

        private readonly DataFieldInfo _selectField;

        private readonly DataFieldInfo _field;

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
            var sql = state.GetDataSql(this, isFullName);
            if (sql != null) {
                return sql;
            }

            var tableName = factory.CreateDataTableMappingSql(TableMapping, state);
            var selectField = _selectField.CreateSqlString(factory, true, state);

            var field = _field.CreateSqlString(factory, isFullName, state);

            string query = null;
            if (_expression != null) {
                query = _expression.CreateSqlString(factory, true, state);
            }
            var op = _isTrue ? QueryCollectionPredicate.In : QueryCollectionPredicate.NotIn;
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
