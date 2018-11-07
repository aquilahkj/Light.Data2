using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Light.Data
{
    class QueryCommands
    {
        public List<DbCommand> Commands {
            get;
            set;
        }

        public QueryState State {
            get;
            set;
        }
    }
}
