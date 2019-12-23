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
            var relationItemDict = new Dictionary<string, RelationItem>();
            var tindex = 0;
            foreach (var link in linkList) {
                var sitems = link.GetRelationItems();
                foreach (var item in sitems) {
                    if (!items.Contains(item)) {
                        if (item.FieldMapping == null && tindex != 0) {
                            continue;
                        }
                        item.AliasName = "T" + tindex;
                        tindex++;
                        items.Add(item);
                        relationItemDict.Add(item.CurrentFieldPath, item);
                    }
                }
            }
            //if (this.rootMapping != items [0].DataMapping) {
            //	throw new LightDataException (RE.RelationMapEntityMappingError);
            //}

            var rootItem = items[0];
            mapDict.Add(items[0].CurrentFieldPath, items[0]);
            var rootInfoList = new List<DataFieldInfo>();
            foreach (var field in RootMapping.DataEntityFields) {
                var info = new DataFieldInfo(field);
                var aliasName = string.Format("{0}_{1}", rootItem.AliasName, info.FieldName);
                var alias = new AliasDataFieldInfo(info, aliasName, rootItem.AliasName);
                //alias.AliasTablesName = rootItem.AliasName;
                joinSelector.SetAliasDataField(alias);
                rootInfoList.Add(alias);
                fieldInfoDict.Add(string.Format("{0}.{1}", items[0].CurrentFieldPath, field.IndexName), alias);
            }
            tableInfoDict.Add(items[0].CurrentFieldPath, rootInfoList.ToArray());

            for (var i = 1; i < items.Count; i++) {
                var mitem = items[i];
                mapDict.Add(mitem.CurrentFieldPath, mitem);
                var fieldMapping = mitem.FieldMapping;
                var mapping = fieldMapping.RelateMapping;
                DataFieldExpression expression = null;
                //DataFieldInfoRelation [] relations = fieldMapping.GetDataFieldInfoRelations ();

                var ritem = mapDict[mitem.PrevFieldPath];
                var malias = ritem.AliasName;
                var ralias = mitem.AliasName;

                var relations = fieldMapping.CreateDataFieldInfoRelations(malias, ralias);
                foreach (var relation in relations) {
                    var minfo = relation.MasterInfo;
                    //minfo.AliasTableName = malias;
                    var rinfo = relation.RelateInfo;
                    //rinfo.AliasTableName = ralias;
                    var keyExpression = new DataFieldMatchExpression(minfo, rinfo, QueryPredicate.Eq);
                    expression = DataFieldExpression.And(expression, keyExpression);
                    //expression = DataFieldExpression.And (expression, minfo == rinfo);
                }
                var infoList = new List<DataFieldInfo>();
                foreach (var field in mapping.DataEntityFields) {
                    var info = new DataFieldInfo(field);
                    var aliasName = string.Format("{0}_{1}", ralias, info.FieldName);
                    var alias = new AliasDataFieldInfo(info, aliasName, ralias);
                    //alias.AliasTableName = ralias;
                    joinSelector.SetAliasDataField(alias);
                    infoList.Add(alias);
                    fieldInfoDict.Add(string.Format("{0}.{1}", mitem.CurrentFieldPath, field.Name), alias);
                }
                tableInfoDict.Add(mitem.CurrentFieldPath, infoList.ToArray());

                var connect = new JoinConnect(JoinType.LeftJoin, expression);
                var model = new EntityJoinModel(mapping, ralias, connect, null, null, JoinSetting.None);
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
                    fieldInfoDict.Add(string.Format("{0}.{1}", string.Empty, fieldMapping.IndexName), field);
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
                    masters[i] = string.Format("{0}.{1}", path, kps[i].MasterKey);
                }
                var collectField = string.Format("{0}.{1}", path, collectFieldMapping.FieldName);
                collectionDict.Add(collectField, masters);
            }
            selector = dataSelector;
        }

        public RelationMap(DataEntityMapping rootMapping)
        {
            this.RootMapping = rootMapping;
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
                    var mlink = new RelationLink(relateFieldMapping, string.Empty);
                    linkList.Add(mlink);
                    LoadEntityMapping(relateFieldMapping.RelateMapping, mlink);
                    add = true;
                }
                else {
                    var flink = link.Fork();
                    var linkType = flink.TryAddField(relateFieldMapping);
                    if (linkType == RelationLinkType.NoMatch) {
                        //新开关系链分支加入链集合
                        var mlink = new RelationLink(relateFieldMapping, link.LastFieldPath);
                        linkList.Add(mlink);
                        LoadEntityMapping(relateFieldMapping.RelateMapping, mlink);
                        add = true;
                    }
                    else if (linkType == RelationLinkType.AddLink) {
                        //将原关系链追加后再加入链集合
                        linkList.Add(flink);
                        LoadEntityMapping(relateFieldMapping.RelateMapping, flink);
                        add = true;
                    }
                    else {
                        //原关系链已能形成闭环，不需修改
                        cycleDict.Add(flink.LastFieldPath, flink.CycleFieldPath);
                    }
                }
                if (add) {
                    var kps = relateFieldMapping.GetKeyPairs();
                    var relates = new string[kps.Length];
                    for (var i = 0; i < kps.Length; i++) {
                        relates[i] = string.Format("{0}.{1}.{2}", path, relateFieldMapping.FieldName, kps[i].RelateKey);
                    }
                    var relate = string.Format("{0}.{1}", path, relateFieldMapping.FieldName);
                    singleDict[relate] = relates;
                }
            }
            foreach (var collectFieldMapping in mapping.CollectionRelationFieldMappings) {
                var kps = collectFieldMapping.GetKeyPairs();
                var masters = new string[kps.Length];
                for (var i = 0; i < kps.Length; i++) {
                    masters[i] = string.Format("{0}.{1}", path, kps[i].MasterKey);
                }
                var collectField = string.Format("{0}.{1}", path, collectFieldMapping.FieldName);
                collectionDict[collectField] = masters;
            }
        }

        public bool CheckValid(string fieldPath, out string aliasName)
        {
            var ret = mapDict.TryGetValue(fieldPath, out var item);
            if (ret) {
                aliasName = item.AliasName;
            }
            else {
                aliasName = null;
            }
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
            else {
                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
            }
        }

        private DataFieldInfo GetFieldInfoForField(string path)
        {
            if (fieldInfoDict.TryGetValue(path, out var info)) {
                return info;
            }
            else {
                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
            }
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
            else {
                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
            }
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
                                var sinfos = GetFieldInfoForSingleField(t);
                                foreach (var sinfo in sinfos) {
                                    if (!hash.Contains(sinfo)) {
                                        hash.Add(sinfo);
                                    }
                                }
                            }
                        }
                    }
                    continue;
                }
                if (tableInfoDict.TryGetValue(path, out var tinfos)) {
                    foreach (var tinfo in tinfos) {
                        stable.Add(path);
                        if (!hash.Contains(tinfo)) {
                            hash.Add(tinfo);
                        }
                    }
                    continue;
                }
                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
            }
            if (RootMapping.HasJoinRelateModel) {
                var jselector = new JoinSelector();
                foreach (AliasDataFieldInfo finfo in hash) {
                    jselector.SetAliasDataField(finfo);
                }
                return jselector;
            }
            else {
                var nselector = new Selector();
                foreach (var finfo in hash) {
                    nselector.SetSelectField(finfo);
                }
                return nselector;
            }
        }

        public ISelector CreateExceptSelector(string[] paths)
        {
            var exceptInfo = GetInfos(paths);
            if (RootMapping.HasJoinRelateModel) {
                var jselector = new JoinSelector();
                foreach (var kvs in fieldInfoDict) {
                    if (!exceptInfo.Contains(kvs.Value)) {
                        jselector.SetAliasDataField(kvs.Value as AliasDataFieldInfo);
                    }
                }
                return jselector;
            }
            else {
                var nselector = new Selector();
                foreach (var kvs in fieldInfoDict) {
                    if (!exceptInfo.Contains(kvs.Value)) {
                        nselector.SetSelectField(kvs.Value);
                    }
                }
                return nselector;
            }
        }
    }
}

