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
        readonly protected DataContextOptions<TContxt> options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public DataContextFactory(DataContextOptions<TContxt> options)
        {
            this.options = options;
        }
        /// <summary>
        /// Create Specified DataContext
        /// </summary>
        /// <returns></returns>
        public abstract TContxt CreateDataContext();
    }
    /// <summary>
    /// Basic DataContext Factory 
    /// </summary>
    public class LightDataContextFactory : DataContextFactory<DataContext>
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
        public override DataContext CreateDataContext()
        {
            return new DataContext(options);
        }
    }
}
