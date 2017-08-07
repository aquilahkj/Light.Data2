using System.Data;

namespace Light.Data
{
	interface IDataDefine
	{
		object LoadData (DataContext context, IDataReader datareader, object state);
	}
}
