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
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, nodataSetNull, _region);
            return _context.QueryDataDefineReader<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele).GetEnumerator();
            //return Context.QueryJoinDataReader<K>(Mapping, Selector, Models, _query, _order, _distinct, _region, _level, Dele, nodataSetNull).GetEnumerator();
        }

        #endregion

        public override ISelectJoin<K> NoDataSetEntityNull(int entityIndex)
        {
            if (entityIndex < 0 || entityIndex >= Models.Count) {
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
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, nodataSetNull, _region);
            return _context.QueryDataDefineList<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele);
            //List<K> list = Context.QueryJoinDataList<K>(Mapping, Selector, Models, _query, _order, _distinct, _region, _level, Dele, nodataSetNull);
            //return list;
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
            Region region = new Region(index, 1);
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, nodataSetNull, region);
            return _context.QueryDataDefineSingle<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, Dele);
            //K target = default(K);
            //Region region = new Region(index, 1);
            //target = Context.QueryJoinDataSingle<K>(Mapping, Selector, Models, _query, _order, _distinct, region, _level, Dele, nodataSetNull);
            //return target;
        }

        #region async

        public async override Task<List<K>> ToListAsync(CancellationToken cancellationToken)
        {
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, nodataSetNull, _region);
            return await _context.QueryDataDefineListAsync<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele, cancellationToken);
            //List<K> list = await Context.QueryJoinDataListAsync<K>(Mapping, Selector, Models, _query, _order, _distinct, _region, _level, Dele, nodataSetNull, cancellationToken);
            //return list;
        }

        public async override Task<K[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            List<K> list = await ToListAsync(cancellationToken);
            return list.ToArray();
        }

        public async override Task<K> FirstAsync(CancellationToken cancellationToken)
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public async override Task<K> ElementAtAsync(int index, CancellationToken cancellationToken)
        {
            Region region = new Region(index, 1);
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, nodataSetNull, region);
            return await _context.QueryDataDefineSingleAsync<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, Dele, cancellationToken);
            //K target = default(K);
            //Region region = new Region(index, 1);
            //target = await Context.QueryJoinDataSingleAsync<K>(Mapping, Selector, Models, _query, _order, _distinct, region, _level, Dele, nodataSetNull, cancellationToken);
            //return target;
        }

        #endregion
    }
}

