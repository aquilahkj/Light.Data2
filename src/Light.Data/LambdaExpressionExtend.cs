using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Light.Data
{
    internal static class LambdaExpressionExtend
    {
        private static readonly TypeInfo EnumerableTypeInfo;

        static LambdaExpressionExtend()
        {
            EnumerableTypeInfo = typeof(IEnumerable).GetTypeInfo();
        }

        public static object ConvertLambdaObject(object value)
        {
            object obj;
            if (value is Delegate dele)
            {
                obj = dele.DynamicInvoke(null);
            }
            else
            {
                obj = value;
            }

            if (obj != null && Equals(obj, DBNull.Value))
            {
                obj = null;
            }

            return obj;
        }

        public static object ConvertObject(Expression expression)
        {
            if (expression is ConstantExpression constant)
            {
                return constant.Value;
            }

            var lambda = Expression.Lambda(expression);
            var fn = lambda.Compile();
            return fn;
        }

        public static SelectModel CreateSelectModel(LambdaExpression expression)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                var state = new SingleParameterLambdaState(expression.Parameters[0]);
                SelectModel model;
                switch (expression.Body)
                {
                    case MemberInitExpression memberInitObj:
                        model = ParseSelectModel(memberInitObj, state);
                        break;
                    case NewExpression newObj:
                        model = ParseSelectModel(newObj, state);
                        break;
                    default:
                        throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message),
                    ex);
            }
        }

        public static AggregateModel CreateAggregateModel(LambdaExpression expression)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                var state = new SingleParameterLambdaState(expression.Parameters[0]);
                AggregateModel model;
                switch (expression.Body)
                {
                    case MemberInitExpression memberInitObj:
                        model = ParseAggregateModel(memberInitObj, state);
                        break;
                    case NewExpression newObj:
                        model = ParseAggregateModel(newObj, state);
                        break;
                    default:
                        throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new LightDataException(
                    string.Format(SR.ParseExpressionError, "aggregate", expression, ex.Message), ex);
            }
        }

        public static ISelector CreateSelector(LambdaExpression expression)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new SingleParameterLambdaState(expression.Parameters[0])
                {
                    Mode = LambdaMode.OutputMode
                };
                var bodyExpression = expression.Body;
                var list = new List<string>();
                if (ParseNewArguments(bodyExpression, state, ref list))
                {
                    return state.CreateSelector(list.ToArray());
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message),
                    ex);
            }
        }

        public static ISelector CreateMultiSelector(LambdaExpression expression, List<IMap> maps)
        {
            try
            {
                if (expression.Parameters.Count <= 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new MultiParameterLambdaState(expression.Parameters, maps)
                {
                    Mode = LambdaMode.OutputMode
                };
                var bodyExpression = expression.Body;
                if (bodyExpression is MemberInitExpression || bodyExpression is NewExpression ||
                    bodyExpression is ParameterExpression)
                {
                    ISelector selector;
                    var list = new List<string>();
                    if (ParseNewArguments(bodyExpression, state, ref list))
                    {
                        selector = state.CreateSelector(list.ToArray());
                    }
                    else
                    {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                    }

                    return selector;
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message),
                    ex);
            }
        }

        public static InsertSelector CreateMultiInsertSelector(LambdaExpression expression, List<IMap> maps)
        {
            try
            {
                if (expression.Parameters.Count <= 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new MultiParameterLambdaState(expression.Parameters, maps)
                {
                    Mode = LambdaMode.OutputMode
                };
                if (expression.Body is MemberInitExpression memberInitObj)
                {
                    var insertMapping = DataEntityMapping.GetTableMapping(memberInitObj.Type);
                    var map = insertMapping.GetRelationMap();
                    var selector = new InsertSelector(insertMapping);
                    foreach (var binding in memberInitObj.Bindings)
                    {
                        if (binding is MemberAssignment ass)
                        {
                            var innerExpression = ass.Expression;
                            if (!ParseDataFieldInfo(innerExpression, state, out var selectField))
                            {
                                var obj = ConvertObject(innerExpression);
                                selectField = new LightConstantDataFieldInfo(obj);
                            }

                            var path = "." + ass.Member.Name;
                            var insertField = map.GetFieldInfoForPath(path);

                            selector.SetInsertField(insertField);
                            selector.SetSelectField(selectField);
                        }
                        else
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionMemberError, binding.Member);
                        }
                    }

                    return selector;
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message),
                    ex);
            }
        }

        public static OrderExpression ResolveLambdaOrderByExpression(LambdaExpression expression, OrderType orderType)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);
                if (ParseDataFieldInfo(expression.Body, state, out var dataFieldInfo))
                {
                    CheckFieldInfo(dataFieldInfo);
                    OrderExpression exp = new DataFieldOrderExpression(dataFieldInfo, orderType);
                    exp.MultiOrder = true;
                    return exp;
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "order", expression, ex.Message),
                    ex);
            }
        }

        public static OrderExpression ResolveLambdaMultiOrderByExpression(LambdaExpression expression,
            OrderType orderType, List<IMap> maps)
        {
            try
            {
                if (expression.Parameters.Count <= 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new MultiParameterLambdaState(expression.Parameters, maps);
                if (ParseDataFieldInfo(expression.Body, state, out var dataFieldInfo))
                {
                    CheckFieldInfo(dataFieldInfo);
                    OrderExpression exp = new DataFieldOrderExpression(dataFieldInfo, orderType);
                    exp.MultiOrder = true;
                    return exp;
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "order", expression, ex.Message),
                    ex);
            }
        }

        public static QueryExpression ResolveLambdaQueryExpression(LambdaExpression expression)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);
                var query = ResolveQueryExpression(expression.Body, state);
                query.MultiQuery = state.MultiEntity;
                return query;
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "query", expression, ex.Message),
                    ex);
            }
        }

        public static QueryExpression ResolveLambdaMultiQueryExpression(LambdaExpression expression, List<IMap> maps)
        {
            try
            {
                if (expression.Parameters.Count <= 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new MultiParameterLambdaState(expression.Parameters, maps);
                var query = ResolveQueryExpression(expression.Body, state);
                query.MultiQuery = state.MultiEntity;
                return query;
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "query", expression, ex.Message),
                    ex);
            }
        }

        public static QueryExpression ResolveLambdaHavingExpression(LambdaExpression expression, AggregateModel model)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new AggregateLambdaState(expression.Parameters[0], model);
                var query = ResolveQueryExpression(expression.Body, state);
                return query;
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "having", expression, ex.Message),
                    ex);
            }
        }

        public static OrderExpression ResolveLambdaAggregateOrderByExpression(LambdaExpression expression,
            OrderType orderType, AggregateModel model)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new AggregateLambdaState(expression.Parameters[0], model);
                if (ParseDataFieldInfo(expression.Body, state, out var dataFieldInfo))
                {
                    CheckFieldInfo(dataFieldInfo);
                    OrderExpression exp = new DataFieldOrderExpression(dataFieldInfo, orderType);
                    return exp;
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
            }
            catch (Exception ex)
            {
                throw new LightDataException(
                    string.Format(SR.ParseExpressionError, "aggregate order", expression, ex.Message), ex);
            }
        }

        public static DataFieldExpression ResolveLambdaOnExpression(LambdaExpression expression, List<IMap> maps)
        {
            try
            {
                if (expression.Parameters.Count <= 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new MultiParameterLambdaState(expression.Parameters, maps);
                var on = ResolveOnExpression(expression.Body, state);
                return on;
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "on", expression, ex.Message), ex);
            }
        }

        public static InsertSelector CreateInsertSelector(LambdaExpression expression)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                var state = new SingleParameterLambdaState(expression.Parameters[0])
                {
                    Mode = LambdaMode.OutputMode
                };
                if (expression.Body is MemberInitExpression memberInitObj)
                {
                    var insertMapping = DataEntityMapping.GetTableMapping(memberInitObj.Type);
                    var map = insertMapping.GetRelationMap();
                    var selector = new InsertSelector(insertMapping, state.MainMapping);
                    foreach (var binding in memberInitObj.Bindings)
                    {
                        if (binding is MemberAssignment ass)
                        {
                            var innerExpression = ass.Expression;
                            if (!ParseDataFieldInfo(innerExpression, state, out var selectField))
                            {
                                var obj = ConvertObject(innerExpression);
                                selectField = new LightConstantDataFieldInfo(obj);
                            }

                            var path = "." + ass.Member.Name;
                            var insertField = map.GetFieldInfoForPath(path);
                            selector.SetInsertField(insertField);
                            selector.SetSelectField(selectField);
                        }
                        else
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionMemberError, binding.Member);
                        }
                    }

                    return selector;
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "insert", expression, ex.Message),
                    ex);
            }
        }

        public static InsertSelector CreateAggregateInsertSelector(LambdaExpression expression, AggregateModel model)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                LambdaState state = new AggregateLambdaState(expression.Parameters[0], model)
                {
                    Mode = LambdaMode.OutputMode
                };
                if (expression.Body is MemberInitExpression memberInitObj)
                {
                    var insertMapping = DataEntityMapping.GetTableMapping(memberInitObj.Type);
                    var map = insertMapping.GetRelationMap();
                    var selector = new InsertSelector(insertMapping);
                    foreach (var binding in memberInitObj.Bindings)
                    {
                        if (binding is MemberAssignment ass)
                        {
                            var innerExpression = ass.Expression;
                            if (!ParseDataFieldInfo(innerExpression, state, out var selectField))
                            {
                                var obj = ConvertObject(innerExpression);
                                selectField = new LightConstantDataFieldInfo(obj);
                            }
                            else
                            {
                                selectField = new DataFieldInfo(selectField.TableMapping, true, selectField.FieldName);
                            }

                            var path = "." + ass.Member.Name;
                            var insertField = map.GetFieldInfoForPath(path);

                            selector.SetInsertField(insertField);
                            selector.SetSelectField(selectField);
                        }
                        else
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionMemberError, binding.Member);
                        }
                    }

                    return selector;
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
            }
            catch (Exception ex)
            {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message),
                    ex);
            }
        }

        public static MassUpdator CreateMassUpdateExpression(LambdaExpression expression)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                var state = new SingleParameterLambdaState(expression.Parameters[0])
                {
                    Mode = LambdaMode.OutputMode
                };

                if (expression.Body is MemberInitExpression memberInitObj)
                {
                    var updateMapping = DataEntityMapping.GetTableMapping(memberInitObj.Type);
                    var map = updateMapping.GetRelationMap();
                    var updator = new MassUpdator(updateMapping);
                    foreach (var binding in memberInitObj.Bindings)
                    {
                        if (binding is MemberAssignment ass)
                        {
                            var innerExpression = ass.Expression;
                            if (!ParseDataFieldInfo(innerExpression, state, out var valueField))
                            {
                                var obj = ConvertObject(innerExpression);
                                valueField = new LightConstantDataFieldInfo(obj);
                            }
                            else if (state.MultiEntity)
                            {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                            }

                            var path = "." + ass.Member.Name;
                            var keyField = map.GetFieldInfoForPath(path);
                            updator.SetUpdateData(keyField, valueField);
                        }
                        else
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionMemberError, binding.Member);
                        }
                    }

                    return updator;
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
            }
            catch (Exception ex)
            {
                throw new LightDataException(
                    string.Format(SR.ParseExpressionError, "update expression", expression, ex.Message), ex);
            }
        }

        public static DataFieldInfo ResolveSingleField(LambdaExpression expression)
        {
            try
            {
                if (expression.Parameters.Count != 1)
                {
                    throw new LambdaParseException(LambdaParseMessage.ParameterCountError);
                }

                var state = new SingleParameterLambdaState(expression.Parameters[0]);
                if (ParseDataFieldInfo(expression.Body, state, out var fieldInfo))
                {
                    if (state.MultiEntity)
                    {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                    }

                    return fieldInfo;
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
            }
            catch (Exception ex)
            {
                throw new LightDataException(
                    string.Format(SR.ParseExpressionError, "single field", expression, ex.Message), ex);
            }
        }

        private static bool ParseNewArguments(Expression expression, LambdaState state, ref List<string> pathList)
        {
            if (expression is ParameterExpression paramObj)
            {
                pathList.Add(paramObj.Name);
                return true;
            }

            if (expression is MemberExpression memberObj)
            {
                if (memberObj.Expression != null)
                {
                    if (memberObj.Expression is ParameterExpression param)
                    {
                        if (state.CheckParameter(param.Name, param.Type))
                        {
                            var fullPath = memberObj.ToString();
                            var pathType = state.ParsePath(fullPath);
                            switch (pathType)
                            {
                                case LambdaPathType.Field:
                                case LambdaPathType.RelateEntity:
                                case LambdaPathType.RelateCollection:
                                    pathList.Add(fullPath);
                                    return true;
                                case LambdaPathType.None:
                                    break;
                                default:
                                    throw new LambdaParseException(LambdaParseMessage.ExpressionMemberInvalid);
                            }
                        }
                        else
                        {
                            throw new LambdaParseException(LambdaParseMessage.ParameterTypeError, param.Name,
                                param.Type);
                        }
                    }

                    if (ParseNewArguments(memberObj.Expression, state, ref pathList))
                    {
                        if (memberObj.Expression is MemberExpression)
                        {
                            // 子表字段判断
                            var fullPath = memberObj.ToString();
                            var pathType = state.ParsePath(fullPath);
                            if (pathType == LambdaPathType.Field || pathType == LambdaPathType.RelateEntity ||
                                pathType == LambdaPathType.RelateCollection)
                            {
                                pathList.RemoveAt(pathList.Count - 1);
                                pathList.Add(fullPath);
                            }
                        }

                        return true;
                    }
                }

                return false;
            }

            if (expression is MethodCallExpression methodCallObj)
            {
                var args = false;
                foreach (var arg in methodCallObj.Arguments)
                {
                    args |= ParseNewArguments(arg, state, ref pathList);
                }

                var obj = false;
                if (methodCallObj.Object != null)
                {
                    obj = ParseNewArguments(methodCallObj.Object, state, ref pathList);
                }

                return args || obj;
            }

            if (expression is NewArrayExpression newArrayObj)
            {
                var ret = false;
                foreach (var arg in newArrayObj.Expressions)
                {
                    ret |= ParseNewArguments(arg, state, ref pathList);
                }

                return ret;
            }

            if (expression is MemberInitExpression memberInitObj)
            {
                var bindings = false;
                if (memberInitObj.Bindings.Count > 0)
                {
                    foreach (var memberBinding in memberInitObj.Bindings)
                    {
                        if (memberBinding is MemberAssignment ass)
                        {
                            bindings |= ParseNewArguments(ass.Expression, state, ref pathList);
                        }
                    }
                }

                var newExp = ParseNewArguments(memberInitObj.NewExpression, state, ref pathList);

                return bindings || newExp;
            }

            if (expression is NewExpression newObj)
            {
                var ret = false;
                if (newObj.Arguments.Count > 0)
                {
                    foreach (var arg in newObj.Arguments)
                    {
                        ret |= ParseNewArguments(arg, state, ref pathList);
                    }
                }

                return ret;
            }

            if (expression is BinaryExpression binaryObj)
            {
                var left = ParseNewArguments(binaryObj.Left, state, ref pathList);
                var right = ParseNewArguments(binaryObj.Right, state, ref pathList);

                return left || right;
            }

            if (expression is IndexExpression indexObj)
            {
                var args = false;
                foreach (var arg in indexObj.Arguments)
                {
                    args |= ParseNewArguments(arg, state, ref pathList);
                }

                var index = false;
                if (indexObj.Object != null)
                {
                    index = ParseNewArguments(indexObj.Object, state, ref pathList);
                }

                return args || index;
            }

            if (expression is DynamicExpression dynamicObj)
            {
                var ret = false;
                foreach (var arg in dynamicObj.Arguments)
                {
                    ret |= ParseNewArguments(arg, state, ref pathList);
                }

                return ret;
            }

            if (expression is ConditionalExpression conditionObj)
            {
                var test = ParseNewArguments(conditionObj.Test, state, ref pathList);
                var ifTrue = ParseNewArguments(conditionObj.IfTrue, state, ref pathList);
                var ifFalse = ParseNewArguments(conditionObj.IfFalse, state, ref pathList);

                return test || ifTrue || ifFalse;
            }

            if (expression is ListInitExpression listInitObj)
            {
                var args = false;
                foreach (var init in listInitObj.Initializers)
                {
                    foreach (var arg in init.Arguments)
                    {
                        args |= ParseNewArguments(arg, state, ref pathList);
                    }
                }

                var newExp = ParseNewArguments(listInitObj.NewExpression, state, ref pathList);

                return args || newExp;
            }

            if (expression is UnaryExpression unaryObj)
            {
                return ParseNewArguments(unaryObj.Operand, state, ref pathList);
            }

            if (expression is TypeBinaryExpression typeBinaryObj)
            {
                return ParseNewArguments(typeBinaryObj.Expression, state, ref pathList);
            }

            if (expression is ConstantExpression || expression is DefaultExpression)
            {
                return false;
            }

            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
        }

        private static void CheckFieldInfo(DataFieldInfo fieldInfo)
        {
            if (Equals(fieldInfo, null))
            {
                throw new LambdaParseException(LambdaParseMessage.ExpressionParseFieldFailed);
            }
        }

        private static SelectModel ParseSelectModel(MemberInitExpression expression, SingleParameterLambdaState state)
        {
            var entityMapping = state.MainMapping;
            var customMapping = SpecifiedDataMapping.GetMapping(expression.Type);
            var model = new SelectModel(entityMapping, customMapping);
            foreach (var binding in expression.Bindings)
            {
                if (binding is MemberAssignment ass)
                {
                    var innerExpression = ass.Expression;
                    if (ParseDataFieldInfo(innerExpression, state, out var fieldInfo))
                    {
                        if (state.MultiEntity)
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                        }

                        if (fieldInfo is LightAggregateDataFieldInfo)
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportAggregateField);
                        }

                        model.AddSelectField(ass.Member.Name, fieldInfo);
                    }
                    else
                    {
                        var obj = ConvertObject(innerExpression);
                        fieldInfo = new LightConstantDataFieldInfo(obj);
                        model.AddSelectField(ass.Member.Name, fieldInfo);
                    }
                }
                else
                {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionMemberError, binding.Member);
                }
            }

            return model;
        }

        private static SelectModel ParseSelectModel(NewExpression expression, SingleParameterLambdaState state)
        {
            var entityMapping = state.MainMapping;
            var customMapping = DynamicDataMapping.GetMapping(expression.Type);
            var model = new SelectModel(entityMapping, customMapping);
            if (expression.Arguments.Count > 0)
            {
                var index = 0;
                foreach (var arg in expression.Arguments)
                {
                    var member = expression.Members[index];
                    var innerExpression = arg;
                    if (ParseDataFieldInfo(innerExpression, state, out var fieldInfo))
                    {
                        if (state.MultiEntity)
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                        }

                        if (fieldInfo is LightAggregateDataFieldInfo)
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportAggregateField);
                        }

                        model.AddSelectField(member.Name, fieldInfo);
                        index++;
                    }
                    else
                    {
                        var obj = ConvertObject(innerExpression);
                        fieldInfo = new LightConstantDataFieldInfo(obj);
                        model.AddSelectField(member.Name, fieldInfo);
                    }
                }
            }
            else
            {
                throw new LambdaParseException(LambdaParseMessage.ExpressionNoArguments);
            }

            return model;
        }

        private static AggregateModel ParseAggregateModel(MemberInitExpression expression,
            SingleParameterLambdaState state)
        {
            var entityMapping = state.MainMapping;
            var aggregateMapping = SpecifiedDataMapping.GetMapping(expression.Type);
            var model = new AggregateModel(entityMapping, aggregateMapping);
            if (expression.Bindings.Count > 0)
            {
                var hasAggregateField = false;
                foreach (var binding in expression.Bindings)
                {
                    if (binding is MemberAssignment ass)
                    {
                        var innerExpression = ass.Expression;
                        if (ParseDataFieldInfo(innerExpression, state, out var fieldInfo))
                        {
                            if (state.MultiEntity)
                            {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                            }

                            if (fieldInfo is LightAggregateDataFieldInfo)
                            {
                                hasAggregateField = true;
                                model.AddAggregateField(ass.Member.Name, fieldInfo);
                            }
                            else
                            {
                                model.AddGroupByField(ass.Member.Name, fieldInfo);
                            }
                        }
                        else
                        {
                            var obj = ConvertObject(innerExpression);
                            fieldInfo = new LightConstantDataFieldInfo(obj);
                            model.AddAggregateField(ass.Member.Name, fieldInfo);
                        }
                    }
                    else
                    {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionMemberError, binding.Member);
                    }
                }

                if (!hasAggregateField)
                {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainAggregateFunction);
                }
            }
            else
            {
                throw new LambdaParseException(LambdaParseMessage.ExpressionNoMember);
            }

            return model;
        }

        private static AggregateModel ParseAggregateModel(NewExpression expression, SingleParameterLambdaState state)
        {
            var entityMapping = state.MainMapping;
            var aggregateMapping = DynamicDataMapping.GetMapping(expression.Type);
            var model = new AggregateModel(entityMapping, aggregateMapping);
            if (expression.Arguments.Count > 0)
            {
                var index = 0;
                var hasAggregateField = false;
                foreach (var arg in expression.Arguments)
                {
                    var member = expression.Members[index];
                    var innerExpression = arg;
                    if (ParseDataFieldInfo(innerExpression, state, out var fieldInfo))
                    {
                        if (state.MultiEntity)
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                        }

                        if (fieldInfo is LightAggregateDataFieldInfo)
                        {
                            hasAggregateField = true;
                            model.AddAggregateField(member.Name, fieldInfo);
                        }
                        else
                        {
                            model.AddGroupByField(member.Name, fieldInfo);
                        }
                    }
                    else
                    {
                        var obj = ConvertObject(innerExpression);
                        fieldInfo = new LightConstantDataFieldInfo(obj);
                        model.AddAggregateField(member.Name, fieldInfo);
                    }

                    index++;
                }

                if (!hasAggregateField)
                {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainAggregateFunction);
                }
            }
            else
            {
                throw new LambdaParseException(LambdaParseMessage.ExpressionNoArguments);
            }

            return model;
        }

        private static bool ParseDataFieldInfo(Expression expression, LambdaState state, out DataFieldInfo fieldInfo)
        {
            fieldInfo = null;
            if (expression.NodeType == ExpressionType.Constant)
            {
                return false;
            }

            if (expression is BinaryExpression binaryObj)
            {
                object leftValue = null;
                var left = ParseDataFieldInfo(binaryObj.Left, state, out var leftFieldInfo);
                if (left)
                {
                    CheckFieldInfo(leftFieldInfo);
                }
                else
                {
                    var obj = ConvertObject(binaryObj.Left);
                    leftValue = new LightConstantDataFieldInfo(obj);
                }

                object rightValue = null;
                var right = ParseDataFieldInfo(binaryObj.Right, state, out var rightFieldInfo);
                if (right)
                {
                    CheckFieldInfo(rightFieldInfo);
                }
                else
                {
                    var obj = ConvertObject(binaryObj.Right);
                    rightValue = new LightConstantDataFieldInfo(obj);
                }

                if (!left && !right)
                {
                    return false;
                }

                // 字符串连接
                if (binaryObj.Method != null && binaryObj.NodeType == ExpressionType.Add &&
                    binaryObj.Method.DeclaringType == typeof(string) && binaryObj.Method.Name == "Concat")
                {
                    if (left && right)
                    {
                        fieldInfo = new LightStringConcatDataFieldInfo(leftFieldInfo.TableMapping, leftFieldInfo,
                            rightFieldInfo);
                        return true;
                    }

                    if (left && !right)
                    {
                        fieldInfo = new LightStringConcatDataFieldInfo(leftFieldInfo.TableMapping, leftFieldInfo,
                            rightValue);
                        return true;
                    }

                    if (!left && right)
                    {
                        fieldInfo = new LightStringConcatDataFieldInfo(rightFieldInfo.TableMapping, leftValue,
                            rightFieldInfo);
                        return true;
                    }
                }

                // 数字数学运算
                if (CheckMathOperator(binaryObj.NodeType, out var mathOperator))
                {
                    if (left && right)
                    {
                        fieldInfo = new LightMathCalculateDataFieldInfo(leftFieldInfo.TableMapping, mathOperator,
                            leftFieldInfo, rightFieldInfo);
                        return true;
                    }

                    if (left && !right)
                    {
                        fieldInfo = new LightMathCalculateDataFieldInfo(leftFieldInfo.TableMapping, mathOperator,
                            leftFieldInfo, rightValue);
                        return true;
                    }

                    if (!left && right)
                    {
                        fieldInfo = new LightMathCalculateDataFieldInfo(rightFieldInfo.TableMapping, mathOperator,
                            leftValue, rightFieldInfo);
                        return true;
                    }
                }

                // 字段比较
                if (CheckQueryPredicate(binaryObj.NodeType, out var queryPredicate))
                {
                    QueryExpression queryExpression;
                    if (left && right)
                    {
                        queryExpression = new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate,
                            leftFieldInfo, rightFieldInfo);
                        fieldInfo = new LightQueryDataFieldInfo(queryExpression);
                        return true;
                    }

                    if (left && !right)
                    {
                        queryExpression = new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate,
                            leftFieldInfo, rightValue);
                        fieldInfo = new LightQueryDataFieldInfo(queryExpression);
                        return true;
                    }

                    if (!left && right)
                    {
                        queryExpression = new LightBinaryQueryExpression(rightFieldInfo.TableMapping,
                            queryPredicate, rightFieldInfo, leftValue);
                        fieldInfo = new LightQueryDataFieldInfo(queryExpression);
                        return true;
                    }
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionNodeTypeUnsupport, binaryObj.NodeType);
            }

            if (expression is UnaryExpression unaryObj)
            {
                if (unaryObj.NodeType == ExpressionType.Not)
                {
                    if (ParseDataFieldInfo(unaryObj.Operand, state, out var inFieldInfo))
                    {
                        CheckFieldInfo(inFieldInfo);
                    }
                    else
                    {
                        return false;
                    }

                    if (inFieldInfo is ISupportNotDefine notDefine)
                    {
                        notDefine.SetNot();
                        fieldInfo = inFieldInfo;
                    }
                    else
                    {
                        fieldInfo = new LightNotDataFieldInfo(inFieldInfo, state.Mode == LambdaMode.QueryMode);
                    }

                    return true;
                }

                if (unaryObj.NodeType == ExpressionType.Convert)
                {
                    if (ParseDataFieldInfo(unaryObj.Operand, state, out var convertFieldInfo))
                    {
                        CheckFieldInfo(convertFieldInfo);
                        fieldInfo = convertFieldInfo;
                        return true;
                    }

                    return false;
                }

                if (unaryObj.NodeType == ExpressionType.TypeAs)
                {
                    if (unaryObj.Type.GetTypeInfo().IsGenericType)
                    {
                        var frameType = unaryObj.Type.GetGenericTypeDefinition();
                        if (frameType.FullName != "System.Nullable`1")
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                        }
                    }

                    if (ParseDataFieldInfo(unaryObj.Operand, state, out var typeFieldInfo))
                    {
                        CheckFieldInfo(typeFieldInfo);
                        fieldInfo = typeFieldInfo;
                        return true;
                    }

                    return false;
                }
            }

            if (expression is MemberExpression memberObj)
            {
                if (memberObj.Expression != null)
                {
                    if (memberObj.Expression is ParameterExpression param)
                    {
                        if (state.CheckParameter(param.Name, param.Type))
                        {
                            var fullPath = memberObj.ToString();
                            var pathType = state.ParsePath(fullPath);
                            switch (pathType)
                            {
                                case LambdaPathType.Field:
                                {
                                    var myInfo = state.GetDataFieldInfo(fullPath);
                                    fieldInfo = myInfo;
                                    return true;
                                }
                                case LambdaPathType.RelateEntity:
                                    return true;
                                default:
                                    throw new LambdaParseException(LambdaParseMessage.ExpressionMemberInvalid);
                            }
                        }

                        throw new LambdaParseException(LambdaParseMessage.ParameterTypeError, param.Name,
                            param.Type);
                    }

                    if (memberObj.Expression is UnaryExpression unary && unary.NodeType == ExpressionType.Convert &&
                        unary.Operand is ParameterExpression param1)
                    {
                        if (state.CheckParameter(param1.Name, param1.Type))
                        {
                            var fullPath = memberObj.ToString().Replace(unary.ToString(), param1.ToString());
                            var pathType = state.ParsePath(fullPath);
                            switch (pathType)
                            {
                                case LambdaPathType.Field:
                                {
                                    var myInfo = state.GetDataFieldInfo(fullPath);
                                    fieldInfo = myInfo;
                                    return true;
                                }
                                case LambdaPathType.RelateEntity:
                                    return true;
                                default:
                                    throw new LambdaParseException(LambdaParseMessage.ExpressionMemberInvalid);
                            }
                        }

                        throw new LambdaParseException(LambdaParseMessage.ParameterTypeError, param1.Name,
                            param1.Type);
                    }

                    if (ParseDataFieldInfo(memberObj.Expression, state, out fieldInfo))
                    {
                        if (Equals(fieldInfo, null))
                        {
                            var fullPath = memberObj.ToString();
                            var pathType = state.ParsePath(fullPath);
                            switch (pathType)
                            {
                                case LambdaPathType.Field:
                                {
                                    var myInfo = state.GetDataFieldInfo(fullPath);
                                    fieldInfo = myInfo;
                                    state.MultiEntity = true;
                                    return true;
                                }
                                case LambdaPathType.RelateEntity:
                                    return true;
                                default:
                                    throw new LambdaParseException(LambdaParseMessage.ExpressionMemberInvalid);
                            }
                        }

                        if (memberObj.Expression.Type.GetTypeInfo().IsGenericType)
                        {
                            var frameType = memberObj.Expression.Type.GetGenericTypeDefinition();
                            if (frameType.FullName == "System.Nullable`1")
                            {
                                switch (memberObj.Member.Name)
                                {
                                    case "Value":
                                        return true;
                                    case "HasValue":
                                        fieldInfo = new LightNullDataFieldInfo(fieldInfo, false);
                                        return true;
                                }
                            }
                        }
                        else
                        {
                            if (memberObj.Expression.Type == typeof(DateTime))
                            {
                                fieldInfo = CreateDateDataFieldInfo(memberObj.Member, fieldInfo);
                                return true;
                            }

                            if (memberObj.Expression.Type == typeof(string))
                            {
                                fieldInfo = CreateStringMemberDataFieldInfo(memberObj.Member, fieldInfo);
                                return true;
                            }
                        }

                        throw new LambdaParseException(LambdaParseMessage.MemberExpressionTypeUnsupport,
                            memberObj.Expression.Type);
                    }
                }

                return false;
            }

            if (expression is MethodCallExpression methodCallObj)
            {
                var methodInfo = methodCallObj.Method;
                if ((methodInfo.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
                {
                    if (methodInfo.DeclaringType == typeof(Function))
                    {
                        fieldInfo = ParseAggregateData(methodCallObj, state);
                        return true;
                    }

                    if (methodInfo.DeclaringType == typeof(ExtendQuery))
                    {
                        fieldInfo = ParseExtendQueryData(methodCallObj, state);
                        return true;
                    }

                    if (ParseMethodArguments(methodCallObj.Arguments, state, out var argObjects,
                        out var mainFieldInfo))
                    {
                        CheckFieldInfo(mainFieldInfo);
                        if (methodInfo.DeclaringType == typeof(Math))
                        {
                            fieldInfo = ParseMathFunctionDataFieldInfo(methodInfo, mainFieldInfo, argObjects);
                            return true;
                        }

                        if (methodInfo.DeclaringType == typeof(string))
                        {
                            fieldInfo = ParseStaticStringFunctionDataFieldInfo(methodInfo, mainFieldInfo, argObjects);
                            return true;
                        }

                        if (methodInfo.DeclaringType == typeof(Convert))
                        {
                            fieldInfo = mainFieldInfo;
                            return true;
                        }

                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionTypeUnsupport,
                            methodInfo.DeclaringType);
                    }

                    return false;
                }
                else
                {
                    DataFieldInfo mainFieldInfo = null;
                    object callObject;
                    if (ParseDataFieldInfo(methodCallObj.Object, state, out var callFieldInfo))
                    {
                        CheckFieldInfo(callFieldInfo);
                        mainFieldInfo = callFieldInfo;
                        callObject = callFieldInfo;
                    }
                    else
                    {
                        callObject = ConvertObject(methodCallObj.Object);
                    }

                    if (ParseMethodArguments(methodCallObj.Arguments, state, out var argObjects,
                        out var mainArgFieldInfo))
                    {
                        if (Equals(mainFieldInfo, null))
                        {
                            mainFieldInfo = mainArgFieldInfo;
                        }
                    }

                    if (Equals(mainFieldInfo, null))
                    {
                        return false;
                    }

                    if (methodCallObj.Object != null && methodCallObj.Object.Type == typeof(string))
                    {
                        fieldInfo = ParseInstanceStringFunctionDataFieldInfo(methodInfo, mainFieldInfo, callObject,
                            argObjects);
                        return true;
                    }

                    if (methodCallObj.Object != null && methodCallObj.Object.Type == typeof(DateTime))
                    {
                        fieldInfo = ParseInstanceDateTimeFunctionDataFieldInfo(methodInfo, mainFieldInfo,
                            argObjects);
                        return true;
                    }

                    if (Equals(callFieldInfo, null) && argObjects != null && argObjects.Length == 1 &&
                        methodInfo.Name == "Contains" && EnumerableTypeInfo.IsAssignableFrom(methodInfo.DeclaringType))
                    {
                        fieldInfo = ParseContainsDataFieldInfo(mainFieldInfo, callObject);
                        return true;
                    }
                }
            }

            // if (expression is NewArrayExpression newArrayObj)
            // {
            //     if (ParseMethodArguments(newArrayObj.Expressions, state, out var argsObjects,
            //         out var arrayFieldInfo))
            //     {
            //         fieldInfo = new LightNewArrayDataFieldInfo(arrayFieldInfo.TableMapping, argsObjects);
            //         return true;
            //     }
            //
            //     return false;
            // }

            if (expression is ConditionalExpression conditionObj)
            {
                if (!TryParseQueryExpression(conditionObj.Test, state, out var query))
                {
                    var value = ConvertObject(conditionObj.Test);
                    query = new LightConstantQueryExpression(value);
                }

                object ifTrueValue;
                if (ParseDataFieldInfo(conditionObj.IfTrue, state, out var ifTrueFieldInfo))
                {
                    CheckFieldInfo(ifTrueFieldInfo);
                    ifTrueValue = ifTrueFieldInfo;
                }
                else
                {
                    ifTrueValue = ConvertObject(conditionObj.IfTrue);
                }

                object ifFalseValue;
                if (ParseDataFieldInfo(conditionObj.IfFalse, state, out var ifFalseFieldInfo))
                {
                    CheckFieldInfo(ifFalseFieldInfo);
                    ifFalseValue = ifFalseFieldInfo;
                }
                else
                {
                    ifFalseValue = ConvertObject(conditionObj.IfFalse);
                }

                fieldInfo = new LightConditionDataFieldInfo(query, ifTrueValue, ifFalseValue);
                return true;
            }

            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
        }

        private static bool ParseMethodArguments(IReadOnlyList<Expression> arguments, LambdaState state,
            out object[] argObjects, out DataFieldInfo fieldInfo)
        {
            fieldInfo = null;
            argObjects = null;
            if (arguments.Count == 0)
            {
                return false;
            }

            var array = new object[arguments.Count];
            var hasFieldInfo = false;
            for (var i = 0; i < arguments.Count; i++)
            {
                var arg = arguments[i];
                if (ParseDataFieldInfo(arg, state, out var argFieldInfo))
                {
                    CheckFieldInfo(argFieldInfo);
                    hasFieldInfo = true;
                    if (Equals(fieldInfo, null))
                    {
                        fieldInfo = argFieldInfo;
                    }

                    array[i] = argFieldInfo;
                }
                else
                {
                    array[i] = ConvertObject(arg);
                }
            }

            if (!hasFieldInfo)
            {
                argObjects = array;
                return false;
            }

            argObjects = array;
            return true;
        }

        private static bool CheckConcatOperatorsType(ExpressionType expressionType, out ConcatOperatorType concatType)
        {
            if (expressionType == ExpressionType.And || expressionType == ExpressionType.AndAlso)
            {
                concatType = ConcatOperatorType.AND;
                return true;
            }

            if (expressionType == ExpressionType.Or || expressionType == ExpressionType.OrElse)
            {
                concatType = ConcatOperatorType.OR;
                return true;
            }

            concatType = ConcatOperatorType.AND;
            return false;
        }

        private static bool CheckQueryPredicate(ExpressionType expressionType, out QueryPredicate queryPredicate)
        {
            bool ret;
            switch (expressionType)
            {
                case ExpressionType.Equal:
                    queryPredicate = QueryPredicate.Eq;
                    ret = true;
                    break;
                case ExpressionType.NotEqual:
                    queryPredicate = QueryPredicate.NotEq;
                    ret = true;
                    break;
                case ExpressionType.GreaterThan:
                    queryPredicate = QueryPredicate.Gt;
                    ret = true;
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    queryPredicate = QueryPredicate.GtEq;
                    ret = true;
                    break;
                case ExpressionType.LessThan:
                    queryPredicate = QueryPredicate.Lt;
                    ret = true;
                    break;
                case ExpressionType.LessThanOrEqual:
                    queryPredicate = QueryPredicate.LtEq;
                    ret = true;
                    break;
                default:
                    queryPredicate = QueryPredicate.Eq;
                    ret = false;
                    break;
            }

            return ret;
        }

        private static bool CheckMathOperator(ExpressionType expressionType, out MathOperator mathOperator)
        {
            bool ret;
            switch (expressionType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    mathOperator = MathOperator.Puls;
                    ret = true;
                    break;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    mathOperator = MathOperator.Minus;
                    ret = true;
                    break;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    mathOperator = MathOperator.Multiply;
                    ret = true;
                    break;
                case ExpressionType.Divide:
                    mathOperator = MathOperator.Divided;
                    ret = true;
                    break;
                case ExpressionType.Modulo:
                    mathOperator = MathOperator.Mod;
                    ret = true;
                    break;
                case ExpressionType.Power:
                case ExpressionType.ExclusiveOr:
                    mathOperator = MathOperator.Power;
                    ret = true;
                    break;
                default:
                    mathOperator = MathOperator.Puls;
                    ret = false;
                    break;
            }

            return ret;
        }

        private static DataFieldInfo CreateDateDataFieldInfo(MemberInfo member, DataFieldInfo fieldInfo)
        {
            switch (member.Name)
            {
                case "Date":
                    fieldInfo = new LightDateDataFieldInfo(fieldInfo);
                    break;
                default:
                    if (Enum.TryParse<DatePart>(member.Name, out var datePart))
                    {
                        fieldInfo = new LightDatePartDataFieldInfo(fieldInfo, datePart);
                    }
                    else
                    {
                        throw new LambdaParseException(LambdaParseMessage.MemberExpressionMemberUnsupport, "DateTime",
                            member.Name);
                    }

                    break;
            }

            return fieldInfo;
        }

        private static DataFieldInfo CreateStringMemberDataFieldInfo(MemberInfo member, DataFieldInfo fieldInfo)
        {
            switch (member.Name)
            {
                case "Length":
                    fieldInfo = new LightStringLengthDataFieldInfo(fieldInfo);
                    break;
                default:
                    throw new LambdaParseException(LambdaParseMessage.MemberExpressionMemberUnsupport, "string",
                        member.Name);
            }

            return fieldInfo;
        }

        private static DataFieldInfo ParseMathFunctionDataFieldInfo(MethodInfo method, DataFieldInfo mainFieldInfo,
            object[] argObjects)
        {
            var parameterInfos = method.GetParameters();
            if (Enum.TryParse(method.Name, out MathFunction mathFunction))
            {
                if (parameterInfos.Length == 0)
                {
                    throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "Math",
                        method.Name);
                }

                if (mathFunction == MathFunction.Atan2 || mathFunction == MathFunction.Max ||
                    mathFunction == MathFunction.Min || mathFunction == MathFunction.Pow)
                {
                    if (parameterInfos.Length != 2)
                    {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "Math",
                            method.Name);
                    }
                }
                else if (mathFunction == MathFunction.Log || mathFunction == MathFunction.Round)
                {
                    if (parameterInfos.Length > 2)
                    {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "Math",
                            method.Name);
                    }
                }

                return new LightMathFunctionDataFieldInfo(mainFieldInfo.TableMapping, mathFunction, argObjects);
            }

            throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport, "Math", method.Name);
        }

        private static DataFieldInfo ParseStaticStringFunctionDataFieldInfo(MethodInfo method,
            DataFieldInfo mainFieldInfo, object[] argObjects)
        {
            if (method.Name == "Concat")
            {
                if (argObjects.Length == 1)
                {
                    if (argObjects[0] is LightNewArrayDataFieldInfo newArray)
                    {
                        return new LightStringConcatDataFieldInfo(newArray.TableMapping, newArray.Values);
                    }

                    return new LightStringConcatDataFieldInfo(mainFieldInfo.TableMapping, argObjects);
                }

                return new LightStringConcatDataFieldInfo(mainFieldInfo.TableMapping, argObjects);
            }

            throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport, "string", method.Name);
        }

        private static DataFieldInfo ParseInstanceStringFunctionDataFieldInfo(MethodInfo method,
            DataFieldInfo mainFieldInfo, object callObject, object[] argObjects)
        {
            var parameterInfos = method.GetParameters();
            switch (method.Name)
            {
                case "StartsWith":
                    if (parameterInfos.Length != 1)
                    {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string",
                            method.Name);
                    }

                    return new LightStringMatchDataFieldInfo(mainFieldInfo.TableMapping, false, true, callObject,
                        argObjects[0]);
                case "EndsWith":
                    if (parameterInfos.Length != 1)
                    {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string",
                            method.Name);
                    }

                    return new LightStringMatchDataFieldInfo(mainFieldInfo.TableMapping, true, false, callObject,
                        argObjects[0]);
                case "Contains":
                    if (parameterInfos.Length != 1)
                    {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string",
                            method.Name);
                    }

                    return new LightStringMatchDataFieldInfo(mainFieldInfo.TableMapping, true, true, callObject,
                        argObjects[0]);
                default:
                    if (Enum.TryParse<StringFunction>(method.Name, out var stringFunction))
                    {
                        if (stringFunction == StringFunction.IndexOf &&
                            !(parameterInfos.Length == 1 ||
                              (parameterInfos.Length == 2 && parameterInfos[1].ParameterType == typeof(int))))
                        {
                            throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string",
                                method.Name);
                        }

                        if (stringFunction == StringFunction.Trim && parameterInfos.Length > 0)
                        {
                            throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string",
                                method.Name);
                        }

                        return new LightStringFunctionDataFieldInfo(mainFieldInfo.TableMapping, stringFunction,
                            callObject, argObjects);
                    }

                    break;
            }

            throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport, "string", method.Name);
        }

        private static DataFieldInfo ParseInstanceDateTimeFunctionDataFieldInfo(MethodInfo method,
            DataFieldInfo mainFieldInfo, object[] argObjects)
        {
            var parameterInfos = method.GetParameters();
            if (method.Name == "ToString")
            {
                if (parameterInfos.Length == 0)
                {
                    return new LightDateFormatDataFieldInfo(mainFieldInfo, null);
                }

                if (parameterInfos.Length == 1)
                {
                    if (parameterInfos[0].ParameterType == typeof(string))
                    {
                        var o = ConvertLambdaObject(argObjects[0]);
                        return new LightDateFormatDataFieldInfo(mainFieldInfo, o as string);
                    }

                    throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "DateTime",
                        method.Name);
                }

                throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "DateTime",
                    method.Name);
            }

            throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport, "DateTime", method.Name);
        }

        private static DataFieldInfo ParseContainsDataFieldInfo(DataFieldInfo mainFieldInfo, object collections)
        {
            return new LightContainsDataFieldInfo(mainFieldInfo, collections);
        }

        private static DataFieldInfo ParseExtendQueryData(MethodCallExpression expression, LambdaState state)
        {
            var method = expression.Method;

            var paramExpressions = expression.Arguments;
            DataFieldInfo data = null;
            switch (method.Name)
            {
                case "Exists":
                {
                    if (paramExpressions[0] is UnaryExpression unaryExpresion &&
                        unaryExpresion.Operand is LambdaExpression lambdaExpresion)
                    {
                        var mstate = new RelateParameterLambdaState(lambdaExpresion.Parameters[0], state);
                        var query = ResolveQueryExpression(lambdaExpresion.Body, mstate);
                        query.MultiQuery = state.MultiEntity;
                        data = new LightExistsDataFieldInfo(mstate.MainMapping, query, true);
                    }
                    else
                    {
                        throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                    }
                }
                    break;
                case "In":
                {
                    if (!ParseDataFieldInfo(paramExpressions[0], state, out var fieldInfo))
                    {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                    }

                    if (paramExpressions[1] is UnaryExpression unaryExpresion &&
                        unaryExpresion.Operand is LambdaExpression lambdaExpresion)
                    {
                        var mstate = new RelateParameterLambdaState(lambdaExpresion.Parameters[0], state);
                        if (!ParseDataFieldInfo(lambdaExpresion.Body, mstate, out var selectFieldInfo))
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                        }

                        QueryExpression query = null;
                        if (paramExpressions.Count == 3)
                        {
                            if (paramExpressions[2] is UnaryExpression unaryExpresion1 &&
                                unaryExpresion1.Operand is LambdaExpression lambdaExpresion1)
                            {
                                query = ResolveQueryExpression(lambdaExpresion1.Body, mstate);
                                query.MultiQuery = state.MultiEntity;
                            }
                            else
                            {
                                throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                            }
                        }

                        data = new LightInQueryDataFieldInfo(mstate.MainMapping, fieldInfo, selectFieldInfo, query,
                            true);
                    }
                    else
                    {
                        throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                    }
                }
                    break;
                case "GtAll":
                case "LtAll":
                case "GtAny":
                case "LtAny":
                case "GtEqAll":
                case "LtEqAll":
                case "GtEqAny":
                case "LtEqAny":
                {
                    if (Enum.TryParse(method.Name, out QueryCollectionPredicate predicate))
                    {
                        if (!ParseDataFieldInfo(paramExpressions[0], state, out var fieldInfo))
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                        }

                        if (paramExpressions[1] is UnaryExpression unaryExpresion &&
                            unaryExpresion.Operand is LambdaExpression lambdaExpresion)
                        {
                            var mstate = new RelateParameterLambdaState(lambdaExpresion.Parameters[0], state);
                            if (!ParseDataFieldInfo(lambdaExpresion.Body, mstate, out var selectFieldInfo))
                            {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                            }

                            QueryExpression query = null;
                            if (paramExpressions.Count == 3)
                            {
                                if (paramExpressions[2] is UnaryExpression unaryExpresion1 &&
                                    unaryExpresion1.Operand is LambdaExpression lambdaExpresion1)
                                {
                                    query = ResolveQueryExpression(lambdaExpresion1.Body, mstate);
                                    query.MultiQuery = state.MultiEntity;
                                }
                                else
                                {
                                    throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                                }
                            }

                            data = new LightSubQueryDataFieldInfo(mstate.MainMapping, fieldInfo, selectFieldInfo,
                                predicate,
                                query);
                        }
                        else
                        {
                            throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                        }
                    }
                    else
                    {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport,
                            "ExtendQuery", method.Name);
                    }
                }
                    break;
                case "IsNull":
                {
                    if (!ParseDataFieldInfo(paramExpressions[0], state, out var fieldInfo))
                    {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                    }

                    data = new LightNullDataFieldInfo(fieldInfo, true);
                }
                    break;
            }

            return data;
        }

        private static DataFieldInfo ParseAggregateData(MethodCallExpression expression, LambdaState state)
        {
            var method = expression.Method;

            var paramExpressions = expression.Arguments;
            DataFieldInfo data = null;

            if (paramExpressions.Count == 0)
            {
                switch (method.Name)
                {
                    case "Count":
                    case "LongCount":
                        data = new LightAggregateCountDataFieldInfo();
                        break;
                }
            }
            else if (paramExpressions.Count == 1)
            {
                if (method.Name == "CountCondition" || method.Name == "LongCountCondition")
                {
                    var queryExpression = ResolveQueryExpression(paramExpressions[0], state);
                    data = new LightAggregateCountDataFieldInfo(queryExpression);
                }
                else
                {
                    if (!ParseDataFieldInfo(paramExpressions[0], state, out var fieldInfo))
                    {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                    }

                    switch (method.Name)
                    {
                        case "Count":
                        case "LongCount":
                            data = new LightAggregateFieldDataFieldInfo(fieldInfo, AggregateType.COUNT, false);
                            break;
                        case "DistinctCount":
                        case "DistinctLongCount":
                            data = new LightAggregateFieldDataFieldInfo(fieldInfo, AggregateType.COUNT, true);
                            break;
                        case "Sum":
                        case "LongSum":
                            data = new LightAggregateFieldDataFieldInfo(fieldInfo, AggregateType.SUM, false);
                            break;
                        case "DistinctSum":
                        case "DistinctLongSum":
                            data = new LightAggregateFieldDataFieldInfo(fieldInfo, AggregateType.SUM, true);
                            break;
                        case "Avg":
                            data = new LightAggregateFieldDataFieldInfo(fieldInfo, AggregateType.AVG, false);
                            break;
                        case "DistinctAvg":
                            data = new LightAggregateFieldDataFieldInfo(fieldInfo, AggregateType.AVG, true);
                            break;
                        case "Max":
                            data = new LightAggregateFieldDataFieldInfo(fieldInfo, AggregateType.MAX, false);

                            break;
                        case "Min":
                            data = new LightAggregateFieldDataFieldInfo(fieldInfo, AggregateType.MIN, false);
                            break;
                    }
                }
            }

            CheckFieldInfo(data);
            return data;
        }

        private static bool TryParseQueryExpression(Expression expression, LambdaState state,
            out QueryExpression resultExpression)
        {
            resultExpression = null;
            if (expression is BinaryExpression binaryObj)
            {
                if (CheckConcatOperatorsType(binaryObj.NodeType, out var catchType))
                {
                    var left = TryParseQueryExpression(binaryObj.Left, state, out var leftExpression);
                    var right = TryParseQueryExpression(binaryObj.Right, state, out var rightExpression);
                    if (left && right)
                    {
                        resultExpression = QueryExpression.Concat(leftExpression, catchType, rightExpression);
                        return true;
                    }

                    if (left)
                    {
                        var rightValue = ConvertObject(binaryObj.Right);
                        resultExpression = QueryExpression.Concat(leftExpression, catchType,
                            new LightConstantQueryExpression(rightValue));
                        return true;
                    }

                    if (right)
                    {
                        var leftValue = ConvertObject(binaryObj.Left);
                        resultExpression = QueryExpression.Concat(new LightConstantQueryExpression(leftValue),
                            catchType, rightExpression);
                        return true;
                    }
                }

                if (CheckQueryPredicate(binaryObj.NodeType, out var queryPredicate))
                {
                    object leftValue = null;
                    var left = ParseDataFieldInfo(binaryObj.Left, state, out var leftFieldInfo);
                    if (left)
                    {
                        CheckFieldInfo(leftFieldInfo);
                    }
                    else
                    {
                        leftValue = ConvertObject(binaryObj.Left);
                    }

                    object rightValue = null;
                    var right = ParseDataFieldInfo(binaryObj.Right, state, out var rightFieldInfo);
                    if (right)
                    {
                        CheckFieldInfo(rightFieldInfo);
                    }
                    else
                    {
                        rightValue = ConvertObject(binaryObj.Right);
                    }

                    if (left && right)
                    {
                        resultExpression = new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate,
                            leftFieldInfo, rightFieldInfo);
                        return true;
                    }

                    if (left)
                    {
                        resultExpression = new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate,
                            leftFieldInfo, rightValue);
                        return true;
                    }

                    if (right)
                    {
                        resultExpression = new LightBinaryQueryExpression(rightFieldInfo.TableMapping, queryPredicate,
                            rightFieldInfo, leftValue);
                        return true;
                    }
                }
            }

            if (expression is UnaryExpression unaryObj)
            {
                if (unaryObj.NodeType == ExpressionType.Not)
                {
                    if (TryParseQueryExpression(unaryObj.Operand, state, out var queryExpression))
                    {
                        if (queryExpression is ISupportNotDefine notDefine)
                        {
                            notDefine.SetNot();
                            resultExpression = queryExpression;
                            return true;
                        }

                        resultExpression = new LightNotQueryExpression(queryExpression);
                        return true;
                    }
                }
            }

            if (expression is MemberExpression)
            {
                if (ParseDataFieldInfo(expression, state, out var fieldInfo))
                {
                    CheckFieldInfo(fieldInfo);
                    if (fieldInfo is IDataFieldInfoConvert convertFieldInfo)
                    {
                        resultExpression = convertFieldInfo.ConvertToExpression();
                        return true;
                    }

                    resultExpression =
                        new LightBinaryQueryExpression(fieldInfo.TableMapping, QueryPredicate.Eq, fieldInfo, true);
                    return true;
                }
            }

            if (expression is MethodCallExpression)
            {
                if (ParseDataFieldInfo(expression, state, out var fieldInfo))
                {
                    CheckFieldInfo(fieldInfo);
                    if (fieldInfo is IDataFieldInfoConvert convertFieldInfo)
                    {
                        resultExpression = convertFieldInfo.ConvertToExpression();
                        return true;
                    }

                    throw new LambdaParseException(LambdaParseMessage.ExpressionDataFieldError, expression);
                }
            }

            if (expression is ConditionalExpression)
            {
                if (ParseDataFieldInfo(expression, state, out var fieldInfo))
                {
                    CheckFieldInfo(fieldInfo);
                    if (fieldInfo is IDataFieldInfoConvert convertFieldInfo)
                    {
                        resultExpression = convertFieldInfo.ConvertToExpression();
                        return true;
                    }

                    throw new LambdaParseException(LambdaParseMessage.ExpressionDataFieldError, expression);
                }
            }

            return false;
        }

        private static QueryExpression ResolveQueryExpression(Expression expression, LambdaState state)
        {
            if (expression is BinaryExpression binaryObj)
            {
                if (CheckConcatOperatorsType(binaryObj.NodeType, out var catchType))
                {
                    var left = ResolveQueryExpression(binaryObj.Left, state);
                    var right = ResolveQueryExpression(binaryObj.Right, state);
                    return QueryExpression.Concat(left, catchType, right);
                }

                if (CheckQueryPredicate(binaryObj.NodeType, out var queryPredicate))
                {
                    object leftValue = null;
                    bool left;
                    if (ParseDataFieldInfo(binaryObj.Left, state, out var leftFieldInfo))
                    {
                        left = true;
                        CheckFieldInfo(leftFieldInfo);
                    }
                    else
                    {
                        left = false;
                        leftValue = ConvertObject(binaryObj.Left);
                    }

                    object rightValue = null;
                    bool right;
                    if (ParseDataFieldInfo(binaryObj.Right, state, out var rightFieldInfo))
                    {
                        right = true;
                        CheckFieldInfo(rightFieldInfo);
                    }
                    else
                    {
                        right = false;
                        rightValue = ConvertObject(binaryObj.Right);
                    }

                    if (left && right)
                    {
                        return new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate,
                            leftFieldInfo, rightFieldInfo);
                    }

                    if (left)
                    {
                        return new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate,
                            leftFieldInfo, rightValue);
                    }

                    if (right)
                    {
                        return new LightBinaryQueryExpression(rightFieldInfo.TableMapping, queryPredicate,
                            rightFieldInfo, leftValue);
                    }

                    throw new LambdaParseException(LambdaParseMessage
                        .BinaryExpressionNotAllowBothConstantValue);
                }
            }

            if (expression is UnaryExpression unaryObj)
            {
                if (unaryObj.NodeType == ExpressionType.Not)
                {
                    var queryExpression = ResolveQueryExpression(unaryObj.Operand, state);
                    if (queryExpression is ISupportNotDefine notDefine)
                    {
                        notDefine.SetNot();
                        return queryExpression;
                    }

                    return new LightNotQueryExpression(queryExpression);
                }
            }

            if (expression is MemberExpression memberObj)
            {
                if (!ParseDataFieldInfo(expression, state, out var fieldInfo))
                {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, memberObj);
                }

                CheckFieldInfo(fieldInfo);
                if (fieldInfo is IDataFieldInfoConvert convertFieldInfo)
                {
                    return convertFieldInfo.ConvertToExpression();
                }

                return new LightBinaryQueryExpression(fieldInfo.TableMapping, QueryPredicate.Eq, fieldInfo, true);
            }

            if (expression is MethodCallExpression methodCallObj)
            {
                if (!ParseDataFieldInfo(expression, state, out var fieldInfo))
                {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, methodCallObj);
                }

                CheckFieldInfo(fieldInfo);
                if (fieldInfo is IDataFieldInfoConvert convertFieldInfo)
                {
                    return convertFieldInfo.ConvertToExpression();
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, methodCallObj);
            }

            if (expression is ConditionalExpression conditionObj)
            {
                if (!ParseDataFieldInfo(expression, state, out var fieldInfo))
                {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, conditionObj);
                }

                CheckFieldInfo(fieldInfo);
                if (fieldInfo is IDataFieldInfoConvert convertFieldInfo)
                {
                    return convertFieldInfo.ConvertToExpression();
                }

                throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, conditionObj);
            }

            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
        }

        private static DataFieldExpression ResolveOnExpression(Expression expression, LambdaState state)
        {
            if (expression is BinaryExpression binary)
            {
                if (CheckConcatOperatorsType(binary.NodeType, out var catchType))
                {
                    var left = ResolveOnExpression(binary.Left, state);
                    var right = ResolveOnExpression(binary.Right, state);
                    return DataFieldExpression.Concat(left, catchType, right);
                }

                if (CheckQueryPredicate(binary.NodeType, out var queryPredicate))
                {
                    bool left;
                    if (ParseDataFieldInfo(binary.Left, state, out var leftFieldInfo))
                    {
                        left = true;
                        CheckFieldInfo(leftFieldInfo);
                    }
                    else
                    {
                        left = false;
                    }

                    bool right;
                    if (ParseDataFieldInfo(binary.Right, state, out var rightFieldInfo))
                    {
                        right = true;
                        CheckFieldInfo(rightFieldInfo);
                    }
                    else
                    {
                        right = false;
                    }

                    if (left && right)
                    {
                        return new DataFieldMatchExpression(leftFieldInfo, rightFieldInfo, queryPredicate);
                    }

                    throw new LambdaParseException(LambdaParseMessage.BinaryExpressionNotAllowBothConstantValue);
                }
            }

            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
        }
    }
}