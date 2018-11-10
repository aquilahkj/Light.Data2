using System;
using System.Collections.Generic;

namespace Light.Data
{
    class RelationMap : IMap
    {
        readonly DataEntityMapping rootMapping;

        public DataEntityMapping RootMapping {
            get {
                return rootMapping;
            }
        }

        public Type Type {
            get {
                return rootMapping.ObjectType;
            }
        }

        ISelector selector;

        readonly List<EntityJoinModel> models = new List<EntityJoinModel>();

        readonly Dictionary<string, RelationItem> mapDict = new Dictionary<string, RelationItem>();

        readonly Dictionary<string, DataFieldInfo[]> tableInfoDict = new Dictionary<string, DataFieldInfo[]>();

        readonly Dictionary<string, DataFieldInfo> fieldInfoDict = new Dictionary<string, DataFieldInfo>();

        readonly List<RelationLink> linkList = new List<RelationLink>();

        readonly Dictionary<string, string> cycleDict = new Dictionary<string, string>();

        readonly Dictionary<string, string[]> collectionDict = new Dictionary<string, string[]>();

        readonly Dictionary<string, string[]> singleDict = new Dictionary<string, string[]>();

        void LoadJoinRelate()
        {
            LoadEntityMapping(this.rootMapping, null);
            JoinSelector joinSelector = new JoinSelector();
            List<RelationItem> items = new List<RelationItem>();
            Dictionary<string, RelationItem> relationItemDict = new Dictionary<string, RelationItem>();
            int tindex = 0;
            foreach (RelationLink link in linkList) {
                RelationItem[] sitems = link.GetRelationItems();
                foreach (RelationItem item in sitems) {
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

            RelationItem rootItem = items[0];
            mapDict.Add(items[0].CurrentFieldPath, items[0]);
            List<DataFieldInfo> rootInfoList = new List<DataFieldInfo>();
            foreach (DataFieldMapping field in this.rootMapping.DataEntityFields) {
                DataFieldInfo info = new DataFieldInfo(field);
                string aliasName = string.Format("{0}_{1}", rootItem.AliasName, info.FieldName);
                AliasDataFieldInfo alias = new AliasDataFieldInfo(info, aliasName, rootItem.AliasName);
                //alias.AliasTablesName = rootItem.AliasName;
                joinSelector.SetAliasDataField(alias);
                rootInfoList.Add(alias);
                fieldInfoDict.Add(string.Format("{0}.{1}", items[0].CurrentFieldPath, field.IndexName), alias);
            }
            tableInfoDict.Add(items[0].CurrentFieldPath, rootInfoList.ToArray());

            for (int i = 1; i < items.Count; i++) {
                RelationItem mitem = items[i];
                mapDict.Add(mitem.CurrentFieldPath, mitem);
                SingleRelationFieldMapping fieldMapping = mitem.FieldMapping;
                DataEntityMapping mapping = fieldMapping.RelateMapping;
                DataFieldExpression expression = null;
                //DataFieldInfoRelation [] relations = fieldMapping.GetDataFieldInfoRelations ();

                RelationItem ritem = mapDict[mitem.PrevFieldPath];
                string malias = ritem.AliasName;
                string ralias = mitem.AliasName;

                DataFieldInfoRelation[] relations = fieldMapping.CreateDataFieldInfoRelations(malias, ralias);
                foreach (DataFieldInfoRelation relation in relations) {
                    DataFieldInfo minfo = relation.MasterInfo;
                    //minfo.AliasTableName = malias;
                    DataFieldInfo rinfo = relation.RelateInfo;
                    //rinfo.AliasTableName = ralias;
                    DataFieldMatchExpression keyExpression = new DataFieldMatchExpression(minfo, rinfo, QueryPredicate.Eq);
                    expression = DataFieldExpression.And(expression, keyExpression);
                    //expression = DataFieldExpression.And (expression, minfo == rinfo);
                }
                List<DataFieldInfo> infoList = new List<DataFieldInfo>();
                foreach (DataFieldMapping field in mapping.DataEntityFields) {
                    DataFieldInfo info = new DataFieldInfo(field);
                    string aliasName = string.Format("{0}_{1}", ralias, info.FieldName);
                    AliasDataFieldInfo alias = new AliasDataFieldInfo(info, aliasName, ralias);
                    //alias.AliasTableName = ralias;
                    joinSelector.SetAliasDataField(alias);
                    infoList.Add(alias);
                    fieldInfoDict.Add(string.Format("{0}.{1}", mitem.CurrentFieldPath, field.Name), alias);
                }
                tableInfoDict.Add(mitem.CurrentFieldPath, infoList.ToArray());

                JoinConnect connect = new JoinConnect(JoinType.LeftJoin, expression);
                EntityJoinModel model = new EntityJoinModel(mapping, ralias, connect, null, null, JoinSetting.None);
                this.selector = joinSelector;
                this.models.Add(model);
            }
        }

        void LoadNormal()
        {
            List<DataFieldInfo> rootInfoList = new List<DataFieldInfo>();
            Selector dataSelector = new Selector();
            foreach (DataFieldMapping fieldMapping in this.rootMapping.DataEntityFields) {
                if (fieldMapping != null) {
                    DataFieldInfo field = new DataFieldInfo(fieldMapping);
                    fieldInfoDict.Add(string.Format("{0}.{1}", string.Empty, fieldMapping.IndexName), field);
                    rootInfoList.Add(field);
                    dataSelector.SetSelectField(field);
                }
            }
            tableInfoDict.Add(string.Empty, rootInfoList.ToArray());
            string path = string.Empty;
            foreach (CollectionRelationFieldMapping collectFieldMapping in rootMapping.CollectionRelationFieldMappings) {
                RelationKey[] kps = collectFieldMapping.GetKeyPairs();
                string[] masters = new string[kps.Length];
                for (int i = 0; i < kps.Length; i++) {
                    masters[i] = string.Format("{0}.{1}", path, kps[i].MasterKey);
                }
                string collectField = string.Format("{0}.{1}", path, collectFieldMapping.FieldName);
                collectionDict.Add(collectField, masters);
            }
            this.selector = dataSelector;
        }

        public RelationMap(DataEntityMapping rootMapping)
        {
            this.rootMapping = rootMapping;
            if (rootMapping.HasJoinRelateModel) {
                LoadJoinRelate();
            }
            else {
                LoadNormal();
            }
        }

        public List<IJoinModel> CreateJoinModels(QueryExpression query, OrderExpression order)
        {
            List<IJoinModel> joinModels = new List<IJoinModel>();
            EntityJoinModel model1 = new EntityJoinModel(rootMapping, "T0", null, query, order, JoinSetting.None);
            joinModels.Add(model1);
            joinModels.AddRange(this.models);
            return joinModels;
        }

        void LoadEntityMapping(DataEntityMapping mapping, RelationLink link)
        {
            string path = link != null ? link.LastFieldPath : string.Empty;
            foreach (SingleRelationFieldMapping relateFieldMapping in mapping.SingleJoinTableRelationFieldMappings) {
                relateFieldMapping.InitialRelation();
                bool add = false;
                if (link == null) {
                    RelationLink mlink = new RelationLink(relateFieldMapping, string.Empty);
                    linkList.Add(mlink);
                    LoadEntityMapping(relateFieldMapping.RelateMapping, mlink);
                    add = true;
                }
                else {
                    RelationLink flink = link.Fork();
                    RelationLinkType linkType = flink.TryAddField(relateFieldMapping);
                    if (linkType == RelationLinkType.NoMatch) {
                        //新开关系链分支加入链集合
                        RelationLink mlink = new RelationLink(relateFieldMapping, link.LastFieldPath);
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
                    RelationKey[] kps = relateFieldMapping.GetKeyPairs();
                    string[] relates = new string[kps.Length];
                    for (int i = 0; i < kps.Length; i++) {
                        relates[i] = string.Format("{0}.{1}.{2}", path, relateFieldMapping.FieldName, kps[i].RelateKey);
                    }
                    string relate = string.Format("{0}.{1}", path, relateFieldMapping.FieldName);
                    singleDict[relate] = relates;
                }
            }
            foreach (CollectionRelationFieldMapping collectFieldMapping in mapping.CollectionRelationFieldMappings) {
                RelationKey[] kps = collectFieldMapping.GetKeyPairs();
                string[] masters = new string[kps.Length];
                for (int i = 0; i < kps.Length; i++) {
                    masters[i] = string.Format("{0}.{1}", path, kps[i].MasterKey);
                }
                string collectField = string.Format("{0}.{1}", path, collectFieldMapping.FieldName);
                collectionDict[collectField] = masters;
            }
        }

        public bool CheckValid(string fieldPath, out string aliasName)
        {
            bool ret = mapDict.TryGetValue(fieldPath, out RelationItem item);
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
            if (fieldInfoDict.TryGetValue(path, out DataFieldInfo info)) {
                return info;
            }
            else {
                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
            }
        }

        DataFieldInfo GetFieldInfoForField(string path)
        {
            if (fieldInfoDict.TryGetValue(path, out DataFieldInfo info)) {
                return info;
            }
            else {
                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
            }
        }

        DataFieldInfo[] GetFieldInfoForSingleField(string path)
        {
            if (singleDict.TryGetValue(path, out string[] fields)) {
                DataFieldInfo[] infos = new DataFieldInfo[fields.Length];
                for (int i = 0; i < fields.Length; i++) {
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

        HashSet<string> RewritePaths(string[] paths)
        {
            HashSet<string> ss = new HashSet<string>(paths);
            if (collectionDict.Count > 0) {
                foreach (string path in paths) {
                    if (collectionDict.TryGetValue(path, out string[] arr)) {
                        foreach (string item in arr) {
                            ss.Add(item);
                        }
                    }
                }
            }
            return ss;
        }

        HashSet<DataFieldInfo> GetInfos(string[] paths)
        {
            HashSet<DataFieldInfo> ss = new HashSet<DataFieldInfo>();
            if (collectionDict.Count > 0) {
                foreach (string path in paths) {
                    if (tableInfoDict.TryGetValue(path, out DataFieldInfo[] arr)) {
                        foreach (DataFieldInfo item in arr) {
                            ss.Add(item);
                        }
                    }
                    if (fieldInfoDict.TryGetValue(path, out DataFieldInfo field)) {
                        ss.Add(field);
                    }
                }
            }
            return ss;
        }

        public ISelector CreateSelector(string[] paths)
        {
            HashSet<string> allPaths = RewritePaths(paths);
            HashSet<DataFieldInfo> hash = new HashSet<DataFieldInfo>();
            HashSet<string> stable = new HashSet<string>();
            foreach (string path in allPaths) {
                if (fieldInfoDict.TryGetValue(path, out DataFieldInfo info)) {
                    if (!hash.Contains(info)) {
                        hash.Add(info);
                        int index = path.LastIndexOf('.');
                        if (index > 0) {
                            string t = path.Substring(0, index);
                            if (!stable.Contains(t)) {
                                stable.Add(t);
                                DataFieldInfo[] sinfos = GetFieldInfoForSingleField(t);
                                foreach (DataFieldInfo sinfo in sinfos) {
                                    if (!hash.Contains(sinfo)) {
                                        hash.Add(sinfo);
                                    }
                                }
                            }
                        }
                    }
                    continue;
                }
                if (tableInfoDict.TryGetValue(path, out DataFieldInfo[] tinfos)) {
                    foreach (DataFieldInfo tinfo in tinfos) {
                        stable.Add(path);
                        if (!hash.Contains(tinfo)) {
                            hash.Add(tinfo);
                        }
                    }
                    continue;
                }
                throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedFieldViaPath, path));
            }
            if (rootMapping.HasJoinRelateModel) {
                JoinSelector jselector = new JoinSelector();
                foreach (AliasDataFieldInfo finfo in hash) {
                    jselector.SetAliasDataField(finfo);
                }
                return jselector;
            }
            else {
                Selector nselector = new Selector();
                foreach (DataFieldInfo finfo in hash) {
                    nselector.SetSelectField(finfo);
                }
                return nselector;
            }
        }

        public ISelector CreateExceptSelector(string[] paths)
        {
            HashSet<DataFieldInfo> exceptInfo = GetInfos(paths);
            if (rootMapping.HasJoinRelateModel) {
                JoinSelector jselector = new JoinSelector();
                foreach (KeyValuePair<string, DataFieldInfo> kvs in fieldInfoDict) {
                    if (!exceptInfo.Contains(kvs.Value)) {
                        jselector.SetAliasDataField(kvs.Value as AliasDataFieldInfo);
                    }
                }
                return jselector;
            }
            else {
                Selector nselector = new Selector();
                foreach (KeyValuePair<string, DataFieldInfo> kvs in fieldInfoDict) {
                    if (!exceptInfo.Contains(kvs.Value)) {
                        nselector.SetSelectField(kvs.Value);
                    }
                }
                return nselector;
            }
        }
    }
}

