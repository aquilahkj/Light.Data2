using System;
namespace Light.Data
{
	internal class LightNewArrayDataFieldInfo : LightDataFieldInfo
	{
		public LightNewArrayDataFieldInfo (DataEntityMapping mapping, params object [] values)
			: base (mapping)
		{
			if (values == null)
				throw new ArgumentNullException (nameof (values));
			Values = values;
		}

		public object [] Values { get; }

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			throw new NotSupportedException ();
		}
	}
}

