using System;
using System.Data;

namespace Light.Data
{
    /// <summary>
    /// Single relation field mapping.
    /// </summary>
    internal class SingleRelationFieldMapping : BaseRelationFieldMapping
    {
        public SingleRelationFieldMapping(string fieldName, DataEntityMapping mapping, Type relateType, RelationKey[] keyPairs, PropertyHandler handler)
            : base(fieldName, mapping, relateType, keyPairs, handler)
        {

        }

        public void InitialRelation()
        {
            InitialRelateMapping();
        }

        public object ToProperty(DataContext context, IDataReader dataReader, QueryState queryState, string fieldPath)
        {
            if (!queryState.GetJoinData(fieldPath, out var value)) {
                var aliasName = queryState.GetAliasName(fieldPath);
                foreach (var info in relateInfos) {
                    var name = string.Format("{0}_{1}", aliasName, info.FieldName);
                    if (queryState.CheckSelectField(aliasName)) {
                        var obj = dataReader[name];
                        if (Equals(obj, DBNull.Value) || Equals(obj, null)) {
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
                    //this.relateEntityMapping.LoadJoinTableData(context, dataReader, item, queryState, fieldPath);
                    //value = item;
                    value = relateEntityMapping.LoadJoinTableData(context, dataReader, queryState, fieldPath);
                } else {
                    queryState.SetJoinData(fieldPath, null);
                    return null;
                }

            }
            return value;
        }
    }
}

