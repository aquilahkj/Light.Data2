namespace Light.Data
{
    class LightExistsDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
    {
        bool _isTrue;

        readonly QueryExpression _expression;

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
            string sql = state.GetDataSql(this, isFullName);
            if (sql != null) {
                return sql;
            }
            string query = _expression.CreateSqlString(factory, true, state);
            string tableName = factory.CreateDataTableMappingSql(TableMapping, state);
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
