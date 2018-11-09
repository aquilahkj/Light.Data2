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
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, _region);
            return _context.QueryDataDefineReader<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele).GetEnumerator();
        }

        #endregion

        public override List<K> ToList()
        {
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, _region);
            return _context.QueryDataDefineList<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele);

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
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, region);
            return _context.QueryDataDefineSingle<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, Dele);
        }

        #region async

        public async override Task<List<K>> ToListAsync(CancellationToken cancellationToken)
        {
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, _region);
            return await _context.QueryDataDefineListAsync<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele, cancellationToken);
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
            QueryCommand queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, region);
            return await _context.QueryDataDefineSingleAsync<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, Dele, cancellationToken);
        }

        #endregion
    }
}

