using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Light.Data
{
	class SpecifiedCustomMapping : CustomMapping
	{
		#region static

		static object _synobj = new object ();

		static Dictionary<Type, SpecifiedCustomMapping> _defaultMapping = new Dictionary<Type, SpecifiedCustomMapping> ();

		public static SpecifiedCustomMapping GetMapping (Type type)
		{
			Dictionary<Type, SpecifiedCustomMapping> mappings = _defaultMapping;
			if (!mappings.TryGetValue(type, out SpecifiedCustomMapping mapping)) {
				lock (_synobj) {
					if (!mappings.ContainsKey(type)) {
						mapping = CreateMapping(type);
						mappings[type] = mapping;
					}
				}
			}
			return mapping;
		}

		private static SpecifiedCustomMapping CreateMapping (Type type)
		{
			SpecifiedCustomMapping mapping = new SpecifiedCustomMapping (type);
			return mapping;
		}

		#endregion

		protected Dictionary<string, DataFieldMapping> _fieldMappingDictionary = new Dictionary<string, DataFieldMapping> ();

		protected ReadOnlyCollection<DataFieldMapping> _fieldList;

		SpecifiedCustomMapping (Type type)
			: base (type)
		{
			InitialDataFieldMapping ();
		}

		private void InitialDataFieldMapping ()
		{
			PropertyInfo [] propertys = ObjectTypeInfo.GetProperties (BindingFlags.Public | BindingFlags.Instance);
			List<DataFieldMapping> tmepList = new List<DataFieldMapping> ();
			foreach (PropertyInfo pi in propertys) {
				DataFieldMapping mapping = DataFieldMapping.CreateCustomFieldMapping (pi, this);
				if (mapping != null) {
					_fieldMappingDictionary.Add (mapping.IndexName, mapping);
					tmepList.Add (mapping);
				}
			}
			if (tmepList.Count == 0) {
				throw new LightDataException(string.Format(SR.NoMappingField, ObjectType.FullName));
			}
			_fieldList = new ReadOnlyCollection<DataFieldMapping> (tmepList);
		}

		public override object LoadData (DataContext context, IDataReader datareader, object state)
		{
			object item = Activator.CreateInstance (ObjectType);
			QueryState queryState = state as QueryState;
			foreach (DataFieldMapping field in this._fieldList) {
				if (queryState == null) {
					object obj = datareader[field.Name];
					object value = field.ToProperty(obj);
					if (!Object.Equals(value, null)) {
						field.Handler.Set(item, value);
					}
				}
				else if (queryState.CheckSelectField(field.Name)) {
					object obj = datareader[field.Name];
					object value = field.ToProperty(obj);
					if (!Object.Equals(value, null)) {
						field.Handler.Set(item, value);
					}
				}
			}
			return item;
		}

		public override object CreateJoinTableData (DataContext context, IDataReader datareader, QueryState queryState, string aliasName)
		{
			object item = Activator.CreateInstance (ObjectType);
            bool nodataSetNull = queryState != null ? queryState.CheckNoDataSetNull(aliasName) : false;
            bool hasData = false;
            foreach (DataFieldMapping field in this._fieldList) {
				string name = string.Format ("{0}_{1}", aliasName, field.Name);
				if (queryState == null) {
					object obj = datareader [name];
					object value = field.ToProperty (obj);
					if (!Object.Equals (value, null)) {
						field.Handler.Set (item, value);
					}
				}
				else if (queryState.CheckSelectField (name)) {
					object obj = datareader [name];
					object value = field.ToProperty (obj);
					if (!Object.Equals (value, null)) {
						field.Handler.Set (item, value);
                        hasData = true;
                    }
				}
			}
            if (!hasData && nodataSetNull) {
                return null;
            }
            return item;
		}

		public override object InitialData ()
		{
			object item = Activator.CreateInstance (ObjectType);
			return item;
		}
	}
}

