using System;
using System.Linq.Expressions;

namespace Light.Data
{
	internal class RelateParameterLambdaState : LambdaState
	{
		private readonly string singleEntityName;

		private readonly RelationMap singleEntityMap;

		private readonly LambdaState state;

		public RelateParameterLambdaState (ParameterExpression parameter, LambdaState state)
		{
			singleEntityName = parameter.Name;
			var type = parameter.Type;
			var entityMapping = DataEntityMapping.GetEntityMapping (type);
			singleEntityMap = entityMapping.GetRelationMap ();
			this.state = state;
		}

		public DataEntityMapping MainMapping => singleEntityMap.RootMapping;


		public override bool CheckParameter (string name, Type type)
		{
			if (singleEntityName == name && singleEntityMap.RootMapping.ObjectType == type) {
				return true;
			}

			return state.CheckParameter (name, type);
		}

		public override ISelector CreateSelector (string [] fullPaths)
		{
			return ExistsSelector.Value;
		}

		public override DataFieldInfo GetDataFieldInfo (string fullPath)
		{
			var index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index < 0) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
			}
			var name = fullPath.Substring (0, index);
			var path = fullPath.Substring (index);
			if (singleEntityName == name) {
				var info = singleEntityMap.GetFieldInfoForPath (path);
				return info;
			}

			return state.GetDataFieldInfo (fullPath);
		}

		public override LambdaPathType ParsePath (string fullPath)
		{
			var index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index == -1)
			{
				if (fullPath == singleEntityName) {
					return LambdaPathType.Parameter;
				}

				return state.ParsePath (fullPath);
			}
			var name = fullPath.Substring (0, index);
			var path = fullPath.Substring (index);
			if (singleEntityName != name) {
				return state.ParsePath (fullPath);
			}
			if (singleEntityMap.CheckIsField (path)) {
				return LambdaPathType.Field;
			}

			if (singleEntityMap.CheckIsRelateEntity (path)) {
				return LambdaPathType.RelateEntity;
			}

			if (singleEntityMap.CheckIsEntityCollection (path)) {
				return LambdaPathType.RelateCollection;
			}

			return LambdaPathType.None;
		}
	}
}
