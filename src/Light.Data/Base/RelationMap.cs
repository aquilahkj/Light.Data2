using System;
using System.Collections.Generic;

namespace Light.Data
{
    internal class RelationMap : IMap
    {
        public DataEntityMapping RootMapping { get; }

        public Type Type => RootMapping.ObjectType;

        private ISelector selector;

        private readonly List<EntityJoinModel> models = new List<EntityJoinModel>();

        private readonly Dictionary<string, RelationItem> mapDict = new Dictionary<string, RelationItem>();

        private readonly Dictionary<string, DataFieldInfo[]> tableInfoDict = new Dictionary<string, DataFieldInfo[]>();

        private readonly Dictionary<string, DataFieldInfo> fieldInfoDict = new Dictionary<string, DataFieldInfo>();

        private readonly List<RelationLink> linkList = new List<RelationLink>();

        private readonly Dictionary<string, string> cycleDict = new Dictionary<string, string>();

        private readonly Dictionary<string, string[]> collectionDict = new Dictionary<string, string[]>();

        private readonly Dictionary<string, string[]> singleDict = new Dictionary<string, string[]>();

        private void LoadJoinRelate()
        {
            LoadEntityMapping(RootMapping, null);
            var joinSelector = new JoinSelector();
            var items = new List<RelationItem>();
            // var relationItemDict = new Dictionary<string, RelationItem>();
            var tIndex = 0;
            foreach (var link in linkList) {
                var sItems = link.GetRelationItems();
                foreach (var item in sItems) {
                    if (!items.Contains(item)) {
                        if (item.FieldMapping == null && tIndex != 0) {
                            continue;
                        }
                        item.AliasName = "T" + tIndex;
                        tIndex++;
                        items.Add(item);
                        // relationItemDict.Add(item.CurrentFieldPath, item);
                    }
                }
            }

            var rootItem = items[0];
            mapDict.Add(items[0].CurrentFieldPath, items[0]);
            var rootInfoList = new List<DataFieldInfo>();
            foreach (var field in RootMapping.DataEntityFields) {
                var info = new DataFieldInfo(field);
                var aliasName = $"{rootItem.AliasName}_{info.FieldName}";
                var alias = new AliasDataFieldInfo(info, aliasName, rootItem.AliasName);
                joinSelector.SetAliasDataField(alias);
                rootInfoList.Add(alias);
                fieldInfoDict.Add($"{items[0].CurrentFieldPath}.{field.IndexName}", alias);
            }
            tableInfoDict.Add(items[0].CurrentFieldPath, rootInfoList.ToArray());

            for (var i = 1; i < items.Count; i++) {
                var mItem = items[i];
                mapDict.Add(mItem.CurrentFieldPath, mItem);
                var fieldMapping = mItem.FieldMapping;
                var mapping = fieldMapping.RelateMapping;
                DataFieldExpression expression = null;

                var rItem = mapDict[mItem.PrevFieldPath];
                var mAlias = rItem.AliasName;
                var rAlias = mItem.AliasName;

                var relations = fieldMapping.CreateDataFieldInfoRelations(mAlias, rAlias);
                foreach (var relation in relations) {
                    var mInfo = relation.MasterInfo;
                    var rInfo = relation.RelateInfo;
                    var keyExpression = new DataFieldOnMatchExpression(mInfo, rInfo, QueryPredicate.Eq);
                    expression = DataFieldExpression.And(expression, keyExpression);
                }
                var infoList = new List<DataFieldInfo>();
                foreach (var field in mapping.DataEntityFields) {
                    var info = new DataFieldInfo(field);
                    var aliasName = $"{rAlias}_{info.FieldName}";
                    var alias = new AliasDataFieldInfo(info, aliasName, rAlias);
                    joinSelector.SetAliasDataField(alias);
                    infoList.Add(alias);
                    fieldInfoDict.Add($"{mItem.CurrentFieldPath}.{field.Name}", alias);
                }
                tableInfoDict.Add(mItem.CurrentFieldPath, infoList.ToArray());

                var connect = new JoinConnect(JoinType.LeftJoin, expression);
                var model = new EntityJoinModel(mapping, rAlias, connect, null, null, JoinSetting.None);
                selector = joinSelector;
                models.Add(model);
            }
        }

