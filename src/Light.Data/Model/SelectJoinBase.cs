using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    abstract class SelectJoinBase<K> : ISelectJoin<K>
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

        LambdaExpression _expression;

        List<IMap> _maps;

        ISelector _selector;

        List<IJoinModel> _models;

        Delegate _dele;

        public DataContext Context {
            get {
                return _context;
            }
        }

        public ISelector Selector {
            get {
                if (_selector == null) {
                    _selector = LambdaExpressionExtend.CreateMutliSelector(_expression, _maps);
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

        public List<IJoinModel> Models {
            get {
                return _models;
            }
        }

        readonly DynamicMultiDataMapping _mapping;

        public DynamicMultiDataMapping Mapping {
            get {
                return _mapping;
            }
        }

        protected SelectJoinBase(DataContext context, LambdaExpression expression, List<IJoinModel> models, List<IMap> maps)
        {
            _models = models;
            _context = context;
            _mapping = DynamicMultiDataMapping.CreateDynamicMultiDataMapping(typeof(K), _models);
            _expression = expression;
            _maps = maps;
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

        public abstract Task<List<K>> ToListAsync(CancellationToken cancellationToken);

        public abstract Task<K[]> ToArrayAsync(CancellationToken cancellationToken);

        public abstract Task<K> FirstAsync(CancellationToken cancellationToken);

        public abstract Task<K> ElementAtAsync(int index, CancellationToken cancellationToken);
    }
}
