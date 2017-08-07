using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Data
{
    /// <summary>
    /// Collection relation field mapping.
    /// </summary>
    class CollectionRelationFieldMapping : BaseRelationFieldMapping
    {
        static Type LCollectionFrameType;

        static CollectionRelationFieldMapping()
        {
            Type type = typeof(LCollection<object>);
            LCollectionFrameType = type.GetGenericTypeDefinition();
        }

        ConstructorInfo defaultConstructorInfo;

        public CollectionRelationFieldMapping(string fieldName, DataEntityMapping mapping, Type relateType, RelationKey[] keyPairs, PropertyHandler handler)
            : base(fieldName, mapping, relateType, keyPairs, handler)
        {

        }

        string[] fieldPaths;

        protected override void InitialRelateMappingInc()
        {
            base.InitialRelateMappingInc();
            List<string> list = new List<string>();
            SingleRelationFieldMapping[] fields = this.relateEntityMapping.GetSingleRelationFieldMappings();
            foreach (SingleRelationFieldMapping item in fields) {
                if (this.IsReverseMatch(item)) {
                    list.Add("." + item.FieldName);
                }
            }
            fieldPaths = list.ToArray();
            Type objectType = LCollectionFrameType.MakeGenericType(this.relateType);
            TypeInfo objectTypeInfo = objectType.GetTypeInfo();
            ConstructorInfo[] constructorInfoArray = objectTypeInfo.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ConstructorInfo constructorInfo in constructorInfoArray) {
                ParameterInfo[] parameterInfoArray = constructorInfo.GetParameters();
                if (parameterInfoArray.Length == 4) {
                    defaultConstructorInfo = constructorInfo;
                    break;
                }
            }
        }

        public object ToProperty(DataContext context, object source, bool exceptOwner)
        {
            InitialRelateMapping();
            QueryExpression expression = null;
            for (int i = 0; i < masterFieldMappings.Length; i++) {
                DataFieldInfo info = this.relateInfos[i];
                DataFieldMapping field = this.masterFieldMappings[i];
                QueryExpression keyExpression = new LightBinaryQueryExpression(relateEntityMapping, QueryPredicate.Eq, info, field.Handler.Get(source));
                expression = QueryExpression.And(expression, keyExpression);
            }

            object target = null;
            if (defaultConstructorInfo != null) {
                object[] args = { context, source, expression, exceptOwner ? this.fieldPaths : null };
                target = defaultConstructorInfo.Invoke(args);
            }
            return target;
        }
    }
}

