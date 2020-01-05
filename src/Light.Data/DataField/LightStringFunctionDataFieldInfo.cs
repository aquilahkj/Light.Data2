using System;
using System.Collections.Generic;

namespace Light.Data
{
    internal class LightStringFunctionDataFieldInfo : LightDataFieldInfo
    {
        private readonly StringFunction _function;

        private readonly object _callObject;

        private readonly object[] _argsObjects;

        public LightStringFunctionDataFieldInfo(DataEntityMapping mapping, StringFunction function, object callObject,
            params object[] argsObjects)
            : base(mapping)
        {
            if (callObject == null)
                throw new ArgumentNullException(nameof(callObject));
            if (function == StringFunction.ToLower || function == StringFunction.ToUpper ||
                function == StringFunction.Trim)
            {
                if (argsObjects != null && argsObjects.Length > 0)
                {
                    throw new ArgumentNullException(nameof(argsObjects));
                }
            }

            if (function == StringFunction.Replace)
            {
                if (argsObjects == null || argsObjects.Length != 2)
                {
                    throw new ArgumentNullException(nameof(argsObjects));
                }
            }

            if (function == StringFunction.Substring || function == StringFunction.IndexOf)
            {
                if (argsObjects == null || argsObjects.Length > 2)
                {
                    throw new ArgumentNullException(nameof(argsObjects));
                }
            }

            _function = function;
            _callObject = callObject;
            _argsObjects = argsObjects;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            var sql = state.GetDataSql(this, isFullName);
            if (sql != null)
            {
                return sql;
            }

            var objectList = new List<object>();

            object obj;
            var info = _callObject as DataFieldInfo;
            if (!Equals(info, null))
            {
                obj = info.CreateSqlString(factory, isFullName, state);
            }
            else
            {
                obj = LambdaExpressionExtend.ConvertLambdaObject(_callObject);
                obj = state.AddDataParameter(factory, obj);
            }

            if (_argsObjects != null)
            {
                foreach (var item in _argsObjects)
                {
                    object obj1;
                    var info1 = item as DataFieldInfo;
                    if (!Equals(info1, null))
                    {
                        obj1 = info1.CreateSqlString(factory, isFullName, state);
                    }
                    else
                    {
                        obj1 = LambdaExpressionExtend.ConvertLambdaObject(item);
                        obj1 = state.AddDataParameter(factory, obj1);
                    }

                    objectList.Add(obj1);
                }
            }

            switch (_function)
            {
                case StringFunction.Substring:
                    sql = factory.CreateSubStringSql(obj, objectList[0], objectList.Count == 2 ? objectList[1] : null);
                    break;
                case StringFunction.IndexOf:
                    sql = factory.CreateIndexOfSql(obj, objectList[0], objectList.Count == 2 ? objectList[1] : null);
                    break;
                case StringFunction.Replace:
                    sql = factory.CreateReplaceSql(obj, objectList[0], objectList[1]);
                    break;
                case StringFunction.ToLower:
                    sql = factory.CreateToLowerSql(obj);
                    break;
                case StringFunction.ToUpper:
                    sql = factory.CreateToUpperSql(obj);
                    break;
                case StringFunction.Trim:
                    sql = factory.CreateTrimSql(obj);
                    break;
            }

            state.SetDataSql(this, isFullName, sql);
            return sql;
        }
    }
}