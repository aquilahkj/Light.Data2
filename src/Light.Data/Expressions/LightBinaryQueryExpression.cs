using System;

namespace Light.Data
{
    class LightBinaryQueryExpression : QueryExpression
    {
        readonly QueryPredicate _predicate;

        readonly object _left;

        readonly object _right;

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

            DataFieldInfo leftInfo = _left as DataFieldInfo;
            DataFieldInfo rightInfo = _right as DataFieldInfo;
            if (!Object.Equals(leftInfo, null) && !Object.Equals(rightInfo, null)) {
                string leftSql = leftInfo.CreateSqlString(factory, isFullName, state);
                string rightSql = rightInfo.CreateSqlString(factory, isFullName, state);
                sql = factory.CreateSingleParamSql(leftSql, _predicate, rightSql);
            }
            else if (!Object.Equals(leftInfo, null)) {
                string leftSql = leftInfo.CreateSqlString(factory, isFullName, state);
                object right = LambdaExpressionExtend.ConvertLambdaObject(_right);
                if (Object.Equals(right, null)) {
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
                else if (right is bool) {
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
                    bool ret = (bool)right;
                    sql = factory.CreateBooleanQuerySql(leftSql, ret, predicate, false);
                }
                else {
                    string name = state.AddDataParameter(factory, right);
                    sql = factory.CreateSingleParamSql(leftSql, _predicate, name);
                }
            }
            else if (!Object.Equals(rightInfo, null)) {
                string rightSql = rightInfo.CreateSqlString(factory, isFullName, state);
                object left = LambdaExpressionExtend.ConvertLambdaObject(_left);
                if (Object.Equals(left, null)) {
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
                else if (left is bool) {
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
                    bool ret = (bool)left;
                    sql = factory.CreateBooleanQuerySql(rightSql, ret, predicate, true);
                }
                else {
                    string name = state.AddDataParameter(factory, left);
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

