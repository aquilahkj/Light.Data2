using System;
using System.Collections.Generic;

namespace Light.Data
{
	class LightStringConcatDataFieldInfo : LightDataFieldInfo
	{
		readonly object [] _values;

		public LightStringConcatDataFieldInfo (DataEntityMapping mapping, params object [] values)
			: base (mapping)
		{
			if (values == null)
				throw new ArgumentNullException (nameof (values));
			this._values = values;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			List<object> objectList = new List<object> ();
			foreach (object item in _values) {
				object obj1;
				DataFieldInfo info1 = item as DataFieldInfo;
				if (!Object.Equals (info1, null)) {
					obj1 = info1.CreateSqlString (factory, isFullName, state);
				}
				else {
					obj1 = LambdaExpressionExtend.ConvertLambdaObject (item);
					if (obj1 == null) {
						obj1 = string.Empty;
					}
					else if (!(obj1 is string)) {
						obj1 = obj1.ToString ();
					}
					obj1 = state.AddDataParameter (factory, obj1);
				}
				objectList.Add (obj1);
			}
			sql = factory.CreateConcatSql (objectList.ToArray ());
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

