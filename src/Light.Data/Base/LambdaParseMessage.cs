namespace Light.Data
{
	internal static class LambdaParseMessage
	{
		public const string ParameterCountError = "The parameter count error";

		public const string ParameterTypeError = "The parameter type error, name={0}, type={1}";

		public const string ExpressionTypeInvalid = "The expression type invalid";

		public const string ExpressionMemberInvalid = "The expression member invalid";

		public const string ExpressionNotContainDataField = "The expression does not contain data field";

		public const string ExpressionNotContainAggregateFunction = "The expression does not contain any aggregate functions";

		public const string ExpressionNotAllowNoDataField = "The expression {0} does not contain data field";
		
		public const string ExpressionDataFieldError = "The expression {0} data field error";

		public const string ExpressionFieldPathError = "The expression field path {0} error";

		public const string ExpressionFieldPathNotExists = "The expression field path {0} does not exists";

		public const string ExpressionNoMember = "The MemberExpression has not member";

		public const string ExpressionNoArguments = "The NewExpression has not any arguments";

		public const string ExpressionMemberError = "The MemberExpression member {0} error";

		public const string ExpressionUnsupportRelateField = "The expression unsupport relate field";

		public const string ExpressionUnsupportAggregateField = "The expression unsupport aggregate field";

		public const string ExtendExpressionError = "The extend expression error";

		public const string ExpressionParseFieldFailed = "The expression parse field failed";

		public const string ExpressionNodeTypeUnsupport = "The expression node type {0} unsupport";

		public const string BinaryExpressionNotAllowBothConstantValue = "The BinaryExpression not allow both constant value";

		public const string MemberExpressionTypeUnsupport = "The MemberExpression type {0} unsupport";

		public const string MethodExpressionTypeUnsupport = "The MethodExpression type {0} unsupport";

		public const string MethodExpressionArgumentError = "The MethodExpression {0}.{1} argument error";

		public const string MemberExpressionMemberUnsupport = "MemberExpression member {0}.{1} unsupport";

		public const string MethodExpressionMethodUnsupport = "The MethodExpression method {0}.{1} unsupport";

		public const string NotSupportRelateEntityJoinSelect = "The unsupport RelateEntity join select";
	}
}

