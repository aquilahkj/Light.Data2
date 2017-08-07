using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Light.Data
{
	class SingleParameterLambdaState : LambdaState
	{
		readonly string singleEntityName;

		readonly RelationMap singleEntityMap;

		public SingleParameterLambdaState (ParameterExpression parameter)
		{
			this.singleEntityName = parameter.Name;
			Type type = parameter.Type;
			DataEntityMapping entityMapping = DataEntityMapping.GetEntityMapping (type);
			this.singleEntityMap = entityMapping.GetRelationMap ();
		}

		public DataEntityMapping MainMapping {
			get {
				return singleEntityMap.RootMapping;
			}
		}

		public override bool CheckPamramter (string name, Type type)
		{
			return singleEntityName == name && singleEntityMap.RootMapping.ObjectType == type;
		}

		public override DataFieldInfo GetDataFieldInfo (string fullPath)
		{
			int index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index < 0) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
			}
			string name = fullPath.Substring (0, index);
			string path = fullPath.Substring (index);
			if (singleEntityName != name) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
			}
			DataFieldInfo info = singleEntityMap.GetFieldInfoForPath (path);
			return info;
		}

		public override LambdaPathType ParsePath (string fullPath)
		{
			int index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index == -1) {
				if (fullPath == singleEntityName) {
					return LambdaPathType.Parameter;
				}
				else {
					throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
				}
			}
			string name = fullPath.Substring (0, index);
			string path = fullPath.Substring (index);
			if (singleEntityName != name) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
			}
			if (singleEntityMap.CheckIsField (path)) {
				return LambdaPathType.Field;
			}
			else if (singleEntityMap.CheckIsRelateEntity (path)) {
				return LambdaPathType.RelateEntity;
			}
			else if (singleEntityMap.CheckIsEntityCollection (path)) {
				return LambdaPathType.RelateCollection;
			}
			else {
				return LambdaPathType.None;
			}
		}

		public override ISelector CreateSelector (string [] fullPaths)
		{
			List<string> list = new List<string> ();
			foreach (string fullPath in fullPaths) {
				int index = fullPath.IndexOf (".", StringComparison.Ordinal);
				if (index < 0) {
					if (fullPath == singleEntityName) {
						if (!list.Contains (string.Empty)) {
							list.Add (string.Empty);
						}
					}
					else {
						throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
					}
				}
				else {
					string name = fullPath.Substring (0, index);
					string path = fullPath.Substring (index);
					if (name == singleEntityName) {
						if (!list.Contains (name)) {
							list.Add (path);
						}
					}
					else {
						throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
					}
				}
			}
			return singleEntityMap.CreateSelector (list.ToArray ());
		}

	}
}

