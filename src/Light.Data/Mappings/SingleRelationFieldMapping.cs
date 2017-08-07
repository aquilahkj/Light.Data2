using System;
using System.Data;

namespace Light.Data
{
	/// <summary>
	/// Single relation field mapping.
	/// </summary>
	class SingleRelationFieldMapping : BaseRelationFieldMapping
	{
		public SingleRelationFieldMapping (string fieldName, DataEntityMapping mapping, Type relateType, RelationKey [] keyPairs, PropertyHandler handler)
			: base (fieldName, mapping, relateType, keyPairs, handler)
		{

		}

		public void InitialRelation ()
		{
			InitialRelateMapping ();
		}

		public object ToProperty (DataContext context, IDataReader datareader, QueryState datas, string fieldPath)
		{
            if (!datas.GetJoinData(fieldPath, out object value)) {
                string aliasName = datas.GetAliasName(fieldPath);
                foreach (DataFieldInfo info in this.relateInfos) {
                    string name = string.Format("{0}_{1}", aliasName, info.FieldName);
                    if (datas.CheckSelectField(aliasName)) {
                        object obj = datareader[name];
                        if (Object.Equals(obj, DBNull.Value) || Object.Equals(obj, null)) {
                            datas.SetJoinData(fieldPath, null);
                            return null;
                        }
                    }
                    else {
                        datas.SetJoinData(fieldPath, null);
                        return null;
                    }
                }
                if (datas.CheckSelectField(aliasName)) {
                    object item = Activator.CreateInstance(this.RelateMapping.ObjectType);
                    datas.SetJoinData(fieldPath, item);
                    this.relateEntityMapping.LoadJoinTableData(context, datareader, item, datas, fieldPath);
                    value = item;
                }
                else {
                    datas.SetJoinData(fieldPath, null);
                    return null;
                }

            }
            return value;
		}
	}
}

