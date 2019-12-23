namespace Light.Data
{
    internal class LightExistsDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
    {
        private bool _isTrue;

        private readonly QueryExpression _expression;

        public LightExistsDataFieldInfo(DataEntityMapping mapping, QueryExpression expression, bool isTrue)
            : base(mapping)
        {
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
            var query = _expression.CreateSqlString(factory, true, state);
            var tableName = factory.CreateDataTableMappingSql(TableMapping, state);
            sql = factory.CreateExistsQuerySql(tableName, query, !_isTrue);

            state.SetDataSql(this, isFullName, sql);
            return sql;
        }

        public QueryExpression ConvertToExpression()
        {
            return new LightExistsQueryExpression(this);
        }
    }
}
