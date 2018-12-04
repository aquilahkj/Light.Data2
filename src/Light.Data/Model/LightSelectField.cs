using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    class LightSelectField<K> : SelectFieldBase<K>
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

        public LightSelectField(DataContext context, LambdaExpression expression, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
            : base(context, expression)
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
            QueryCommand queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, _region);
            DataDefine define = DataDefine.GetDefine(typeof(K));
            return _context.QueryDataDefineReader<K>(define, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, null, null).GetEnumerator();
        }

        #endregion

        public override List<K> ToList()
        {
            QueryCommand queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, _region);
            DataDefine define = DataDefine.GetDefine(typeof(K));
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
            Region region = new Region(index, 1);
            QueryCommand queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, region);
            DataDefine define = DataDefine.GetDefine(typeof(K));
            return _context.QueryDataDefineSingle<K>(define, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, null, null);
        }

        #region async

        public async override Task<List<K>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            QueryCommand queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, _region);
            DataDefine define = DataDefine.GetDefine(typeof(K));
            return await _context.QueryDataDefineListAsync<K>(define, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, null, null, cancellationToken);
        }

        public async override Task<K[]> ToArrayAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<K> list = await ToListAsync(CancellationToken.None);
            return list.ToArray();
        }

        public async override Task<K> FirstAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public async override Task<K> ElementAtAsync(int index, CancellationToken cancellationToken = default(CancellationToken))
        {
            Region region = new Region(index, 1);
            QueryCommand queryCommand = _context.Database.QuerySingleField(_context, SpecifiedFieldInfo, _query, _order, false, region);
            DataDefine define = DataDefine.GetDefine(typeof(K));
            return await _context.QueryDataDefineSingleAsync<K>(define, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, null, null, cancellationToken);
        }

        #endregion
    }
}
