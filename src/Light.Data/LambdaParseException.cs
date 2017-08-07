using System;
namespace Light.Data
{
	class LambdaParseException : Exception
	{
		public LambdaParseException (string message)
			: base (message)
		{
		}

		public LambdaParseException (string message, params object [] args)
			: base (string.Format (message, args))
		{
		}
	}
}

