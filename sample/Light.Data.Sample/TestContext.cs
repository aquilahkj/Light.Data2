using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data.Sample
{
    public class TestContext : DataContext
    {
        public TestContext(DataContextOptions<TestContext> options) : base(options)
        {

        }
    }
}
