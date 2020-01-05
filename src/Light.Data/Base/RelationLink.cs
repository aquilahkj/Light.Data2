using System;
using System.Collections.Generic;

namespace Light.Data
{
    internal enum RelationLinkType
    {
        NoMatch,
        AddLink,
        Cycle
    }

    internal class RelationLink
    {
        public string LastFieldPath { get; private set; }

        public RelationItem CycleItem { get; private set; }

        private List<RelationItem> items = new List<RelationItem>();

        private int keyCount;

        public RelationItem LastItem => items[items.Count - 1];

        public string CycleFieldPath { get; private set; }

        public string PrevFieldPath { get; private set; }

        public int Count => items.Count;

        private RelationLink()
        {

        }

        public RelationLink(SingleRelationFieldMapping rootRelationMapping, string fieldPath)
        {
            if (rootRelationMapping == null)
                throw new ArgumentNullException(nameof(rootRelationMapping));
            //this.rootRelationMapping = rootRelationMapping;
            var keys = rootRelationMapping.GetKeyPairs();
            keyCount = keys.Length;
            var masters = new string[keyCount];
            var relates = new string[keyCount];
            for (var i = 0; i < keyCount; i++) {
                masters[i] = keys[i].MasterKey;
                relates[i] = keys[i].RelateKey;
            }
            LastFieldPath = $"{fieldPath}.{rootRelationMapping.FieldName}";
            PrevFieldPath = fieldPath;
            var masterItem = new RelationItem
            {
                DataMapping = rootRelationMapping.MasterMapping,
                FieldMapping = null,
                //PrevFieldPath = prevFieldPath,
                CurrentFieldPath = PrevFieldPath,
                Keys = masters
            };

            var relateItem = new RelationItem
            {
                DataMapping = rootRelationMapping.RelateMapping,
                FieldMapping = rootRelationMapping,
                PrevFieldPath = PrevFieldPath,
                CurrentFieldPath = LastFieldPath,
                Keys = relates
            };

            items.Add(masterItem);
            items.Add(relateItem);
        }

        private bool IsMatch(string[] array1, string[] array2)
        {
            foreach (var t1 in array1)
            {
                var flag = false;
                foreach (var t in array2)
                {
                    if (t1 == t) {
                        flag = true;
                        break;
                    }
                }
                if (!flag) {
                    return false;
                }
            }

            return true;
        }

        public RelationLinkType TryAddField(SingleRelationFieldMapping relateMapping)
        {
            var last = items[items.Count - 1];
            if (!Equals(relateMapping.MasterMapping, last.DataMapping)) {
                throw new LightDataException(string.Format(SR.RelationFieldError, relateMapping.MasterMapping.ObjectType, relateMapping.FieldName));
            }
            var keys = relateMapping.GetKeyPairs();
            if (keys.Length != keyCount) {
                return RelationLinkType.NoMatch;
            }
            var masters = new string[keyCount];
            var relates = new string[keyCount];
            for (var i = 0; i < keyCount; i++) {
                masters[i] = keys[i].MasterKey;
                relates[i] = keys[i].RelateKey;
            }
            if (!IsMatch(last.Keys, masters)) {
                return RelationLinkType.NoMatch;
            }
            PrevFieldPath = LastFieldPath;
            LastFieldPath = $"{last.CurrentFieldPath}.{relateMapping.FieldName}";
            var len = items.Count - 1;
            for (var i = 0; i < len; i++) {
                var item = items[i];
                if (Equals(relateMapping.RelateMapping, item.DataMapping))
                {
                    if (IsMatch(item.Keys, relates)) {
                        CycleFieldPath = item.CurrentFieldPath;
                        return RelationLinkType.Cycle;
                    }

                    throw new LightDataException(string.Format(SR.RelationFieldKeyNotMatch, relateMapping.MasterMapping.ObjectType, relateMapping.FieldName));
                }
            }
            var relateItem = new RelationItem
            {
                DataMapping = relateMapping.RelateMapping,
                FieldMapping = relateMapping,
                PrevFieldPath = PrevFieldPath,
                CurrentFieldPath = LastFieldPath,
                Keys = relates
            };
            items.Add(relateItem);
            return RelationLinkType.AddLink;
        }

        public RelationItem[] GetRelationItems()
        {
            return items.ToArray();
        }

        public RelationLink Fork()
        {
            var link = new RelationLink
            {
                CycleItem = CycleItem,
                LastFieldPath = LastFieldPath,
                keyCount = keyCount,
                items = new List<RelationItem>(items)
            };
            return link;
        }
    }
}

