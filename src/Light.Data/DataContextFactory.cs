using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    public abstract class DataContextFactory<TContxt> where TContxt : DataContext
    {
        readonly protected DataContextOptions<TContxt> options;

        public DataContextFactory(DataContextOptions<TContxt> options)
        {
            this.options = options;
        }

        public abstract TContxt CreateDataContext();
    }

    public class LightDataContextFactory : DataContextFactory<DataContext>
    {
        public LightDataContextFactory(DataContextOptions<DataContext> options) : base(options)
        {
        }

        public override DataContext CreateDataContext()
        {
            return new DataContext(options);
        }
    }
}
