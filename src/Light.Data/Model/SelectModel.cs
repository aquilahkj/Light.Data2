using System.Collections.Generic;

namespace Light.Data
{
	internal class SelectModel
	{
		public SelectModel (DataEntityMapping entityMapping, CustomMapping outputMapping)
		{
			EntityMapping = entityMapping;
			_customOuputMapping = outputMapping;
		}

		private CustomMapping _customOuputMapping;
        
		public DataMapping OutputMapping => _customOuputMapping;

		public IJoinTableMapping JoinTableMapping => _customOuputMapping;

		public DataEntityMapping EntityMapping { get; }

		private readonly Dictionary<string, DataFieldInfo> _selectDict = new Dictionary<string, DataFieldInfo> ();

		public void AddSelectField (string name, DataFieldInfo fieldInfo)
		{
			if (name != fieldInfo.FieldName) {
				var selectInfo = new SpecifiedDataFieldInfo (fieldInfo, name);
				_selectDict.Add (name, selectInfo);
			}
			else {
				_selectDict.Add (name, fieldInfo);
			}
		}

		public virtual DataFieldInfo [] GetDataFieldInfos ()
		{
			var infos = new DataFieldInfo [_selectDict.Count];
			var index = 0;
			foreach (var kvp in _selectDict) {
				infos [index] = kvp.Value;
				index++;
			}
			return infos;
		}

		public DataFieldInfo GetFieldData (string name)
		{
            if (_selectDict.TryGetValue(name, out var info)) {
                return info;
            }
            else {
                return null;
            }
        }

        public bool CheckName (string name)
		{
			return _selectDict.ContainsKey (name);
		}

		public Selector CreateSelector ()
		{
			var selecor = new SpecifiedSelector ();
			foreach (var item in _selectDict.Values) {
				selecor.SetSelectField (item);
			}
			return selecor;
		}
	}
}
