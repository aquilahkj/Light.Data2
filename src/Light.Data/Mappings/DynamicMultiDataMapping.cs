using System;
using System.Collections.Generic;
using System.Data;

namespace Light.Data
{
	class DynamicMultiDataMapping : DataMapping
	{
		public static DynamicMultiDataMapping CreateDynamicMultiDataMapping (Type type, List<IJoinModel> models)
		{
			Tuple<string, IJoinTableMapping> [] array = new Tuple<string, IJoinTableMapping> [models.Count];
			for (int i = 0; i < models.Count; i++) {
				IJoinModel model = models [i];
				Tuple<string, IJoinTableMapping> tuple = new Tuple<string, IJoinTableMapping> (model.AliasTableName, model.JoinMapping);
				array [i] = tuple;
			}
			DynamicMultiDataMapping mapping = new DynamicMultiDataMapping (type, array);
			return mapping;
		}

		readonly IJoinTableMapping [] mappings;
		readonly string [] aliasNames;

		public DynamicMultiDataMapping (Type type, Tuple<string, IJoinTableMapping> [] targetMappings)
			: base (type)
		{
			mappings = new IJoinTableMapping [targetMappings.Length];
			aliasNames = new string [targetMappings.Length];
			for (int i = 0; i < targetMappings.Length; i++) {
				aliasNames [i] = targetMappings [i].Item1;
				mappings [i] = targetMappings [i].Item2;
			}
		}

		public override object InitialData ()
		{
			object [] objects = new object [mappings.Length];
			for (int i = 0; i < mappings.Length; i++) {
				objects [i] = mappings [i].InitialData ();
			}
			return objects;
		}

		public override object LoadData (DataContext context, IDataReader datareader, object state)
		{
			QueryState queryState = state as QueryState;
			object [] objects = new object [mappings.Length];
			for (int i = 0; i < mappings.Length; i++) {
				string aliasName = aliasNames [i];
				objects [i] = mappings [i].CreateJoinTableData (context, datareader, queryState, aliasName);
			}
			return objects;
		}
	}
}

