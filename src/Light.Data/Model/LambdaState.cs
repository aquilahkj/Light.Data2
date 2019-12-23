using System;

namespace Light.Data
{
	internal abstract class LambdaState
	{
		public abstract bool CheckParameter (string name, Type type);

		public abstract DataFieldInfo GetDataFieldInfo (string fullPath);

		public abstract LambdaPathType ParsePath (string fullPath);

		public abstract ISelector CreateSelector (string[] fullPaths);

		public bool MultiEntity { get; set; }
		
		public  LambdaMode Mode { get; set; }
	}

	internal enum LambdaPathType
	{
		None,
		Parameter,
		Field,
		RelateEntity,
		RelateCollection
	}

	internal enum LambdaMode
	{
		QueryMode,
		OutputMode
	}
}

