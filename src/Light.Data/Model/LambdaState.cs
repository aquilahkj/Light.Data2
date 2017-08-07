using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Light.Data
{
	abstract class LambdaState
	{
		public abstract bool CheckPamramter (string name, Type type);

		public abstract DataFieldInfo GetDataFieldInfo (string fullPath);

		public abstract LambdaPathType ParsePath (string fullPath);

		public abstract ISelector CreateSelector (string[] fullPaths);

		bool mutliEntity;

		public bool MutliEntity {
			get {
				return mutliEntity;
			}

			set {
				mutliEntity = value;
			}
		}

		//bool aggregateField;

		//public bool AggregateField {
		//	get {
		//		return aggregateField;
		//	}

		//	set {
		//		aggregateField = value;
		//	}
		//}
	}

	enum LambdaPathType
	{
		None,
		Parameter,
		Field,
		RelateEntity,
		RelateCollection
	}
}

