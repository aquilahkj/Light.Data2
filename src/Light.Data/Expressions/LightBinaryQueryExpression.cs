namespace Light.Data
{
    internal class LightBinaryQueryExpression : QueryExpression
    {
        private readonly QueryPredicate _predicate;

        private readonly object _left;

        private readonly object _right;

        public LightBinaryQueryExpression(DataEntityMapping mapping, QueryPredicate predicate, object left, object right)
            : base(mapping)
        {
            _predicate = predicate;
            _left = left;
            _right = right;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            string sql = null;

            var leftInfo = _left as DataFieldInfo;
            var rightInfo = _right as DataFieldInfo;
            if (!Equals(leftInfo, null) && !Equals(rightInfo, null)) {
                var leftSql = leftInfo.CreateSqlString(factory, isFullName, state);
                var rightSql = rightInfo.CreateSqlString(factory, isFullName, state);
                sql = factory.CreateSingleParamSql(leftSql, _predicate, rightSql);
            }
            else if (!Equals(leftInfo, null)) {
                var leftSql = leftInfo.CreateSqlString(factory, isFullName, state);
                var right = LambdaExpressionExtend.ConvertLambdaObject(_right);
                if (Equals(right, null)) {
                    bool predicate;
                    if (_predicate == QueryPredicate.Eq) {
                        predicate = true;
                    }
                    else if (_predicate == QueryPredicate.NotEq) {
                        predicate = false;
                    }
                    else {
                        throw new LightDataException(string.Format(SR.UnsupportPredicate, _predicate, "null"));
                    }
                    sql = factory.CreateNullQuerySql(leftSql, predicate);
                }
                else if (right is bool ret) {
                    bool predicate;
                    if (_predicate == QueryPredicate.Eq) {
                        predicate = true;
                    }
                    else if (_predicate == QueryPredicate.NotEq) {
                        predicate = false;
                    }
                    else {
                        throw new LightDataException(string.Format(SR.UnsupportPredicate, _predicate, "bool"));
                    }

                    sql = factory.CreateBooleanQuerySql(leftSql, ret, predicate, false);
                }
                else
                {
                    right = right.AdjustValue();
                    var name = state.AddDataParameter(factory, leftInfo.ToParameter(right));
                    sql = factory.CreateSingleParamSql(leftSql, _predicate, name);
                }
            }
            else if (!Equals(rightInfo, null)) {
                var rightSql = rightInfo.CreateSqlString(factory, isFullName, state);
                var left = LambdaExpressionExtend.ConvertLambdaObject(_left);
                if (Equals(left, null)) {
                    bool predicate;
                    if (_predicate == QueryPredicate.Eq) {
                        predicate = true;
                    }
                    else if (_predicate == QueryPredicate.NotEq) {
                        predicate = false;
                    }
                    else {
                        throw new LightDataException(string.Format(SR.UnsupportPredicate, _predicate, "null"));
                    }
                    sql = factory.CreateNullQuerySql(rightSql, predicate);
                }
                else if (left is bool ret) {
                    bool predicate;
                    if (_predicate == QueryPredicate.Eq) {
                        predicate = true;
                    }
                    else if (_predicate == QueryPredicate.NotEq) {
                        predicate = false;
                    }
                    else {
                        throw new LightDataException(string.Format(SR.UnsupportPredicate, _predicate, "bool"));
                    }

                    sql = factory.CreateBooleanQuerySql(rightSql, ret, predicate, true);
                }
                else {
                    left = left.AdjustValue();
                    var name = state.AddDataParameter(factory, rightInfo.ToParameter(left));
                    sql = factory.CreateSingleParamSql(name, _predicate, rightSql);
                }
            }
            else {
                throw new LightDataException(SR.DataFieldContentError);
            }
            return sql;
        }
    }
}

