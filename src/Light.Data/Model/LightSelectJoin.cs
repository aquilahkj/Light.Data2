using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    class LightSelectJoin<K> : SelectJoinBase<K>
    {
        readonly QueryExpression _query;

        readonly OrderExpression _order;

        readonly bool _distinct;

        readonly Region _region;

        readonly SafeLevel _level;

        List<int> nodataSetNull = null;

        public override QueryExpression QueryExpression {
            get {
                return _query;
            }
        }

        public override OrderExpression OrderExpression {
            get {
                return _order;
            }
        }

        public override bool Distinct {
            get {
                return _distinct;
            }
        }

        public override Region Region {
            get {
                return _region;
            }
        }

        public override SafeLevel Level {
            get {
                return _level;
            }
        }

        protected LightSelectJoin(DataContext context, LambdaExpression expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
            : base(context, expression, models, maps)
        {
            _query = query;
            _order = order;
            _distinct = distinct;
            _region = region;
            _level = level;
        }

        #region IEnumerable implementation

        public override IEnumerator<K> GetEnumerator()
        {
            return Context.QueryJoinDataReader<K>(Mapping, Selector, Models, _query, _order, _distinct, _region, Dele, nodataSetNull).GetEnumerator();
        }

        #endregion

        public override ISelectJoin<K> NoDataSetEntityNull(int entityIndex)
        {
            if (entityIndex < 0) {
                throw new ArgumentOutOfRangeException(nameof(entityIndex));
            }
            if (entityIndex >= Models.Count) {
                throw new ArgumentOutOfRangeException(nameof(entityIndex));
            }
            if (nodataSetNull == null) {
                nodataSetNull = new List<int>();
            }
            if (!nodataSetNull.Contains(entityIndex)) {
                nodataSetNull.Add(entityIndex);
            }
            return this;
        }

        public override List<K> ToList()
        {
            List<K> list = Context.QueryJoinDataList<K>(Mapping, Selector, Models, _query, _order, _distinct, _region, Dele, nodataSetNull);
            return list;
        }

        public override K[] ToArray()
        {
            return ToList().ToArray();
        }

        public override K First()
        {
            return ElementAt(0);
        }

        public override K ElementAt(int index)
        {
            K target = default(K);
            Region region = new Region(index, 1);
            target = Context.QueryJoinDataSingle<K>(Mapping, Selector, Models, _query, _order, _distinct, region, Dele, nodataSetNull);
            return target;
        }

        #region async

        public async override Task<List<K>> ToListAsync(CancellationToken cancellationToken)
        {
            List<K> list = await Context.QueryJoinDataListAsync<K>(Mapping, Selector, Models, _query, _order, _distinct, _region, Dele, nodataSetNull, cancellationToken);
            return list;
        }

        public async override Task<List<K>> ToListAsync()
        {
            return await ToListAsync(CancellationToken.None);
        }

        public async override Task<K[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            List<K> list = await ToListAsync(cancellationToken);
            return list.ToArray();
        }

        public async override Task<K[]> ToArrayAsync()
        {
            return await ToArrayAsync(CancellationToken.None);
        }

        public async override Task<K> FirstAsync(CancellationToken cancellationToken)
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public async override Task<K> FirstAsync()
        {
            return await FirstAsync(CancellationToken.None);
        }

        public async override Task<K> ElementAtAsync(int index, CancellationToken cancellationToken)
        {
            K target = default(K);
            Region region = new Region(index, 1);
            target = await Context.QueryJoinDataSingleAsync<K>(Mapping, Selector, Models, _query, _order, _distinct, region, Dele, nodataSetNull, cancellationToken);
            return target;
        }

        public async override Task<K> ElementAtAsync(int index)
        {
            return await ElementAtAsync(index, CancellationToken.None);
        }
        #endregion
    }
}

