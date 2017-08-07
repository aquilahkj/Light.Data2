using System;
namespace Light.Data
{
	class ExistsSelector : ISelector
	{
		static ExistsSelector instance = new ExistsSelector ();

		internal static ExistsSelector Value {
			get {
				return instance;
			}
		}

		public string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			return factory.CreateSelectExistsSql ();
		}

		public string [] GetSelectFieldNames ()
		{
			return new string [0];
		}
	}
}
