using System.Data;

namespace Light.Data
{
    internal class IdentityDefine : IDataDefine
    {
        public object LoadData(DataContext context, IDataReader datareader, object state)
        {
            return datareader[0];
        }
    }
}
