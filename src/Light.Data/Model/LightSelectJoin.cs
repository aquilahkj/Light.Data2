using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    internal class LightSelectJoin<K> : SelectJoinBase<K>
    {
        private readonly QueryExpression _query;

        private readonly OrderExpression _order;

        private readonly bool _distinct;

        private readonly Region _region;

        private readonly SafeLevel _level;
        
        public override QueryExpression QueryExpression => _query;

        public override OrderExpression OrderExpression => _order;

        public override bool Distinct => _distinct;

        public override Region Region => _region;

        public override SafeLevel Level => _level;

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
            var queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, _region);
            return _context.QueryDataDefineReader<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele).GetEnumerator();
        }

        #endregion

        public override List<K> ToList()
        {
            var queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, _region);
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
            var region = new Region(index, 1);
            var queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, region);
            return _context.QueryDataDefineSingle<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, Dele);
        }

        #region async

        public override async Task<List<K>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, _region);
            return await _context.QueryDataDefineListAsync<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele, cancellationToken);
        }

        public override async Task<K[]> ToArrayAsync(CancellationToken cancellationToken = default)
        {
            var list = await ToListAsync(cancellationToken);
            return list.ToArray();
        }

        public override async Task<K> FirstAsync(CancellationToken cancellationToken = default)
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public override async Task<K> ElementAtAsync(int index, CancellationToken cancellationToken = default)
        {
            var region = new Region(index, 1);
            var queryCommand = _context.Database.QueryJoinData(_context, Mapping, Selector, Models, _query, _order, _distinct, region);
            return await _context.QueryDataDefineSingleAsync<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, Dele, cancellationToken);
        }

        #endregion
    }
}

