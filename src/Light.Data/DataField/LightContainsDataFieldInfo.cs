using System;
using System.Collections;
using System.Collections.Generic;

namespace Light.Data
{
    internal class LightContainsDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
    {
        private bool _isNot;

        private readonly object _collection;

        private readonly DataFieldInfo _baseFieldInfo;

        public LightContainsDataFieldInfo(DataFieldInfo info, object collection)
            : base(info.TableMapping)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            _collection = collection;
            _baseFieldInfo = info;
        }

        public void SetNot()
        {
            _isNot = !_isNot;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            var sql = state.GetDataSql(this, isFullName);
            if (sql != null) {
                return sql;
            }

            object obj = _baseFieldInfo.CreateSqlString(factory, isFullName, state);
            var values = (IEnumerable)LambdaExpressionExtend.ConvertLambdaObject(_collection);
            var list = new List<string>();
            foreach (var item in values) {
                list.Add(state.AddDataParameter(factory, _baseFieldInfo.ToParameter(item.AdjustValue())));
            }

            if (list.Count > 0)
            {
                sql = factory.CreateCollectionParamsQuerySql(obj,
                    _isNot ? QueryCollectionPredicate.NotIn : QueryCollectionPredicate.In, list);
            }
            else
            {
                var value = _isNot;
                sql = factory.CreateBooleanConstantSql(value);
            }
            state.SetDataSql(this, isFullName, sql);
            return sql;
        }

        public QueryExpression ConvertToExpression()
        {
            return new LightContainsQueryExpression(this);
        }
    }
}

