using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Data
{
    /// <summary>
    /// Collection relation field mapping.
    /// </summary>
    internal class CollectionRelationFieldMapping : BaseRelationFieldMapping
    {
        private static Type LCollectionFrameType;

        static CollectionRelationFieldMapping()
        {
            var type = typeof(LCollection<object>);
            LCollectionFrameType = type.GetGenericTypeDefinition();
        }

        private ConstructorInfo defaultConstructorInfo;

        public CollectionRelationFieldMapping(string fieldName, DataEntityMapping mapping, Type relateType, RelationKey[] keyPairs, PropertyHandler handler)
            : base(fieldName, mapping, relateType, keyPairs, handler)
        {

        }

        private string[] fieldPaths;

        protected override void InitialRelateMappingInc()
        {
            base.InitialRelateMappingInc();
            var list = new List<string>();
            var fields = relateEntityMapping.GetSingleRelationFieldMappings();
            foreach (var item in fields) {
                if (IsReverseMatch(item)) {
                    list.Add("." + item.FieldName);
                }
            }
            fieldPaths = list.ToArray();
            var objectType = LCollectionFrameType.MakeGenericType(relateType);
            var objectTypeInfo = objectType.GetTypeInfo();
            var constructorInfoArray = objectTypeInfo.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var constructorInfo in constructorInfoArray) {
                var parameterInfoArray = constructorInfo.GetParameters();
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
            for (var i = 0; i < masterFieldMappings.Length; i++) {
                var info = relateInfos[i];
                var field = masterFieldMappings[i];
                QueryExpression keyExpression = new LightBinaryQueryExpression(relateEntityMapping, QueryPredicate.Eq, info, field.Handler.Get(source));
                expression = QueryExpression.And(expression, keyExpression);
            }

            object target = null;
            if (defaultConstructorInfo != null) {
                object[] args = { context, source, expression, exceptOwner ? fieldPaths : null };
                target = defaultConstructorInfo.Invoke(args);
            }
            return target;
        }
    }
}

