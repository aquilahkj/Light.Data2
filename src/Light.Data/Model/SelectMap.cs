using System;
namespace Light.Data
{
	class SelectMap : IMap
	{
		readonly SelectModel _model;

		public SelectMap (SelectModel model)
		{
			this._model = model;
		}

		public Type Type {
			get {
				return _model.OutputMapping.ObjectType;
			}
		}

		public bool CheckIsEntityCollection (string path)
		{
			return false;
		}

		public bool CheckIsField (string path)
		{
			string name;
			if (path.StartsWith (".", StringComparison.Ordinal)) {
				name = path.Substring (1);
			}
			else {
				name = path;
			}
			return _model.CheckName (name);
		}

		public bool CheckIsRelateEntity (string path)
		{
			return false;
		}

		public DataFieldInfo GetFieldInfoForPath (string path)
		{
			string name;
			if (path.StartsWith (".", StringComparison.Ordinal)) {
				name = path.Substring (1);
			}
			else {
				name = path;
			}
			DataFieldInfo info = _model.GetFieldData (name);
			if (!Object.Equals (info, null)) {
				return info;
			}
			else {
				throw new LightDataException (string.Format (SR.CanNotFindTheSpecifiedFieldViaPath, path));
			}
		}

		public ISelector CreateSelector (string [] paths)
		{
			Selector selector = new Selector ();
			foreach (string path in paths) {
				string name;
				if (path.StartsWith (".", StringComparison.Ordinal)) {
					name = path.Substring (1);
				}
				else {
					name = path;
				}
				if (name == string.Empty) {
					DataFieldInfo [] nameInfos = _model.GetDataFieldInfos ();
					foreach (DataFieldInfo fieldInfo in nameInfos) {
						selector.SetSelectField (fieldInfo);
					}
				}
				else {
					DataFieldInfo info = _model.GetFieldData (name);
					if (!Object.Equals (info, null)) {
						DataFieldInfo nameInfo = new DataFieldInfo (info.TableMapping, false, name);
						selector.SetSelectField (nameInfo);
					}
					else {
						throw new LightDataException (string.Format (SR.CanNotFindTheSpecifiedFieldViaPath, path));
					}
				}
			}
			return selector;
		}
	}
}
