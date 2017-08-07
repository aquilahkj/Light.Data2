using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Light.Data
{
    class MutliParameterLambdaState : LambdaState
    {
        readonly Dictionary<string, IMap> mapDict = new Dictionary<string, IMap>();

        readonly Dictionary<string, string> aliasDict = new Dictionary<string, string>();

        public MutliParameterLambdaState(ICollection<ParameterExpression> paramters, List<IMap> maps)
        {
            if (paramters.Count != maps.Count) {
                throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
            }
            int index = 0;
            foreach (ParameterExpression parameter in paramters) {
                string name = parameter.Name;
                Type type = parameter.Type;
                IMap map = maps[index];
                if (type != map.Type) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterTypeError, name, type);
                }
                mapDict[name] = map;
                aliasDict[name] = "T" + index;
                index++;
            }
        }

        public override bool CheckPamramter(string name, Type type)
        {
            if (mapDict.TryGetValue(name, out IMap map)) {
                return map.Type == type;
            }
            else {
                return false;
            }
        }

        public override DataFieldInfo GetDataFieldInfo(string fullPath)
        {
            int index = fullPath.IndexOf(".", StringComparison.Ordinal);
            if (index < 0) {
                if (mapDict.TryGetValue(fullPath, out IMap map1)) {
                    DataFieldInfo info = map1.GetFieldInfoForPath(fullPath);
                    string aliasTableName = aliasDict[fullPath];
                    info = info.CreateAliasTableInfo(aliasTableName);
                    return info;
                }
                throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathError, fullPath);
            }
            string name = fullPath.Substring(0, index);
            string path = fullPath.Substring(index);
            if (mapDict.TryGetValue(name, out IMap map)) {
                DataFieldInfo info = map.GetFieldInfoForPath(path);
                string aliasTableName = aliasDict[name];
                info = info.CreateAliasTableInfo(aliasTableName);
                return info;
            }
            else {
                throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
            }
        }

        public override LambdaPathType ParsePath(string fullPath)
        {
            int index = fullPath.IndexOf(".", StringComparison.Ordinal);
            if (index == -1) {
                if (mapDict.ContainsKey(fullPath)) {
                    return LambdaPathType.Parameter;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathError, fullPath);
                }
            }
            string name = fullPath.Substring(0, index);
            string path = fullPath.Substring(index);
            if (mapDict.TryGetValue(name, out IMap map)) {
                if (map.CheckIsField(path)) {
                    return LambdaPathType.Field;
                }
                else if (map.CheckIsRelateEntity(path)) {
                    return LambdaPathType.RelateEntity;
                }
                else if (map.CheckIsEntityCollection(path)) {
                    return LambdaPathType.RelateCollection;
                }
                else {
                    return LambdaPathType.None;
                }
            }
            else {
                throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
            }
        }

        public override ISelector CreateSelector(string[] fullPaths)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            foreach (string fullPath in fullPaths) {
                int index = fullPath.IndexOf(".", StringComparison.Ordinal);
                if (index < 0) {
                    if (mapDict.ContainsKey(fullPath)) {
                        if (!dict.TryGetValue(fullPath, out List<string> list)) {
                            list = new List<string>();
                            dict.Add(fullPath, list);
                        }
                        if (!list.Contains(string.Empty)) {
                            list.Add(string.Empty);
                        }
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathError, fullPath);
                    }
                }
                else {
                    string name = fullPath.Substring(0, index);
                    string path = fullPath.Substring(index);
                    if (mapDict.ContainsKey(name)) {
                        if (!dict.TryGetValue(name, out List<string> list)) {
                            list = new List<string>();
                            dict.Add(name, list);
                        }
                        if (!list.Contains(path)) {
                            list.Add(path);
                        }
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
                    }
                }
            }
            Dictionary<string, Selector> selectDict = new Dictionary<string, Selector>();
            foreach (KeyValuePair<string, List<string>> kvs in dict) {
                IMap map = mapDict[kvs.Key];
                string alias = aliasDict[kvs.Key];
                Selector selector = map.CreateSelector(kvs.Value.ToArray()) as Selector;
                if (selector == null) {
                    throw new LambdaParseException(LambdaParseMessage.NotSupportRelateEnityJoinSelect);
                }
                selectDict.Add(alias, selector);
            }
            JoinSelector joinSelector = Selector.ComposeSelector(selectDict);
            return joinSelector;
        }
    }
}

