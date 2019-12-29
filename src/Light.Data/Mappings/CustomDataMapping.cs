using System;
using System.Data;

namespace Light.Data
{
	internal abstract class CustomDataMapping : DataMapping, IJoinTableMapping
	{
		protected CustomDataMapping (Type type)
			: base (type)
		{
		}

		public abstract object LoadAliasJoinTableData (DataContext context, IDataReader dataReader, QueryState queryState, string aliasName);
	}
}

