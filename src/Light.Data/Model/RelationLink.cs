using System;
using System.Collections.Generic;

namespace Light.Data
{
    enum RelationLinkType
    {
        NoMatch,
        AddLink,
        Cycle
    }

    class RelationLink
    {
        public string LastFieldPath {
            get {
                return lastFieldPath;
            }
        }

        public RelationItem CycleItem {
            get {
                return cycleItem;
            }
        }

        List<RelationItem> items = new List<RelationItem>();

        int keyCount;

        string lastFieldPath;

        string prevFieldPath;

        string cycleFieldPath;

        RelationItem cycleItem;

        public RelationItem LastItem {
            get {
                return items[items.Count - 1];
            }
        }

        public string CycleFieldPath {
            get {
                return cycleFieldPath;
            }
        }

        public string PrevFieldPath {
            get {
                return prevFieldPath;
            }
        }

        public int Count {
            get {
                return items.Count;
            }
        }

        private RelationLink()
        {

        }

        public RelationLink(SingleRelationFieldMapping rootRelationMapping, string fieldPath)
        {
            if (rootRelationMapping == null)
                throw new ArgumentNullException(nameof(rootRelationMapping));
            //this.rootRelationMapping = rootRelationMapping;
            RelationKey[] keys = rootRelationMapping.GetKeyPairs();
            this.keyCount = keys.Length;
            string[] masters = new string[this.keyCount];
            string[] relates = new string[this.keyCount];
            for (int i = 0; i < this.keyCount; i++) {
                masters[i] = keys[i].MasterKey;
                relates[i] = keys[i].RelateKey;
            }
            lastFieldPath = string.Format("{0}.{1}", fieldPath, rootRelationMapping.FieldName);
            prevFieldPath = fieldPath;
            RelationItem masterItem = new RelationItem() {
                DataMapping = rootRelationMapping.MasterMapping,
                FieldMapping = null,
                //PrevFieldPath = prevFieldPath,
                CurrentFieldPath = prevFieldPath,
                Keys = masters
            };

            RelationItem relateItem = new RelationItem() {
                DataMapping = rootRelationMapping.RelateMapping,
                FieldMapping = rootRelationMapping,
                PrevFieldPath = prevFieldPath,
                CurrentFieldPath = lastFieldPath,
                Keys = relates
            };

            items.Add(masterItem);
            items.Add(relateItem);
        }

        private bool IsMatch(string[] array1, string[] array2)
        {
            for (int i = 0; i < array1.Length; i++) {
                bool flag = false;
                for (int j = 0; j < array2.Length; j++) {
                    if (array1[i] == array2[j]) {
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
            RelationItem last = items[items.Count - 1];
            if (!Object.Equals(relateMapping.MasterMapping, last.DataMapping)) {
                throw new LightDataException(string.Format(SR.RelationFieldError, relateMapping.MasterMapping.ObjectType, relateMapping.FieldName));
            }
            RelationKey[] keys = relateMapping.GetKeyPairs();
            if (keys.Length != keyCount) {
                return RelationLinkType.NoMatch;
            }
            string[] masters = new string[this.keyCount];
            string[] relates = new string[this.keyCount];
            for (int i = 0; i < this.keyCount; i++) {
                masters[i] = keys[i].MasterKey;
                relates[i] = keys[i].RelateKey;
            }
            if (!IsMatch(last.Keys, masters)) {
                return RelationLinkType.NoMatch;
            }
            prevFieldPath = lastFieldPath;
            lastFieldPath = string.Format("{0}.{1}", last.CurrentFieldPath, relateMapping.FieldName);
            int len = items.Count - 1;
            for (int i = 0; i < len; i++) {
                RelationItem item = items[i];
                if (Object.Equals(relateMapping.RelateMapping, item.DataMapping)) {
                    if (IsMatch(item.Keys, relates)) {
                        cycleFieldPath = item.CurrentFieldPath;
                        return RelationLinkType.Cycle;
                    }
                    else {
                        throw new LightDataException(string.Format(SR.RelationFieldKeyNotMatch, relateMapping.MasterMapping.ObjectType, relateMapping.FieldName));
                    }
                }
            }
            RelationItem relateItem = new RelationItem() {
                DataMapping = relateMapping.RelateMapping,
                FieldMapping = relateMapping,
                PrevFieldPath = prevFieldPath,
                CurrentFieldPath = lastFieldPath,
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
            RelationLink link = new RelationLink();
            link.cycleItem = cycleItem;
            link.lastFieldPath = lastFieldPath;
            link.keyCount = keyCount;
            link.items = new List<RelationItem>(items);
            return link;
        }
    }
}

