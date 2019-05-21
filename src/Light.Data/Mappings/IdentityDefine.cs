using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data
{
    class IdentityDefine : IDataDefine
    {
        public object LoadData(DataContext context, IDataReader datareader, object state)
        {
            return datareader[0];
        }
    }
}
