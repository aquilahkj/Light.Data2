using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Sample
{
    public class TestExtendContext : DataContext
    {
        public TestExtendContext(DataContextOptions<TestContext> options) : base(options)
        {

        }
    }
}
