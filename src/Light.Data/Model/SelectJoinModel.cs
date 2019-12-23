using System.Text;

namespace Light.Data
{
	internal class SelectJoinModel : IJoinModel
	{
		public SelectModel Model { get; }

		public JoinConnect Connect { get; }

		public IJoinTableMapping JoinMapping { get; }

		public QueryExpression Query { get; set; }

		public OrderExpression Order { get; set; }

		public bool Distinct { get; set; }

		public string AliasTableName { get; }

		public bool NoDataSetEntityNull { get; set; }

        public SelectJoinModel (SelectModel model, string aliasTableName, JoinConnect connect, QueryExpression query, OrderExpression order, JoinSetting setting)
		{
			Model = model;
			Connect = connect;
			Query = query;
			Order = order;
			AliasTableName = aliasTableName;
			JoinMapping = model.JoinTableMapping;
            if ((setting & JoinSetting.QueryDistinct) == JoinSetting.QueryDistinct) {
                Distinct = true;
            }
            if ((setting & JoinSetting.NoDataSetEntityNull) == JoinSetting.NoDataSetEntityNull) {
                NoDataSetEntityNull = true;
            }
        }

		public string CreateSqlString (CommandFactory factory, CreateSqlState state)
		{
			var sb = new StringBuilder ();
			var command = factory.CreateSelectCommand (Model.EntityMapping, Model.CreateSelector (), Query, Order, Distinct, null, state);
            var aliasName = AliasTableName;// ?? factory.CreateDataTableMappingSql(_model.EntityMapping, state); 
			sb.Append (factory.CreateAliasQuerySql (command.CommandText, aliasName));
			return sb.ToString ();
		}
	}
}
