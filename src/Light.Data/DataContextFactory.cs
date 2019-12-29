namespace Light.Data
{
    /// <summary>
    /// Abstract DataContext Factory
    /// </summary>
    /// <typeparam name="TContxt"></typeparam>
    public abstract class DataContextFactory<TContxt> where TContxt : DataContext
    {
        /// <summary>
        /// Options
        /// </summary>
        protected readonly DataContextOptions<TContxt> options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        protected DataContextFactory(DataContextOptions<TContxt> options)
        {
            this.options = options;
        }
    }
    /// <summary>
    /// Basic DataContext Factory 
    /// </summary>
    public sealed class LightDataContextFactory : DataContextFactory<DataContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public LightDataContextFactory(DataContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// Create Basic DataContext
        /// </summary>
        /// <returns></returns>
        public DataContext CreateDataContext()
        {
            return new DataContext(options);
        }
    }
}
