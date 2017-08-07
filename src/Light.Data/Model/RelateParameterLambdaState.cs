using System;
using System.Linq.Expressions;

namespace Light.Data
{
	class RelateParameterLambdaState : LambdaState
	{
		readonly string singleEntityName;

		readonly RelationMap singleEntityMap;

		readonly LambdaState state;

		public RelateParameterLambdaState (ParameterExpression parameter, LambdaState state)
		{
			this.singleEntityName = parameter.Name;
			Type type = parameter.Type;
			DataEntityMapping entityMapping = DataEntityMapping.GetEntityMapping (type);
			this.singleEntityMap = entityMapping.GetRelationMap ();
			this.state = state;
		}

		public DataEntityMapping MainMapping {
			get {
				return singleEntityMap.RootMapping;
			}
		}


		public override bool CheckPamramter (string name, Type type)
		{
			if (singleEntityName == name && singleEntityMap.RootMapping.ObjectType == type) {
				return true;
			}
			else {
				return state.CheckPamramter (name, type);
			}
		}

		public override ISelector CreateSelector (string [] fullPaths)
		{
			return ExistsSelector.Value;
		}

		public override DataFieldInfo GetDataFieldInfo (string fullPath)
		{
			int index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index < 0) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
			}
			string name = fullPath.Substring (0, index);
			string path = fullPath.Substring (index);
			if (singleEntityName == name) {
				DataFieldInfo info = singleEntityMap.GetFieldInfoForPath (path);
				return info;
			}
			else {
				return state.GetDataFieldInfo (fullPath);
			}
		}

		public override LambdaPathType ParsePath (string fullPath)
		{
			int index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index == -1) {
				if (fullPath == singleEntityName) {
					return LambdaPathType.Parameter;
				}
				else {
					return state.ParsePath (fullPath);
				}
			}
			string name = fullPath.Substring (0, index);
			string path = fullPath.Substring (index);
			if (singleEntityName != name) {
				return state.ParsePath (fullPath);
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
	}
}
