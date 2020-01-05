using System;
using System.Linq.Expressions;

namespace Light.Data
{
	internal class AggregateLambdaState : LambdaState
	{
		private readonly string aggregateName;

		private readonly Type aggregateType;

		private readonly AggregateModel aggregateModel;

		public AggregateLambdaState (ParameterExpression parameter, AggregateModel model)
		{
			aggregateModel = model;
			aggregateName = parameter.Name;
			aggregateType = parameter.Type;
			MainMapping = model.EntityMapping;
		}

		public DataEntityMapping MainMapping { get; }

		public override bool CheckParameter (string name, Type type)
		{
			return aggregateName == name && aggregateType == type;
		}

		public override ISelector CreateSelector (string [] fullPaths)
		{
			throw new NotSupportedException ();
		}

		public override DataFieldInfo GetDataFieldInfo (string fullPath)
		{
			var index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index < 0) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
			}
			var name = fullPath.Substring (0, index);
			var path = fullPath.Substring (index + 1);
			if (aggregateName != name) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
			}
			var info = aggregateModel.GetAggregateData (path);
			return info;
		}

		public override LambdaPathType ParsePath (string fullPath)
		{
			var index = fullPath.IndexOf (".", StringComparison.Ordinal);
			if (index == -1)
			{
				if (fullPath == aggregateName) {
					return LambdaPathType.Parameter;
				}

				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathError, fullPath);
			}
			var name = fullPath.Substring (0, index);
			var path = fullPath.Substring (index + 1);
			if (aggregateName != name) {
				throw new LambdaParseException (LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
			}
			if (aggregateModel.CheckName (path)) {
				return LambdaPathType.Field;
			}

			return LambdaPathType.None;
		}
	}
}

