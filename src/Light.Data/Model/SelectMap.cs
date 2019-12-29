using System;

namespace Light.Data
{
	internal class SelectMap : IMap
	{
		private readonly SelectModel _model;

		public SelectMap (SelectModel model)
		{
			_model = model;
		}

		public Type Type => _model.OutputMapping.ObjectType;

		public bool CheckIsEntityCollection (string path)
		{
			return false;
		}

		public bool CheckIsField (string path)
		{
			var name = path.StartsWith (".", StringComparison.Ordinal) ? path.Substring (1) : path;
			return _model.CheckName (name);
		}

		public bool CheckIsRelateEntity (string path)
		{
			return false;
		}

		public DataFieldInfo GetFieldInfoForPath (string path)
		{
			var name = path.StartsWith (".", StringComparison.Ordinal) ? path.Substring (1) : path;
			var info = _model.GetFieldData (name);
			if (!Equals (info, null)) {
				return info;
			}

			throw new LightDataException (string.Format (SR.CanNotFindTheSpecifiedFieldViaPath, path));
		}

		public ISelector CreateSelector (string [] paths)
		{
			var selector = new Selector ();
			foreach (var path in paths)
			{
				var name = path.StartsWith (".", StringComparison.Ordinal) ? path.Substring (1) : path;
				if (name == string.Empty) {
					var nameInfos = _model.GetDataFieldInfos ();
					foreach (var fieldInfo in nameInfos) {
						selector.SetSelectField (fieldInfo);
					}
				}
				else {
					var info = _model.GetFieldData (name);
					if (!Equals (info, null)) {
						var nameInfo = new DataFieldInfo (info.TableMapping, false, name);
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
