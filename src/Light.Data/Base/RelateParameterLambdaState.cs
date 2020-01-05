using System;
using System.Linq.Expressions;

namespace Light.Data
{
	internal class RelateParameterLambdaState : LambdaState
	{
		private readonly string _singleEntityName;

		private readonly RelationMap _singleEntityMap;

		private readonly LambdaState _state;

		public RelateParameterLambdaState (ParameterExpression parameter, LambdaState state)
		{
			var entityMapping = DataEntityMapping.GetEntityMapping (parameter.Type);
			_singleEntityName = parameter.Name;
			_singleEntityMap = entityMapping.GetRelationMap ();
			_state = state;
		}

		public DataEntityMapping MainMapping => _singleEntityMap.RootMapping;


		public override bool CheckParameter (string name, Type type)
		{
			if (_singleEntityName == name && _singleEntityMap.RootMapping.ObjectType == type) {
				return true;
			}

			return _state.CheckParameter (name, type);
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
			if (_singleEntityName == name) {
				var info = _singleEntityMap.GetFieldInfoForPath (path);
				return info;
			}

			return _state.GetDataFieldInfo (fullPath);
		}

		public override LambdaPathType ParsePath (string fullPath)
		{
			var index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index == -1)
			{
				if (fullPath == _singleEntityName) {
					return LambdaPathType.Parameter;
				}

				return _state.ParsePath (fullPath);
			}
			var name = fullPath.Substring (0, index);
			var path = fullPath.Substring (index);
			if (_singleEntityName != name) {
				return _state.ParsePath (fullPath);
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
	}
}
