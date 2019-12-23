using System.Text;

namespace Light.Data
{
    internal class EntityJoinModel : IJoinModel
    {
        public JoinConnect Connect { get; }

        public DataEntityMapping Mapping { get; }

        public IJoinTableMapping JoinMapping { get; }

        public QueryExpression Query { get; set; }

        public OrderExpression Order { get; set; }

        public bool Distinct { get; set; }

        public string AliasTableName { get; }

        public bool NoDataSetEntityNull { get; set; }

        public EntityJoinModel(DataEntityMapping mapping, string aliasTableName, JoinConnect connect, QueryExpression query, OrderExpression order, JoinSetting setting)
        {
            Mapping = mapping;
            //this._selector = AllSelector.Value;
            Connect = connect;
            Query = query;
            Order = order;
            AliasTableName = aliasTableName;
            JoinMapping = mapping;
            if ((setting & JoinSetting.QueryDistinct) == JoinSetting.QueryDistinct) {
                Distinct = true;
            }
            if ((setting & JoinSetting.NoDataSetEntityNull) == JoinSetting.NoDataSetEntityNull) {
               NoDataSetEntityNull = true;
            }
        }

        public string CreateSqlString(CommandFactory factory, CreateSqlState state)
        {
            var sb = new StringBuilder();
            if (Query != null || Order != null || Distinct) {
                var command = factory.CreateSelectCommand(Mapping, AllSelector.Value, Query, Order, Distinct, null, state);
                var aliasName = AliasTableName;// ?? factory.CreateDataTableMappingSql(_mapping, state);
                sb.Append(factory.CreateAliasQuerySql(command.CommandText, aliasName));
            }
            else {
                if (AliasTableName != null) {
                    sb.Append(factory.CreateAliasTableSql(factory.CreateDataTableMappingSql(Mapping, state), AliasTableName));
                }
                else {
                    sb.Append(factory.CreateDataTableMappingSql(Mapping, state));
                }
            }
            return sb.ToString();
        }
    }
}

