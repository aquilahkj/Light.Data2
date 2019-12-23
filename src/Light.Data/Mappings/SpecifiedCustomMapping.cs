using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Light.Data
{
	internal class SpecifiedCustomMapping : CustomMapping
	{
		#region static

		private static object _synobj = new object ();

		private static Dictionary<Type, SpecifiedCustomMapping> _defaultMapping = new Dictionary<Type, SpecifiedCustomMapping> ();

		public static SpecifiedCustomMapping GetMapping (Type type)
		{
			var mappings = _defaultMapping;
			if (!mappings.TryGetValue(type, out var mapping)) {
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
			var mapping = new SpecifiedCustomMapping (type);
			return mapping;
		}

		#endregion

		protected Dictionary<string, DataFieldMapping> _fieldMappingDictionary = new Dictionary<string, DataFieldMapping> ();

		protected ReadOnlyCollection<DataFieldMapping> _fieldList;

		private SpecifiedCustomMapping (Type type)
			: base (type)
		{
			InitialDataFieldMapping ();
		}

		private void InitialDataFieldMapping ()
		{
			var propertys = ObjectTypeInfo.GetProperties (BindingFlags.Public | BindingFlags.Instance);
			var tmepList = new List<DataFieldMapping> ();
			foreach (var pi in propertys) {
				var mapping = DataFieldMapping.CreateCustomFieldMapping (pi, this);
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
			var item = Activator.CreateInstance (ObjectType);
			var queryState = state as QueryState;
			foreach (var field in _fieldList) {
				if (queryState == null) {
					var obj = datareader[field.Name];
					var value = field.ToProperty(obj);
					if (!Equals(value, null)) {
						field.Handler.Set(item, value);
					}
				}
				else if (queryState.CheckSelectField(field.Name)) {
					var obj = datareader[field.Name];
					var value = field.ToProperty(obj);
					if (!Equals(value, null)) {
						field.Handler.Set(item, value);
					}
				}
			}
			return item;
		}

		public override object LoadAliasJoinTableData (DataContext context, IDataReader datareader, QueryState queryState, string aliasName)
		{
			var item = Activator.CreateInstance (ObjectType);
            var nodataSetNull = queryState != null ? queryState.CheckNoDataSetNull(aliasName) : false;
            var hasData = false;
            foreach (var field in _fieldList) {
				var name = string.Format ("{0}_{1}", aliasName, field.Name);
				if (queryState == null) {
					var obj = datareader [name];
					var value = field.ToProperty (obj);
					if (!Equals (value, null)) {
						field.Handler.Set (item, value);
					}
				}
				else if (queryState.CheckSelectField (name)) {
					var obj = datareader [name];
					var value = field.ToProperty (obj);
					if (!Equals (value, null)) {
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
			var item = Activator.CreateInstance (ObjectType);
			return item;
		}
	}
}

