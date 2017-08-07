using System;
namespace Light.Data
{
	interface IMap
	{
		Type Type {
			get;
		}

		bool CheckIsField (string path);

		bool CheckIsRelateEntity (string path);

		bool CheckIsEntityCollection (string path);

		DataFieldInfo GetFieldInfoForPath (string path);

		ISelector CreateSelector (string [] paths);


	}
}

