using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Light.Data
{
    internal class MultiParameterLambdaState : LambdaState
    {
        private readonly Dictionary<string, IMap> _mapDict = new Dictionary<string, IMap>();

        private readonly Dictionary<string, string> _aliasDict = new Dictionary<string, string>();

        public MultiParameterLambdaState(ICollection<ParameterExpression> parameterCollection, IReadOnlyList<IMap> maps)
        {
            if (parameterCollection.Count != maps.Count) {
                throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
            }
            var index = 0;
            foreach (var parameter in parameterCollection) {
                var name = parameter.Name;
                var type = parameter.Type;
                var map = maps[index];
                if (type != map.Type) {
                    throw new LambdaParseException(LambdaParseMessage.ParameterTypeError, name, type);
                }
                _mapDict[name] = map;
                _aliasDict[name] = "T" + index;
                index++;
            }
        }

        public override bool CheckParameter(string name, Type type)
        {
            if (_mapDict.TryGetValue(name, out var map)) {
                return map.Type == type;
            }

            return false;
        }

        public override DataFieldInfo GetDataFieldInfo(string fullPath)
        {
            var index = fullPath.IndexOf(".", StringComparison.Ordinal);
            if (index < 0) {
                if (_mapDict.TryGetValue(fullPath, out var map)) {
                    var info = map.GetFieldInfoForPath(fullPath);
                    var aliasTableName = _aliasDict[fullPath];
                    info = info.CreateAliasTableInfo(aliasTableName);
                    return info;
                }
                throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathError, fullPath);
            }
            else
            {
                var name = fullPath.Substring(0, index);
                var path = fullPath.Substring(index);
                if (_mapDict.TryGetValue(name, out var map))
                {
                    var info = map.GetFieldInfoForPath(path);
                    var aliasTableName = _aliasDict[name];
                    info = info.CreateAliasTableInfo(aliasTableName);
                    return info;
                }
                throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
            }
        }

        public override LambdaPathType ParsePath(string fullPath)
        {
            var index = fullPath.IndexOf(".", StringComparison.Ordinal);
            if (index == -1) {
                if (_mapDict.ContainsKey(fullPath)) {
                    return LambdaPathType.Parameter;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathError, fullPath);
                }
            }
            var name = fullPath.Substring(0, index);
            var path = fullPath.Substring(index);
            if (_mapDict.TryGetValue(name, out var map))
            {
                if (map.CheckIsField(path)) {
                    return LambdaPathType.Field;
                }

                if (map.CheckIsRelateEntity(path)) {
                    return LambdaPathType.RelateEntity;
                }

                if (map.CheckIsEntityCollection(path)) {
                    return LambdaPathType.RelateCollection;
                }

                return LambdaPathType.None;
            }

            throw new LambdaParseException(LambdaParseMessage.ExpressionFieldPathNotExists, fullPath);
        }

        public override ISelector CreateSelector(string[] fullPaths)
        {
            var dict = new Dictionary<string, List<string>>();
            foreach (var fullPath in fullPaths) {
                var index = fullPath.IndexOf(".", StringComparison.Ordinal);
                if (index < 0) {
                    if (_mapDict.ContainsKey(fullPath)) {
                        if (!dict.TryGetValue(fullPath, out var list)) {
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
                    var name = fullPath.Substring(0, index);
                    var path = fullPath.Substring(index);
                    if (_mapDict.ContainsKey(name)) {
                        if (!dict.TryGetValue(name, out var list)) {
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
            var selectDict = new Dictionary<string, Selector>();
            foreach (var kvs in dict) {
                var map = _mapDict[kvs.Key];
                var alias = _aliasDict[kvs.Key];
                var selector = map.CreateSelector(kvs.Value.ToArray()) as Selector;
                if (selector == null) {
                    throw new LambdaParseException(LambdaParseMessage.NotSupportRelateEntityJoinSelect);
                }
                selectDict.Add(alias, selector);
            }
            var joinSelector = Selector.ComposeSelector(selectDict);
            return joinSelector;
        }
    }
}

