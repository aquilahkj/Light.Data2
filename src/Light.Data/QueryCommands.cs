using System.Collections.Generic;
using System.Data.Common;

namespace Light.Data
{
    internal class QueryCommands
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
