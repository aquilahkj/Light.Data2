using System;
using System.Data;

namespace Light.Data
{
    /// <summary>
    /// Single relation field mapping.
    /// </summary>
    class SingleRelationFieldMapping : BaseRelationFieldMapping
    {
        public SingleRelationFieldMapping(string fieldName, DataEntityMapping mapping, Type relateType, RelationKey[] keyPairs, PropertyHandler handler)
            : base(fieldName, mapping, relateType, keyPairs, handler)
        {

        }

        public void InitialRelation()
        {
            InitialRelateMapping();
        }

        public object ToProperty(DataContext context, IDataReader datareader, QueryState queryState, string fieldPath)
        {
            if (!queryState.GetJoinData(fieldPath, out object value)) {
                string aliasName = queryState.GetAliasName(fieldPath);
                foreach (DataFieldInfo info in this.relateInfos) {
                    string name = string.Format("{0}_{1}", aliasName, info.FieldName);
                    if (queryState.CheckSelectField(aliasName)) {
                        object obj = datareader[name];
                        if (Object.Equals(obj, DBNull.Value) || Object.Equals(obj, null)) {
                            queryState.SetJoinData(fieldPath, null);
                            return null;
                        }
                    } else {
                        queryState.SetJoinData(fieldPath, null);
                        return null;
                    }
                }
                if (queryState.CheckSelectField(aliasName)) {
                    //object item = Activator.CreateInstance(this.RelateMapping.ObjectType);
                    //queryState.SetJoinData(fieldPath, item);
                    //this.relateEntityMapping.LoadJoinTableData(context, datareader, item, queryState, fieldPath);
                    //value = item;
                    value = this.relateEntityMapping.LoadJoinTableData(context, datareader, queryState, fieldPath);
                } else {
                    queryState.SetJoinData(fieldPath, null);
                    return null;
                }

            }
            return value;
        }
    }
}