        private void LoadNormal()
        {
            var rootInfoList = new List<DataFieldInfo>();
            var dataSelector = new Selector();
            foreach (var fieldMapping in RootMapping.DataEntityFields) {
                if (fieldMapping != null) {
                    var field = new DataFieldInfo(fieldMapping);
                    fieldInfoDict.Add($"{string.Empty}.{fieldMapping.IndexName}", field);
                    rootInfoList.Add(field);
                    dataSelector.SetSelectField(field);
                }
            }
            tableInfoDict.Add(string.Empty, rootInfoList.ToArray());
            var path = string.Empty;
            foreach (var collectFieldMapping in RootMapping.CollectionRelationFieldMappings) {
                var kps = collectFieldMapping.GetKeyPairs();
                var masters = new string[kps.Length];
                for (var i = 0; i < kps.Length; i++) {
                    masters[i] = $"{path}.{kps[i].MasterKey}";
                }
                var collectField = $"{path}.{collectFieldMapping.FieldName}";
                collectionDict.Add(collectField, masters);
            }
            selector = dataSelector;
        }

        public RelationMap(DataEntityMapping rootMapping)
        {
            RootMapping = rootMapping;
            if (rootMapping.HasJoinRelateModel) {
                LoadJoinRelate();
            }
            else {
                LoadNormal();
            }
        }

        public List<IJoinModel> CreateJoinModels(QueryExpression query, OrderExpression order)
        {
            var joinModels = new List<IJoinModel>();
            var model1 = new EntityJoinModel(RootMapping, "T0", null, query, order, JoinSetting.None);
            joinModels.Add(model1);
            joinModels.AddRange(models);
            return joinModels;
        }

        private void LoadEntityMapping(DataEntityMapping mapping, RelationLink link)
        {
            var path = link != null ? link.LastFieldPath : string.Empty;
            foreach (var relateFieldMapping in mapping.SingleJoinTableRelationFieldMappings) {
                relateFieldMapping.InitialRelation();
                var add = false;
                if (link == null) {
                    var mLink = new RelationLink(relateFieldMapping, string.Empty);
                    linkList.Add(mLink);
                    LoadEntityMapping(relateFieldMapping.RelateMapping, mLink);
                    add = true;
                }
                else {
                    var fLink = link.Fork();
                    var linkType = fLink.TryAddField(relateFieldMapping);
                    if (linkType == RelationLinkType.NoMatch) {
                        //新开关系链分支加入链集合
                        var mLink = new RelationLink(relateFieldMapping, link.LastFieldPath);
                        linkList.Add(mLink);
                        LoadEntityMapping(relateFieldMapping.RelateMapping, mLink);
                        add = true;
                    }
                    else if (linkType == RelationLinkType.AddLink) {
                        //将原关系链追加后再加入链集合
                        linkList.Add(fLink);
                        LoadEntityMapping(relateFieldMapping.RelateMapping, fLink);
                        add = true;
                    }
                    else {
                        //原关系链已能形成闭环，不需修改
                        cycleDict.Add(fLink.LastFieldPath, fLink.CycleFieldPath);
                    }
                }
                if (add) {
                    var kps = relateFieldMapping.GetKeyPairs();
                    var relates = new string[kps.Length];
                    for (var i = 0; i < kps.Length; i++) {
                        relates[i] = $"{path}.{relateFieldMapping.FieldName}.{kps[i].RelateKey}";
                    }
                    var relate = $"{path}.{relateFieldMapping.FieldName}";
                    singleDict[relate] = relates;
                }
            }
            foreach (var collectFieldMapping in mapping.CollectionRelationFieldMappings) {
                var kps = collectFieldMapping.GetKeyPairs();
                var masters = new string[kps.Length];
                for (var i = 0; i < kps.Length; i++) {
                    masters[i] = $"{path}.{kps[i].MasterKey}";
                }
                var collectField = $"{path}.{collectFieldMapping.FieldName}";
                collectionDict[collectField] = masters;
            }
        }

        public bool CheckValid(string fieldPath, out string aliasName)
        {
            var ret = mapDict.TryGetValue(fieldPath, out var item);
            aliasName = ret ? item.AliasName : null;
            return ret;
        }

        public bool TryGetCycleFieldPath(string fieldPath, out string cycleFieldPath)
        {
            if (cycleDict.Count == 0) {
                cycleFieldPath = null;
                return false;
            }
            return cycleDict.TryGetValue(fieldPath, out cycleFieldPath);
        }

