using System;
namespace Light.Data
{
	sealed class AllSelector : ISelector
	{
		static AllSelector instance = new AllSelector ();

		internal static AllSelector Value {
			get {
				return instance;
			}
		}

		public string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			return factory.CreateSelectAllSql ();
		}

		public string [] GetSelectFieldNames ()
		{
			return new string [0];
		}
	}
}
