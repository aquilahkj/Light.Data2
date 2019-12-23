using System.Data.Common;

namespace Light.Data
{
    internal class QueryCommand
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
