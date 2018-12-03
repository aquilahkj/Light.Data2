using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    abstract class SelectFieldBase<K> : ISelectField<K>
    {
        public abstract QueryExpression QueryExpression {
            get;
        }

        public abstract OrderExpression OrderExpression {
            get;
        }

        public abstract bool Distinct {
            get;
        }

        public abstract Region Region {
            get;
        }

        public abstract SafeLevel Level {
            get;
        }

        protected readonly DataContext _context;

        public DataContext Context {
            get {
                return _context;
            }
        }

        LambdaExpression _expression;

        DataFieldInfo _specifiedFieldInfo;

        public DataFieldInfo SpecifiedFieldInfo {
            get {
                if (_specifiedFieldInfo == null) {
                    _specifiedFieldInfo = LambdaExpressionExtend.ResolveSingleField(_expression);
                }
                return _specifiedFieldInfo;
            }
        }

        protected SelectFieldBase(DataContext context, LambdaExpression expression)
        {
            _context = context;
            _expression = expression;
        }

        public abstract IEnumerator<K> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public abstract List<K> ToList();

        public abstract K[] ToArray();

        public abstract K First();

        public abstract K ElementAt(int index);

        public abstract Task<List<K>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task<K[]> ToArrayAsync(CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task<K> FirstAsync(CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task<K> ElementAtAsync(int index, CancellationToken cancellationToken = default(CancellationToken));
    }
}
