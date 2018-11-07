using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    abstract class SelectBase<K> : ISelect<K>
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

        ISelector _selector;

        LambdaExpression _expression;

        readonly DataEntityMapping _mapping;

        SelectModel _model;

        Delegate _dele;

        public DataEntityMapping Mapping {
            get {
                return _mapping;
            }
        }

        public SelectModel Model {
            get {
                if (_model == null) {
                    _model = LambdaExpressionExtend.CreateSelectModel(_expression);
                }
                return _model;
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

        public ISelector Selector {
            get {
                if (_selector == null) {
                    _selector = LambdaExpressionExtend.CreateSelector(_expression);
                }
                return _selector;
            }
        }


        protected SelectBase(DataContext context, LambdaExpression expression, Type type)
        {
            _context = context;
            _expression = expression;
            _mapping = DataEntityMapping.GetEntityMapping(type);
        }

        public abstract K First();

        public abstract K ElementAt(int index);

        public abstract IEnumerator<K> GetEnumerator();

        public abstract List<K> ToList();

        public abstract K[] ToArray();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public abstract IJoinTable<K, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> Join<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> Join<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression);

        public abstract Task<List<K>> ToListAsync(CancellationToken cancellationToken);

        public abstract Task<K[]> ToArrayAsync(CancellationToken cancellationToken);

        public abstract Task<K> FirstAsync(CancellationToken cancellationToken);

        public abstract Task<K> ElementAtAsync(int index, CancellationToken cancellationToken);
    }
}
