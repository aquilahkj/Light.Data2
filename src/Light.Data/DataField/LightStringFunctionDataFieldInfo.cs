using System;
using System.Collections.Generic;

namespace Light.Data
{
	class LightStringFunctionDataFieldInfo : LightDataFieldInfo
	{
		readonly StringFunction _function;

		readonly object _callObject;

		readonly object [] _argsObjects;

		public LightStringFunctionDataFieldInfo (DataEntityMapping mapping, StringFunction function, object callObject, params object [] argsObjects)
			: base (mapping)
		{
			if (callObject == null)
				throw new ArgumentNullException (nameof (callObject));
			if (function == StringFunction.ToLower || function == StringFunction.ToUpper || function == StringFunction.Trim) {
				if (argsObjects != null && argsObjects.Length > 0) {
					throw new ArgumentNullException (nameof (argsObjects));
				}
			}
			if (function == StringFunction.Replace) {
				if (argsObjects == null || argsObjects.Length != 2) {
					throw new ArgumentNullException (nameof (argsObjects));
				}
			}
			if (function == StringFunction.Substring || function == StringFunction.IndexOf) {
				if (argsObjects == null || argsObjects.Length > 2) {
					throw new ArgumentNullException (nameof (argsObjects));
				}
			}
			_function = function;
			_callObject = callObject;
			_argsObjects = argsObjects;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			List<object> objectList = new List<object> ();

			object obj;
			DataFieldInfo info = _callObject as DataFieldInfo;
			if (!Object.Equals (info, null)) {
				obj = info.CreateSqlString (factory, isFullName, state);
			}
			else {
				obj = LambdaExpressionExtend.ConvertLambdaObject (_callObject);
				obj = state.AddDataParameter (factory, obj);
			}
            if (_argsObjects != null) {
                foreach (object item in _argsObjects) {
                    object obj1;
                    DataFieldInfo info1 = item as DataFieldInfo;
                    if (!Object.Equals(info1, null)) {
                        obj1 = info1.CreateSqlString(factory, isFullName, state);
                    } else {
                        obj1 = LambdaExpressionExtend.ConvertLambdaObject(item);
                        obj1 = state.AddDataParameter(factory, obj1);
                    }
                    objectList.Add(obj1);
                }
            }
			switch (_function) {
			case StringFunction.Substring:
				if (objectList.Count == 2) {
					sql = factory.CreateSubStringSql (obj, objectList [0], objectList [1]);
				}
				else {
					sql = factory.CreateSubStringSql (obj, objectList [0], null);
				}
				break;
			case StringFunction.IndexOf:
				if (objectList.Count == 2) {
					sql = factory.CreateIndexOfSql (obj, objectList [0], objectList [1]);
				}
				else {
					sql = factory.CreateIndexOfSql (obj, objectList [0], null);
				}
				break;
			case StringFunction.Replace:
				sql = factory.CreateReplaceSql (obj, objectList [0], objectList [1]);
				break;
			case StringFunction.ToLower:
				sql = factory.CreateToLowerSql (obj);
				break;
			case StringFunction.ToUpper:
				sql = factory.CreateToUpperSql (obj);
				break;
			case StringFunction.Trim:
				sql = factory.CreateTrimSql (obj);
				break;
			}
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

