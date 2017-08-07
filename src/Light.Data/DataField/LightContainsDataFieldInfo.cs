using System;
using System.Collections;
using System.Collections.Generic;

namespace Light.Data
{
    class LightContainsDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
    {
        bool _isNot;

        readonly object _collection;

        readonly DataFieldInfo _baseFieldInfo;

        public LightContainsDataFieldInfo(DataFieldInfo info, object collection)
            : base(info.TableMapping)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            this._collection = collection;
            this._baseFieldInfo = info;
        }

        public void SetNot()
        {
            _isNot = !_isNot;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            string sql = state.GetDataSql(this, isFullName);
            if (sql != null) {
                return sql;
            }

            object obj = _baseFieldInfo.CreateSqlString(factory, isFullName, state);
            IEnumerable values = (IEnumerable)LambdaExpressionExtend.ConvertLambdaObject(_collection);
            List<string> list = new List<string>();
            foreach (object item in values) {
                list.Add(state.AddDataParameter(factory, _baseFieldInfo.ToParameter(item)));
            }

            sql = factory.CreateCollectionParamsQuerySql(obj, _isNot ? QueryCollectionPredicate.NotIn : QueryCollectionPredicate.In, list);
            state.SetDataSql(this, isFullName, sql);
            return sql;
        }

        public QueryExpression ConvertToExpression()
        {
            return new LightContainsQueryExpression(this);
        }
    }
}

