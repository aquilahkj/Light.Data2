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
            return _context.QuerySingleFieldReader<K>(SpecifiedFieldInfo, typeof(K), _query, _order, _distinct, _region).GetEnumerator();
        }

        #endregion

        public override List<K> ToList()
        {
            List<K> list = _context.QuerySingleFieldList<K>(SpecifiedFieldInfo, typeof(K), _query, _order, _distinct, _region);
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
            target = _context.QuerySingleFieldSingle<K>(SpecifiedFieldInfo, typeof(K), _query, _order, false, region);
            return target;
        }

        #region async

        public async override Task<List<K>> ToListAsync(CancellationToken cancellationToken)
        {
            List<K> list = await _context.QuerySingleFieldListAsync<K>(SpecifiedFieldInfo, typeof(K), _query, _order, _distinct, _region, cancellationToken);
            return list;
        }

        public async override Task<List<K>> ToListAsync()
        {
            return await ToListAsync(CancellationToken.None);
        }

        public async override Task<K[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            List<K> list = await ToListAsync();
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
            target = await _context.QuerySingleFieldSingleAsync<K>(SpecifiedFieldInfo, typeof(K), _query, _order, false, region, cancellationToken);
            return target;
        }

        public async override Task<K> ElementAtAsync(int index)
        {
            return await ElementAtAsync(index, CancellationToken.None);
        }

        #endregion
    }
}
