using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    internal class LightSelectField<K> : SelectFieldBase<K>
    {
        private readonly QueryExpression _query;

        private readonly OrderExpression _order;

        private readonly Region _region;

        private readonly SafeLevel _level;

        public override QueryExpression QueryExpression => _query;

        public override OrderExpression OrderExpression => _order;

        public override bool Distinct { get; }

        public override Region Region => _region;

        public override SafeLevel Level => _level;

        public LightSelectField(DataContext context, LambdaExpression expression, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
            : base(context, expression)
        {
            _query = query;
            _order = order;
            Distinct = distinct;
            _region = region;
            _level = level;
        }

        #region IEnumerable implementation

        public override IEnumerator<K> GetEnumerator()
        {
            var queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, _region);
            var define = DataDefine.GetDefine(typeof(K));
            return _context.QueryDataDefineReader<K>(define, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, null, null).GetEnumerator();
        }

        #endregion

        public override List<K> ToList()
        {
            var queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, _region);
            var define = DataDefine.GetDefine(typeof(K));
            return _context.QueryDataDefineList<K>(define, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, null, null);
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
            var queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, region);
            var define = DataDefine.GetDefine(typeof(K));
            return _context.QueryDataDefineSingle<K>(define, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, null, null);
        }

        #region async

        public override async Task<List<K>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, _region);
            var define = DataDefine.GetDefine(typeof(K));
            return await _context.QueryDataDefineListAsync<K>(define, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, null, null, cancellationToken);
        }

        public override async Task<K[]> ToArrayAsync(CancellationToken cancellationToken = default)
        {
            var list = await ToListAsync(CancellationToken.None);
            return list.ToArray();
        }

        public override async Task<K> FirstAsync(CancellationToken cancellationToken = default)
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public override async Task<K> ElementAtAsync(int index, CancellationToken cancellationToken = default)
        {
            var region = new Region(index, 1);
            var queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, region);
            var define = DataDefine.GetDefine(typeof(K));
            return await _context.QueryDataDefineSingleAsync<K>(define, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, null, null, cancellationToken);
        }

        #endregion
    }
}
