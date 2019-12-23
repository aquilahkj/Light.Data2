using System;
using System.Collections.Generic;

namespace Light.Data
{
	internal class LightMathFunctionDataFieldInfo : LightDataFieldInfo
	{
		private readonly MathFunction _function;

		private readonly object [] _argsObjects;

		public LightMathFunctionDataFieldInfo (DataEntityMapping mapping, MathFunction function, params object [] argsObjects)
			: base (mapping)
		{
			if (argsObjects == null || argsObjects.Length == 0)
				throw new ArgumentNullException (nameof (argsObjects));
			if (function == MathFunction.Atan2 || function == MathFunction.Max || function == MathFunction.Min || function == MathFunction.Pow) {
				if (argsObjects.Length != 2) {
					throw new ArgumentNullException (nameof (argsObjects));
				}
			}
			if (function == MathFunction.Log || function == MathFunction.Round) {
				if (argsObjects.Length > 2) {
					throw new ArgumentNullException (nameof (argsObjects));
				}
			}
			_function = function;
			_argsObjects = argsObjects;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			var objectList = new List<object> ();
			foreach (var item in _argsObjects) {
				object obj1;
				var info1 = item as DataFieldInfo;
				if (!Equals (info1, null)) {
					obj1 = info1.CreateSqlString (factory, isFullName, state);
				}
				else {
					obj1 = LambdaExpressionExtend.ConvertLambdaObject (item);
					obj1 = state.AddDataParameter (factory, obj1);
				}
				objectList.Add (obj1);
			}
			switch (_function) {
			case MathFunction.Abs:
				sql = factory.CreateAbsSql (objectList [0]);
				break;
			case MathFunction.Sign:
				sql = factory.CreateSignSql (objectList [0]);
				break;
			case MathFunction.Sin:
				sql = factory.CreateSinSql (objectList [0]);
				break;
			case MathFunction.Cos:
				sql = factory.CreateCosSql (objectList [0]);
				break;
			case MathFunction.Tan:
				sql = factory.CreateTanSql (objectList [0]);
				break;
			case MathFunction.Atan:
				sql = factory.CreateAtanSql (objectList [0]);
				break;
			case MathFunction.Asin:
				sql = factory.CreateAsinSql (objectList [0]);
				break;
			case MathFunction.Acos:
				sql = factory.CreateAcosSql (objectList [0]);
				break;
			case MathFunction.Atan2:
				sql = factory.CreateAtan2Sql (objectList [0], objectList [1]);
				break;
			case MathFunction.Ceiling:
				sql = factory.CreateCeilingSql (objectList [0]);
				break;
			case MathFunction.Floor:
				sql = factory.CreateFloorSql (objectList [0]);
				break;
			case MathFunction.Round:
				if (objectList.Count == 2) {
					sql = factory.CreateRoundSql(objectList [0], objectList [1]);
				}
				else {
					sql = factory.CreateRoundSql (objectList [0], 0);
				}
				break;
			case MathFunction.Truncate:
				sql = factory.CreateTruncateSql (objectList [0]);
				break;
			case MathFunction.Log:
				if (objectList.Count == 2) {
					sql = factory.CreateLogSql (objectList [0], objectList [1]);
				}
				else {
					sql = factory.CreateLogSql (objectList [0]);
				}
				break;
			case MathFunction.Log10:
				sql = factory.CreateLog10Sql (objectList [0]);
				break;
			case MathFunction.Exp:
				sql = factory.CreateExpSql (objectList [0]);
				break;
			case MathFunction.Pow:
				sql = factory.CreatePowSql (objectList [0], objectList [1]);
				break;
			case MathFunction.Sqrt:
				sql = factory.CreateSqrtSql (objectList [0]);
				break;
			case MathFunction.Max:
				sql = factory.CreateMaxSql (objectList [0], objectList [1]);
				break;
			case MathFunction.Min:
				sql = factory.CreateMinSql (objectList [0], objectList [1]);
				break;
			}
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}



		//protected override bool EqualsDetail (DataFieldInfo info)
		//{
		//	if (base.EqualsDetail (info)) {
		//		LambdaMathFunctionDataFieldInfo target = info as LambdaMathFunctionDataFieldInfo;
		//		if (!Object.Equals (target, null)) {
		//			return this._function == target._function
		//				       && Utility.EnumableObjectEquals (this._values, target._values);
		//		}
		//		else {
		//			return false;
		//		}
		//	}
		//	else {
		//		return false;
		//	}
		//}
	}
}

