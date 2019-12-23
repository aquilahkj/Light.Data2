using System.Data;

namespace Light.Data
{
	internal interface IDataDefine
	{
		object LoadData (DataContext context, IDataReader datareader, object state);
	}
}
