using System.Text;

namespace Light.Data
{
    internal class AggregateJoinModel : IJoinModel
    {
        public JoinConnect Connect { get; }

        public AggregateModel Model { get; }

        public IJoinTableMapping JoinMapping { get; }

        public QueryExpression Query { get; }

        public OrderExpression Order { get; }

        public QueryExpression Having { get; }

        public string AliasTableName { get; }

        public bool NoDataSetEntityNull { get; }

        public AggregateJoinModel(AggregateModel model, string aliasTableName, JoinConnect connect, QueryExpression query, QueryExpression having, OrderExpression order, JoinSetting setting)
        {
            Model = model;
            Connect = connect;
            Query = query;
            Having = having;
            Order = order;
            AliasTableName = aliasTableName;
            JoinMapping = model.OutputDataMapping;
            if ((setting & JoinSetting.NoDataSetEntityNull) == JoinSetting.NoDataSetEntityNull) {
                NoDataSetEntityNull = true;
            }
        }

        public string CreateSqlString(CommandFactory factory, CreateSqlState state)
        {
            var sb = new StringBuilder();
            var command = factory.CreateAggregateTableCommand(Model.EntityMapping, Model.GetSelector(), Model.GetGroupBy(), Query, Having, null, null, state);
            var aliasName = AliasTableName;// ?? factory.CreateDataTableMappingSql(_model.EntityMapping, state);
            sb.Append(factory.CreateAliasQuerySql(command.CommandText, aliasName));
            return sb.ToString();
        }
    }
}

