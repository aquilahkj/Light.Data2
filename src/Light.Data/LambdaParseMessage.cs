using System;
namespace Light.Data
{
	static class LambdaParseMessage
	{
		public const string ExpressionParameterCountError = "expression parameter count error";

		public const string ExpressionParameterTypeError = "expression parameter type error,name={0},type={1}";

		public const string ExpressionTypeInvalid = "expression type invalid";

		public const string ExpressionMemberInvalid = "expression member invalid";

		public const string ExpressionNotContainDataField = "expression not contain data field";

		public const string ExpressionNotContainAggregateFunction = "expression not contain aggregate function";

		public const string ExpressionNotAllowNoDataField = "expression {0} not contain data field";

		public const string ExpressionFieldPathError = "expression field path {0} error";

		public const string ExpressionFieldPathNotExists = "expression field path {0} error";

		public const string ExpressionNoMember = "member expression no member";

		public const string ExpressionNoArguments = "new expression no arguments";

		public const string ExpressionBindingError = "member expression member {0} error";

		public const string ExpressionUnsupportRelateField = "expression unsupport relate field";

		public const string ExpressionUnsupportAggregateField = "expression unsupport aggregate field";

		public const string ExtendExpressionError = "extend expression error";

		public const string ExpressionParseFieldFailed = "expression parse field failed";

		public const string ExpressionNodeTypeUnsuppore = "expression node type {0} unsupport";

		public const string BinaryExpressionNotAllowBothConstantValue = "BinaryExpression not allow both constant value";

		public const string MemberExpressionTypeUnsupport = "MemberExpression type {0} unsupport";

		public const string MethodExpressionTypeUnsupport = "MethodExpression type {0} unsupport";

		public const string MethodExpressionArgumentError = "MethodExpression {0}.{1} argument error";

		public const string MemberExpressionMemberUnsupport = "MemberExpression member {0}.{1} unsupport";

		public const string MethodExpressionMethodUnsupport = "MethodExpression method {0}.{1} unsupport";

		public const string NotSupportRelateEnityJoinSelect = "unsupport RelateEnity join select";
	}
}

