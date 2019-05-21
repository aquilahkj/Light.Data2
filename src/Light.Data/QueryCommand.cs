using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Light.Data
{
    class QueryCommand
    {
        public DbCommand Command {
            get;
            set;
        }

        public bool InnerPage {
            get;
            set;
        }

        public QueryState State {
            get;
            set;
        }

        public bool IdentitySql {
            get;
            set;
        }
    }
}