        public bool CheckIsField(string path)
        {
            return fieldInfoDict.ContainsKey(path);
        }

        public bool CheckIsRelateEntity(string path)
        {
            return tableInfoDict.ContainsKey(path);
        }

        public bool CheckIsEntityCollection(string path)
        {
            return collectionDict.ContainsKey(path);
        }

        public DataFieldInfo GetFieldInfoForPath(string path)
        {
            if (fieldInfoDict.TryGetValue(path, out var info)) {
                return info;
            }

            throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
        }

        private DataFieldInfo GetFieldInfoForField(string path)
        {
            if (fieldInfoDict.TryGetValue(path, out var info)) {
                return info;
            }

            throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
        }

        private DataFieldInfo[] GetFieldInfoForSingleField(string path)
        {
            if (singleDict.TryGetValue(path, out var fields)) {
                var infos = new DataFieldInfo[fields.Length];
                for (var i = 0; i < fields.Length; i++) {
                    infos[i] = GetFieldInfoForField(fields[i]);
                }
                return infos;
            }

            throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
        }

        public ISelector GetDefaultSelector()
        {
            return selector;
        }

        private HashSet<string> RewritePaths(string[] paths)
        {
            var ss = new HashSet<string>(paths);
            if (collectionDict.Count > 0) {
                foreach (var path in paths) {
                    if (collectionDict.TryGetValue(path, out var arr)) {
                        foreach (var item in arr) {
                            ss.Add(item);
                        }
                    }
                }
            }
            return ss;
        }

        private HashSet<DataFieldInfo> GetInfos(string[] paths)
        {
            var ss = new HashSet<DataFieldInfo>();
            if (collectionDict.Count > 0) {
                foreach (var path in paths) {
                    if (tableInfoDict.TryGetValue(path, out var arr)) {
                        foreach (var item in arr) {
                            ss.Add(item);
                        }
                    }
                    if (fieldInfoDict.TryGetValue(path, out var field)) {
                        ss.Add(field);
                    }
                }
            }
            return ss;
        }

        public ISelector CreateSelector(string[] paths)
        {
            var allPaths = RewritePaths(paths);
            var hash = new HashSet<DataFieldInfo>();
            var stable = new HashSet<string>();
            foreach (var path in allPaths) {
                if (fieldInfoDict.TryGetValue(path, out var info)) {
                    if (!hash.Contains(info)) {
                        hash.Add(info);
                        var index = path.LastIndexOf('.');
                        if (index > 0) {
                            var t = path.Substring(0, index);
                            if (!stable.Contains(t)) {
                                stable.Add(t);
                                var sInfos = GetFieldInfoForSingleField(t);
                                foreach (var sInfo in sInfos) {
                                    if (!hash.Contains(sInfo)) {
                                        hash.Add(sInfo);
                                    }
                                }
                            }
                        }
                    }
                    continue;
                }
                if (tableInfoDict.TryGetValue(path, out var tInfos)) {
                    foreach (var tInfo in tInfos) {
                        stable.Add(path);
                        if (!hash.Contains(tInfo)) {
                            hash.Add(tInfo);
                        }
                    }
                    continue;
                }
                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
            }
            if (RootMapping.HasJoinRelateModel) {
                var jSelector = new JoinSelector();
                foreach (var dataFieldInfo in hash)
                {
                    var info = (AliasDataFieldInfo) dataFieldInfo;
                    jSelector.SetAliasDataField(info);
                }
                return jSelector;
            }

            var nSelector = new Selector();
            foreach (var item in hash) {
                nSelector.SetSelectField(item);
            }
            return nSelector;
        }

        public ISelector CreateExceptSelector(string[] paths)
        {
            var exceptInfo = GetInfos(paths);
            if (RootMapping.HasJoinRelateModel) {
                var jSelector = new JoinSelector();
                foreach (var kvs in fieldInfoDict) {
                    if (!exceptInfo.Contains(kvs.Value)) {
                        jSelector.SetAliasDataField(kvs.Value as AliasDataFieldInfo);
                    }
                }
                return jSelector;
            }

            var nSelector = new Selector();
            foreach (var kvs in fieldInfoDict) {
                if (!exceptInfo.Contains(kvs.Value)) {
                    nSelector.SetSelectField(kvs.Value);
                }
            }
            return nSelector;
        }
    }
}

