using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Light.Data
{
	internal class SingleParameterLambdaState : LambdaState
	{
		private readonly string _singleEntityName;

		private readonly RelationMap _singleEntityMap;

		public SingleParameterLambdaState (ParameterExpression parameter)
		{
			_singleEntityName = parameter.Name;
			var entityMapping = DataEntityMapping.GetEntityMapping (parameter.Type);
			_singleEntityMap = entityMapping.GetRelationMap ();
		}

		public DataEntityMapping MainMapping => _singleEntityMap.RootMapping;

		public override bool CheckParameter (string name, Type type)
		{
			return _singleEntityName == name && _singleEntityMap.RootMapping.ObjectType == type;
		}

		public override DataFieldInfo GetDataFieldInfo (string fullPath)
		{
			var index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index < 0) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
			}
			var name = fullPath.Substring (0, index);
			var path = fullPath.Substring (index);
			if (_singleEntityName != name) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
			}
			var info = _singleEntityMap.GetFieldInfoForPath (path);
			return info;
		}

		public override LambdaPathType ParsePath (string fullPath)
		{
			var index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index == -1) {
				if (fullPath == _singleEntityName) {
					return LambdaPathType.Parameter;
				}
				else {
					throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
				}
			}
			var name = fullPath.Substring (0, index);
			var path = fullPath.Substring (index);
			if (_singleEntityName != name) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
			}
			if (_singleEntityMap.CheckIsField (path)) {
				return LambdaPathType.Field;
			}

			if (_singleEntityMap.CheckIsRelateEntity (path)) {
				return LambdaPathType.RelateEntity;
			}

			if (_singleEntityMap.CheckIsEntityCollection (path)) {
				return LambdaPathType.RelateCollection;
			}

			return LambdaPathType.None;
		}

		public override ISelector CreateSelector (string [] fullPaths)
		{
			var list = new List<string> ();
			foreach (var fullPath in fullPaths) {
				var index = fullPath.IndexOf (".", StringComparison.Ordinal);
				if (index < 0) {
					if (fullPath == _singleEntityName) {
						if (!list.Contains (string.Empty)) {
							list.Add (string.Empty);
						}
					}
					else {
						throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
					}
				}
				else {
					var name = fullPath.Substring (0, index);
					var path = fullPath.Substring (index);
					if (name == _singleEntityName) {
						if (!list.Contains (path)) {
							list.Add (path);
						}
					}
					else {
						throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
					}
				}
			}
			return _singleEntityMap.CreateSelector (list.ToArray ());
		}

	}
}

