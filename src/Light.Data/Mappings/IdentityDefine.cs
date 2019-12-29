using System.Data;

namespace Light.Data
{
    internal class IdentityDefine : IDataDefine
    {
        public object LoadData(DataContext context, IDataReader dataReader, object state)
        {
            return dataReader[0];
        }
    }
}
