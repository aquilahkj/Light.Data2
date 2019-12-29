using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Light.Data
{
	internal class SpecifiedDataMapping : CustomDataMapping
	{
		#region static

		private static readonly object locker = new object ();

		private static readonly Dictionary<Type, SpecifiedDataMapping> _defaultMapping = new Dictionary<Type, SpecifiedDataMapping> ();

		public static SpecifiedDataMapping GetMapping (Type type)
		{
			var mappings = _defaultMapping;
			if (!mappings.TryGetValue(type, out var mapping)) {
				lock (locker) {
					if (!mappings.TryGetValue(type, out mapping)) {
						mapping = CreateMapping(type);
						mappings[type] = mapping;
					}
				}
			}
			return mapping;
		}

		private static SpecifiedDataMapping CreateMapping (Type type)
		{
			var mapping = new SpecifiedDataMapping (type);
			return mapping;
		}

		#endregion

		// protected readonly Dictionary<string, DataFieldMapping> _fieldMappingDictionary = new Dictionary<string, DataFieldMapping> ();

		protected ReadOnlyCollection<DataFieldMapping> _fieldList;

		private SpecifiedDataMapping (Type type)
			: base (type)
		{
			InitialDataFieldMapping ();
		}

		private void InitialDataFieldMapping ()
		{
			var properties = ObjectTypeInfo.GetProperties (BindingFlags.Public | BindingFlags.Instance);
			var tempList = new List<DataFieldMapping> ();
			foreach (var pi in properties) {
				var mapping = DataFieldMapping.CreateCustomFieldMapping (pi, this);
				tempList.Add (mapping);
				// if (mapping != null) {
				// 	tempList.Add (mapping);
				// }
			}
			if (tempList.Count == 0) {
				throw new LightDataException(string.Format(SR.NoMappingField, ObjectType.FullName));
			}
			_fieldList = new ReadOnlyCollection<DataFieldMapping> (tempList);
		}

		public override object LoadData (DataContext context, IDataReader dataReader, object state)
		{
			var item = Activator.CreateInstance (ObjectType);
			var queryState = state as QueryState;
			foreach (var field in _fieldList) {
				if (queryState == null) {
					var obj = dataReader[field.Name];
					var value = field.ToProperty(obj);
					if (!Equals(value, null)) {
						field.Handler.Set(item, value);
					}
				}
				else if (queryState.CheckSelectField(field.Name)) {
					var obj = dataReader[field.Name];
					var value = field.ToProperty(obj);
					if (!Equals(value, null)) {
						field.Handler.Set(item, value);
					}
				}
			}
			return item;
		}

		public override object LoadAliasJoinTableData (DataContext context, IDataReader dataReader, QueryState queryState, string aliasName)
		{
			var item = Activator.CreateInstance (ObjectType);
            var nodataSetNull = queryState?.CheckNoDataSetNull(aliasName) ?? false;
            var hasData = false;
            foreach (var field in _fieldList) {
				var name = string.Format ("{0}_{1}", aliasName, field.Name);
				if (queryState == null) {
					var obj = dataReader [name];
					var value = field.ToProperty (obj);
					if (!Equals (value, null)) {
						field.Handler.Set (item, value);
					}
				}
				else if (queryState.CheckSelectField (name)) {
					var obj = dataReader [name];
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

