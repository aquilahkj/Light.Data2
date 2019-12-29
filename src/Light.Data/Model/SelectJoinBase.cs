using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    internal abstract class SelectJoinBase<K> : ISelectJoin<K>
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

        private readonly LambdaExpression _expression;

        private readonly List<IMap> _maps;

        private ISelector _selector;

        private Delegate _dele;

        public DataContext Context => _context;

        public ISelector Selector {
            get {
                if (_selector == null) {
                    _selector = LambdaExpressionExtend.CreateMultiSelector(_expression, _maps);
                }
                return _selector;
            }
        }

        public Delegate Dele {
            get {
                if (_dele == null) {
                    _dele = _expression.Compile();
                }
                return _dele;
            }
        }

        public List<IJoinModel> Models { get; }

        public MultiDataDynamicMapping Mapping { get; }

        protected SelectJoinBase(DataContext context, LambdaExpression expression, List<IJoinModel> models, List<IMap> maps)
        {
            Models = models;
            _context = context;
            Mapping = MultiDataDynamicMapping.CreateMultiDataDynamicMapping(typeof(K), Models);
            _expression = expression;
            _maps = maps;
        }

        public abstract IEnumerator<K> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract List<K> ToList();

        public abstract K[] ToArray();

        public abstract K First();

        public abstract K ElementAt(int index);

        public abstract Task<List<K>> ToListAsync(CancellationToken cancellationToken = default);

        public abstract Task<K[]> ToArrayAsync(CancellationToken cancellationToken = default);

        public abstract Task<K> FirstAsync(CancellationToken cancellationToken = default);

        public abstract Task<K> ElementAtAsync(int index, CancellationToken cancellationToken = default);
    }
}
