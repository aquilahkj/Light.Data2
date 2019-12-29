using System;

namespace Light.Data
{
	internal class AggregateMap : IMap
	{
		private readonly AggregateModel _model;

		public AggregateMap (AggregateModel model)
		{
			_model = model;
		}

		public Type Type => _model.OutputDataMapping.ObjectType;

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
			var info = _model.GetAggregateData (name);
			if (!Equals (info, null)) {
				return info;
			}

			throw new LightDataException (string.Format (SR.CanNotFindTheSpecifiedFieldViaPath, path));
		}

		public ISelector CreateSelector (string [] paths)
		{
			var selector = new Selector ();
			foreach (var path in paths) {
				string name;
				if (path.StartsWith (".", StringComparison.Ordinal)) {
					name = path.Substring (1);
				}
				else {
					name = path;
				}
				if (name == string.Empty) {
					DataFieldInfo [] nameInfos = _model.GetAggregateDataFieldInfos();
					foreach (var fieldInfo in nameInfos) {
						selector.SetSelectField (fieldInfo);
					}
				}
				else {
					var nameInfo = _model.GetAggregateData (name);
					if (!Equals (nameInfo, null)) {
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

