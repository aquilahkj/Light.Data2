using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace Light.Data
{
    static class LambdaExpressionExtend
    {
        static TypeInfo IEnumerableTypeInfo;

        static LambdaExpressionExtend()
        {
            IEnumerableTypeInfo = typeof(IEnumerable).GetTypeInfo();
        }

        public static object ConvertLambdaObject(object value)
        {
            object obj;
            if (value is Delegate dele) {
                obj = dele.DynamicInvoke(null);
            }
            else {
                obj = value;
            }
            if (obj != null && Object.Equals(obj, DBNull.Value)) {
                obj = null;
            }
            return obj;
        }

        public static object ConvertObject(Expression expression)
        {
            if (expression is ConstantExpression constant) {
                return constant.Value;
            }
            else {
                LambdaExpression lambda = Expression.Lambda(expression);
                Delegate fn = lambda.Compile();
                return fn;
            }
        }

        public static SelectModel CreateSelectModel(LambdaExpression expression)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                SingleParameterLambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);
                SelectModel model = null;
                if (expression.Body is MemberInitExpression memberInitObj) {
                    model = ParseSelectModel(memberInitObj, state);
                }
                else if (expression.Body is NewExpression newObj) {
                    model = ParseSelectModel(newObj, state);
                }
                //else {
                //    DataEntityMapping entityMapping = state.MainMapping;
                //    SoloFieldDataMapping soloMapping = SoloFieldDataMapping.GetMapping(expression.Type);
                //    if (ParseDataFieldInfo(expression.Body, state, out DataFieldInfo fieldInfo)) {
                //        if (state.MutliEntity) {
                //            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                //        }
                //        if (fieldInfo is LightAggregateDataFieldInfo) {
                //            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportAggregateField);
                //        }
                //        model.AddSelectField("F", fieldInfo);
                //    } else {
                //        object value = ConvertObject(expression.Body);
                //        fieldInfo = new LightConstantDataFieldInfo(value);
                //        model.AddSelectField("F", fieldInfo);
                //    }
                //}
                if (model != null) {
                    return model;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message), ex);
            }
        }

        public static AggregateModel CreateAggregateModel(LambdaExpression expression)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                SingleParameterLambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);
                AggregateModel model = null;
                if (expression.Body is MemberInitExpression memberInitObj) {
                    model = ParseAggregateModel(memberInitObj, state);
                }
                else {
                    if (expression.Body is NewExpression newObj) {
                        model = ParseAggregateModel(newObj, state);
                    }
                }
                if (model != null) {
                    return model;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "group by", expression, ex.Message), ex);
            }
        }

        public static ISelector CreateSelector(LambdaExpression expression)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);
                Expression bodyExpression = expression.Body;
                if (ParseNewArguments(bodyExpression, state, out List<string> list)) {
                    return state.CreateSelector(list.ToArray());
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message), ex);
            }
        }

        public static ISelector CreateMutliSelector(LambdaExpression expression, List<IMap> maps)
        {
            try {
                if (expression.Parameters.Count <= 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new MutliParameterLambdaState(expression.Parameters, maps);
                Expression bodyExpression = expression.Body;
                if (bodyExpression is MemberInitExpression || bodyExpression is NewExpression || bodyExpression is ParameterExpression) {
                    if (ParseNewArguments(bodyExpression, state, out List<string> list)) {
                        return state.CreateSelector(list.ToArray());
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                    }
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message), ex);
            }
        }

        public static InsertSelector CreateMutliInsertSelector(LambdaExpression expression, List<IMap> maps)
        {
            try {
                if (expression.Parameters.Count <= 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new MutliParameterLambdaState(expression.Parameters, maps);
                if (expression.Body is MemberInitExpression memberInitObj) {
                    DataTableEntityMapping insertMapping = DataEntityMapping.GetTableMapping(memberInitObj.Type);
                    RelationMap map = insertMapping.GetRelationMap();
                    InsertSelector selector = new InsertSelector(insertMapping);
                    if (memberInitObj.Bindings != null) {
                        foreach (MemberBinding binding in memberInitObj.Bindings) {
                            if (binding is MemberAssignment ass) {
                                Expression innerExpression = ass.Expression;
                                if (!ParseDataFieldInfo(innerExpression, state, out DataFieldInfo selectField)) {
                                    object obj = ConvertObject(innerExpression);
                                    selectField = new LightConstantDataFieldInfo(obj);
                                }
                                string mypath = "." + ass.Member.Name;
                                DataFieldInfo insertField = map.GetFieldInfoForPath(mypath);

                                selector.SetInsertField(insertField);
                                selector.SetSelectField(selectField);
                            }
                            else {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionBindingError, binding.Member);
                            }
                        }
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNoMember);
                    }
                    return selector;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message), ex);
            }
        }

        public static OrderExpression ResolveLambdaOrderByExpression(LambdaExpression expression, OrderType orderType)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);
                if (ParseDataFieldInfo(expression.Body, state, out DataFieldInfo dataFieldInfo)) {
                    CheckFieldInfo(dataFieldInfo);
                    OrderExpression exp = new DataFieldOrderExpression(dataFieldInfo, orderType);
                    exp.MutliOrder = true;
                    return exp;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "order", expression, ex.Message), ex);
            }
        }

        public static OrderExpression ResolveLambdaMutliOrderByExpression(LambdaExpression expression, OrderType orderType, List<IMap> maps)
        {
            try {
                if (expression.Parameters.Count <= 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new MutliParameterLambdaState(expression.Parameters, maps);
                if (ParseDataFieldInfo(expression.Body, state, out DataFieldInfo dataFieldInfo)) {
                    CheckFieldInfo(dataFieldInfo);
                    OrderExpression exp = new DataFieldOrderExpression(dataFieldInfo, orderType);
                    exp.MutliOrder = true;
                    return exp;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "order", expression, ex.Message), ex);
            }
        }

        public static QueryExpression ResolveLambdaQueryExpression(LambdaExpression expression)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);
                QueryExpression query = ResolveQueryExpression(expression.Body, state);
                query.MutliQuery = state.MutliEntity;
                return query;
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "query", expression, ex.Message), ex);
            }
        }

        public static QueryExpression ResolveLambdaMutliQueryExpression(LambdaExpression expression, List<IMap> maps)
        {
            try {
                if (expression.Parameters.Count <= 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new MutliParameterLambdaState(expression.Parameters, maps);
                QueryExpression query = ResolveQueryExpression(expression.Body, state);
                query.MutliQuery = state.MutliEntity;
                return query;
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "query", expression, ex.Message), ex);
            }
        }

        public static QueryExpression ResolveLambdaHavingExpression(LambdaExpression expression, AggregateModel model)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new AggregateLambdaState(expression.Parameters[0], model);
                QueryExpression query = ResolveQueryExpression(expression.Body, state);
                return query;
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "having", expression, ex.Message), ex);
            }
        }

        public static OrderExpression ResolveLambdaAggregateOrderByExpression(LambdaExpression expression, OrderType orderType, AggregateModel model)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new AggregateLambdaState(expression.Parameters[0], model);
                if (ParseDataFieldInfo(expression.Body, state, out DataFieldInfo dataFieldInfo)) {
                    CheckFieldInfo(dataFieldInfo);
                    OrderExpression exp = new DataFieldOrderExpression(dataFieldInfo, orderType);
                    return exp;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "aggregate order", expression, ex.Message), ex);
            }
        }

        public static DataFieldExpression ResolvelambdaOnExpression(LambdaExpression expression, List<IMap> maps)
        {
            try {
                if (expression.Parameters.Count <= 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new MutliParameterLambdaState(expression.Parameters, maps);
                DataFieldExpression on = ResolveOnExpression(expression.Body, state);
                return on;
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "on", expression, ex.Message), ex);
            }
        }

        public static InsertSelector CreateInsertSelector(LambdaExpression expression)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                SingleParameterLambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);
                if (expression.Body is MemberInitExpression memberInitObj) {
                    DataTableEntityMapping insertMapping = DataEntityMapping.GetTableMapping(memberInitObj.Type);
                    RelationMap map = insertMapping.GetRelationMap();
                    InsertSelector selector = new InsertSelector(insertMapping, state.MainMapping);
                    if (memberInitObj.Bindings != null) {
                        foreach (MemberBinding binding in memberInitObj.Bindings) {
                            if (binding is MemberAssignment ass) {
                                Expression innerExpression = ass.Expression;
                                if (!ParseDataFieldInfo(innerExpression, state, out DataFieldInfo selectField)) {
                                    object obj = ConvertObject(innerExpression);
                                    selectField = new LightConstantDataFieldInfo(obj);
                                }
                                string mypath = "." + ass.Member.Name;
                                DataFieldInfo insertField = map.GetFieldInfoForPath(mypath);
                                selector.SetInsertField(insertField);
                                selector.SetSelectField(selectField);
                            }
                            else {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionBindingError, binding.Member);
                            }
                        }
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNoMember);
                    }
                    return selector;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "insert", expression, ex.Message), ex);
            }
        }

        public static InsertSelector CreateAggregateInsertSelector(LambdaExpression expression, AggregateModel model)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                LambdaState state = new AggregateLambdaState(expression.Parameters[0], model);
                if (expression.Body is MemberInitExpression memberInitObj) {
                    DataTableEntityMapping insertMapping = DataEntityMapping.GetTableMapping(memberInitObj.Type);
                    RelationMap map = insertMapping.GetRelationMap();
                    InsertSelector selector = new InsertSelector(insertMapping);
                    if (memberInitObj.Bindings != null) {
                        foreach (MemberBinding binding in memberInitObj.Bindings) {
                            if (binding is MemberAssignment ass) {
                                Expression innerExpression = ass.Expression;
                                if (!ParseDataFieldInfo(innerExpression, state, out DataFieldInfo selectField)) {
                                    object obj = ConvertObject(innerExpression);
                                    selectField = new LightConstantDataFieldInfo(obj);
                                }
                                else {
                                    selectField = new DataFieldInfo(selectField.TableMapping, true, selectField.FieldName);
                                }
                                string mypath = "." + ass.Member.Name;
                                DataFieldInfo insertField = map.GetFieldInfoForPath(mypath);

                                selector.SetInsertField(insertField);
                                selector.SetSelectField(selectField);
                            }
                            else {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionBindingError, binding.Member);
                            }
                        }
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNoMember);
                    }
                    return selector;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "select", expression, ex.Message), ex);
            }
        }

        public static MassUpdator CreateMassUpdator(LambdaExpression expression)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                SingleParameterLambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);

                if (expression.Body is MemberInitExpression memberInitObj) {
                    DataTableEntityMapping updateMapping = DataEntityMapping.GetTableMapping(memberInitObj.Type);
                    RelationMap map = updateMapping.GetRelationMap();
                    MassUpdator updator = new MassUpdator(updateMapping);
                    if (memberInitObj.Bindings != null && memberInitObj.Bindings.Count > 0) {
                        foreach (MemberBinding binding in memberInitObj.Bindings) {
                            if (binding is MemberAssignment ass) {
                                Expression innerExpression = ass.Expression;
                                if (!ParseDataFieldInfo(innerExpression, state, out DataFieldInfo valueField)) {
                                    object obj = ConvertObject(innerExpression);
                                    valueField = new LightConstantDataFieldInfo(obj);
                                }
                                else if (state.MutliEntity) {
                                    throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                                }
                                string mypath = "." + ass.Member.Name;
                                DataFieldInfo keyField = map.GetFieldInfoForPath(mypath);
                                updator.SetUpdateData(keyField, valueField);
                            }
                            else {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionBindingError, binding.Member);
                            }
                        }
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNoMember);
                    }
                    return updator;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "updator", expression, ex.Message), ex);
            }
        }

        public static DataFieldInfo ResolveSingleField(LambdaExpression expression)
        {
            try {
                if (expression.Parameters.Count != 1) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionParameterCountError);
                }
                SingleParameterLambdaState state = new SingleParameterLambdaState(expression.Parameters[0]);
                if (ParseDataFieldInfo(expression.Body, state, out DataFieldInfo fieldInfo)) {
                    if (state.MutliEntity) {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                    }
                    return fieldInfo;
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                }
            }
            catch (Exception ex) {
                throw new LightDataException(string.Format(SR.ParseExpressionError, "single field", expression, ex.Message), ex);
            }
        }

        private static bool ParseNewArguments(Expression expression, LambdaState state, out List<string> pathList)
        {
            if (expression is ParameterExpression paramObj) {
                pathList = new List<string> {
                    paramObj.Name
                };
                return true;
            }
            if (expression is MemberExpression memberObj) {
                if (memberObj.Expression != null) {
                    if (memberObj.Expression is ParameterExpression param) {
                        if (state.CheckPamramter(param.Name, param.Type)) {
                            string fullPath = memberObj.ToString();
                            LambdaPathType pathType = state.ParsePath(fullPath);
                            switch (pathType) {
                                case LambdaPathType.Field:
                                    pathList = new List<string> {
                                        fullPath
                                    };
                                    return true;
                                case LambdaPathType.RelateEntity:
                                    pathList = new List<string> {
                                        fullPath
                                    };
                                    return true;
                                case LambdaPathType.RelateCollection:
                                    pathList = new List<string> {
                                        fullPath
                                    };
                                    return true;
                                case LambdaPathType.Parameter:
                                    throw new LambdaParseException(LambdaParseMessage.ExpressionMemberInvalid);
                                case LambdaPathType.None:
                                    break;
                                default:
                                    pathList = new List<string> {
                                        param.Name
                                    };
                                    return true;
                            }
                        }
                        else {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionParameterTypeError, param.Name, param.Type);
                        }
                    }
                    if (ParseNewArguments(memberObj.Expression, state, out List<string> memberList)) {
                        if (memberList == null) {
                            string fullPath = memberObj.ToString();
                            LambdaPathType pathType = state.ParsePath(fullPath);
                            switch (pathType) {
                                case LambdaPathType.Field:
                                    pathList = new List<string> {
                                        fullPath
                                    };
                                    return true;
                                case LambdaPathType.RelateEntity:
                                    pathList = null;
                                    return true;
                                case LambdaPathType.RelateCollection:
                                    pathList = new List<string> {
                                        fullPath
                                    };
                                    return true;
                                default:
                                    pathList = new List<string> {
                                        memberObj.Expression.ToString()
                                    };
                                    return true;
                            }
                        }
                        else {
                            pathList = memberList;
                            return true;
                        }
                    }
                    else {
                        pathList = null;
                        return false;
                    }
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            if (expression is UnaryExpression unaryObj) {
                return ParseNewArguments(unaryObj.Operand, state, out pathList);
            }
            if (expression is MethodCallExpression methodcallObj) {
                bool ret = false;
                List<string> methodList = new List<string>();
                if (methodcallObj.Arguments != null) {
                    foreach (Expression arg in methodcallObj.Arguments) {
                        if (ParseNewArguments(arg, state, out List<string> argList)) {
                            ret = true;
                            if (argList == null) {
                                methodList.Add(arg.ToString());
                            }
                            else {
                                methodList.AddRange(argList);
                            }
                        }
                    }
                }
                if (methodcallObj.Object != null) {
                    if (ParseNewArguments(methodcallObj.Object, state, out List<string> callList)) {
                        ret = true;
                        if (callList == null) {
                            methodList.Add(methodcallObj.Object.ToString());
                        }
                        else {
                            methodList.AddRange(callList);
                        }
                    }
                }
                if (ret) {
                    pathList = methodList;
                    return true;
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            if (expression is NewArrayExpression newarrayObj) {
                if (newarrayObj.Expressions != null) {
                    bool ret = false;
                    List<string> newarrayList = new List<string>();
                    List<string> argList = null;
                    foreach (Expression arg in newarrayObj.Expressions) {
                        if (ParseNewArguments(arg, state, out argList)) {
                            ret = true;
                            if (argList == null) {
                                newarrayList.Add(arg.ToString());
                            }
                            else {
                                newarrayList.AddRange(argList);
                            }
                        }
                    }
                    if (ret) {
                        pathList = newarrayList;
                        return true;
                    }
                    else {
                        pathList = null;
                        return false;
                    }
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            if (expression is MemberInitExpression memberInitObj) {
                bool ret = false;
                List<string> memberInitList = new List<string>();

                if (memberInitObj.Bindings != null && memberInitObj.Bindings.Count > 0) {
                    foreach (MemberAssignment ass in memberInitObj.Bindings) {
                        if (ass != null) {
                            if (ParseNewArguments(ass.Expression, state, out List<string> argList)) {
                                ret = true;
                                if (argList == null) {
                                    memberInitList.Add(ass.Expression.ToString());
                                }
                                else {
                                    memberInitList.AddRange(argList);
                                }
                            }
                        }
                    }
                }
                if (memberInitObj.NewExpression != null) {
                    if (ParseNewArguments(memberInitObj.NewExpression, state, out List<string> newList)) {
                        ret = true;
                        if (newList == null) {
                            memberInitList.Add(memberInitObj.NewExpression.ToString());
                        }
                        else {
                            memberInitList.AddRange(newList);
                        }
                    }
                }
                if (ret) {
                    pathList = memberInitList;
                    return true;
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            if (expression is NewExpression newObj) {
                if (newObj.Arguments != null) {
                    bool ret = false;
                    List<string> newobjList = new List<string>();
                    List<string> argList = null;
                    foreach (Expression arg in newObj.Arguments) {
                        if (ParseNewArguments(arg, state, out argList)) {
                            ret = true;
                            if (argList == null) {
                                newobjList.Add(arg.ToString());
                            }
                            else {
                                newobjList.AddRange(argList);
                            }
                        }
                    }
                    if (ret) {
                        pathList = newobjList;
                        return true;
                    }
                    else {
                        pathList = null;
                        return false;
                    }
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            if (expression is ConstantExpression constantObj) {
                pathList = null;
                return false;
            }
            if (expression is BinaryExpression binaryObj) {
                bool ret = false;
                List<string> binaryList = new List<string>();
                if (ParseNewArguments(binaryObj.Left, state, out List<string> leftList)) {
                    ret = true;
                    if (leftList == null) {
                        binaryList.Add(binaryObj.Left.ToString());
                    }
                    else {
                        binaryList.AddRange(leftList);
                    }
                }
                if (ParseNewArguments(binaryObj.Right, state, out List<string> rightList)) {
                    ret = true;
                    if (rightList == null) {
                        binaryList.Add(binaryObj.Right.ToString());
                    }
                    else {
                        binaryList.AddRange(rightList);
                    }
                }
                if (ret) {
                    pathList = binaryList;
                    return true;
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            if (expression is IndexExpression indexObj) {
                bool ret = false;
                List<string> indexList = new List<string>();
                if (indexObj.Arguments != null) {
                    foreach (Expression arg in indexObj.Arguments) {
                        if (ParseNewArguments(arg, state, out List<string> argList)) {
                            ret = true;
                            if (argList == null) {
                                indexList.Add(arg.ToString());
                            }
                            else {
                                indexList.AddRange(argList);
                            }
                        }
                    }
                }
                if (indexObj.Object != null) {
                    if (ParseNewArguments(indexObj.Object, state, out List<string> callList)) {
                        ret = true;
                        if (callList == null) {
                            indexList.Add(indexObj.Object.ToString());
                        }
                        else {
                            indexList.AddRange(callList);
                        }
                    }
                }
                if (ret) {
                    pathList = indexList;
                    return true;
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            if (expression is DynamicExpression dynamicObj) {
                if (dynamicObj.Arguments != null) {
                    bool ret = false;
                    List<string> aynamicList = new List<string>();
                    List<string> argList = null;
                    foreach (Expression arg in dynamicObj.Arguments) {
                        if (ParseNewArguments(arg, state, out argList)) {
                            ret = true;
                            if (argList == null) {
                                aynamicList.Add(arg.ToString());
                            }
                            else {
                                aynamicList.AddRange(argList);
                            }
                        }
                    }
                    if (ret) {
                        pathList = aynamicList;
                        return true;
                    }
                    else {
                        pathList = null;
                        return false;
                    }
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            if (expression is DefaultExpression defaultObj) {
                pathList = null;
                return false;
            }
            if (expression is ConditionalExpression conditionObj) {
                bool ret = false;
                List<string> conditionList = new List<string>();
                if (ParseNewArguments(conditionObj.Test, state, out List<string> testList)) {
                    ret = true;
                    if (testList == null) {
                        conditionList.Add(conditionObj.Test.ToString());
                    }
                    else {
                        conditionList.AddRange(testList);
                    }
                }
                if (ParseNewArguments(conditionObj.IfTrue, state, out List<string> trueList)) {
                    ret = true;
                    if (trueList == null) {
                        conditionList.Add(conditionObj.IfTrue.ToString());
                    }
                    else {
                        conditionList.AddRange(trueList);
                    }
                }
                if (ParseNewArguments(conditionObj.IfFalse, state, out List<string> falseList)) {
                    ret = true;
                    if (falseList == null) {
                        conditionList.Add(conditionObj.IfFalse.ToString());
                    }
                    else {
                        conditionList.AddRange(falseList);
                    }
                }
                if (ret) {
                    pathList = conditionList;
                    return true;
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            if (expression is TypeBinaryExpression typeBinaryObj) {
                return ParseNewArguments(typeBinaryObj.Expression, state, out pathList);
            }
            if (expression is ListInitExpression listInitObj) {
                bool ret = false;
                List<string> lisInitList = new List<string>();
                if (listInitObj.Initializers != null) {
                    foreach (ElementInit init in listInitObj.Initializers) {
                        List<string> argList = null;
                        foreach (Expression arg in init.Arguments) {
                            if (ParseNewArguments(arg, state, out argList)) {
                                ret = true;
                                if (argList == null) {
                                    lisInitList.Add(arg.ToString());
                                }
                                else {
                                    lisInitList.AddRange(argList);
                                }
                            }
                        }
                    }
                }
                if (listInitObj.NewExpression != null) {
                    if (ParseNewArguments(listInitObj.NewExpression, state, out List<string> newList)) {
                        ret = true;
                        if (newList == null) {
                            lisInitList.Add(listInitObj.NewExpression.ToString());
                        }
                        else {
                            lisInitList.AddRange(newList);
                        }
                    }
                }
                if (ret) {
                    pathList = lisInitList;
                    return true;
                }
                else {
                    pathList = null;
                    return false;
                }
            }
            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
        }

        private static void CheckFieldInfo(DataFieldInfo fieldInfo)
        {
            if (Object.Equals(fieldInfo, null)) {
                throw new LambdaParseException(LambdaParseMessage.ExpressionParseFieldFailed);
            }
        }

        private static SelectModel ParseSelectModel(MemberInitExpression expression, SingleParameterLambdaState state)
        {
            DataEntityMapping entityMapping = state.MainMapping;
            SpecifiedCustomMapping customMapping = SpecifiedCustomMapping.GetMapping(expression.Type);
            SelectModel model = new SelectModel(entityMapping, customMapping);
            if (expression.Bindings != null && expression.Bindings.Count > 0) {
                foreach (MemberBinding binding in expression.Bindings) {
                    if (binding is MemberAssignment ass) {
                        Expression innerExpression = ass.Expression;
                        if (ParseDataFieldInfo(innerExpression, state, out DataFieldInfo fieldInfo)) {
                            if (state.MutliEntity) {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                            }
                            if (fieldInfo is LightAggregateDataFieldInfo) {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportAggregateField);
                            }
                            model.AddSelectField(ass.Member.Name, fieldInfo);
                        }
                        else {
                            object value = ConvertObject(innerExpression);
                            fieldInfo = new LightConstantDataFieldInfo(value);
                            model.AddSelectField(ass.Member.Name, fieldInfo);
                        }
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionBindingError, binding.Member);
                    }
                }
            }
            else {
                throw new LambdaParseException(LambdaParseMessage.ExpressionNoMember);
            }
            return model;
        }

        private static SelectModel ParseSelectModel(NewExpression expression, SingleParameterLambdaState state)
        {
            DataEntityMapping entityMapping = state.MainMapping;
            DynamicCustomMapping customMapping = DynamicCustomMapping.GetMapping(expression.Type);
            SelectModel model = new SelectModel(entityMapping, customMapping);
            if (expression.Arguments != null && expression.Arguments.Count > 0) {
                int index = 0;
                foreach (Expression arg in expression.Arguments) {
                    MemberInfo member = expression.Members[index];
                    Expression innerExpression = arg;
                    if (ParseDataFieldInfo(innerExpression, state, out DataFieldInfo fieldInfo)) {
                        if (state.MutliEntity) {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                        }
                        if (fieldInfo is LightAggregateDataFieldInfo) {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportAggregateField);
                        }
                        model.AddSelectField(member.Name, fieldInfo);
                        index++;
                    }
                    else {
                        object value = ConvertObject(innerExpression);
                        fieldInfo = new LightConstantDataFieldInfo(value);
                        model.AddSelectField(member.Name, fieldInfo);
                    }
                }
            }
            else {
                throw new LambdaParseException(LambdaParseMessage.ExpressionNoArguments);
            }
            return model;
        }

        private static AggregateModel ParseAggregateModel(MemberInitExpression expression, SingleParameterLambdaState state)
        {
            DataEntityMapping entityMapping = state.MainMapping;
            SpecifiedCustomMapping arrgregateMapping = SpecifiedCustomMapping.GetMapping(expression.Type);
            AggregateModel model = new AggregateModel(entityMapping, arrgregateMapping);
            if (expression.Bindings != null && expression.Bindings.Count > 0) {
                bool hasAggregateField = false;
                foreach (MemberBinding binding in expression.Bindings) {
                    if (binding is MemberAssignment ass) {
                        Expression innerExpression = ass.Expression;
                        if (ParseDataFieldInfo(innerExpression, state, out DataFieldInfo fieldInfo)) {
                            if (state.MutliEntity) {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                            }
                            if (fieldInfo is LightAggregateDataFieldInfo) {
                                hasAggregateField = true;
                                model.AddAggregateField(ass.Member.Name, fieldInfo);
                            }
                            else {
                                model.AddGroupByField(ass.Member.Name, fieldInfo);
                            }
                        }
                        else {
                            object value = ConvertObject(innerExpression);
                            fieldInfo = new LightConstantDataFieldInfo(value);
                            model.AddAggregateField(ass.Member.Name, fieldInfo);
                            //throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                        }
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionBindingError, binding.Member);
                    }
                }
                if (!hasAggregateField) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainAggregateFunction);
                }
            }
            else {
                throw new LambdaParseException(LambdaParseMessage.ExpressionNoMember);
            }
            return model;
        }

        private static AggregateModel ParseAggregateModel(NewExpression expression, SingleParameterLambdaState state)
        {
            DataEntityMapping entityMapping = state.MainMapping;
            DynamicCustomMapping arrgregateMapping = DynamicCustomMapping.GetMapping(expression.Type);
            AggregateModel model = new AggregateModel(entityMapping, arrgregateMapping);
            if (expression.Arguments != null && expression.Arguments.Count > 0) {
                int index = 0;
                bool hasAggregateField = false;
                foreach (Expression arg in expression.Arguments) {
                    MemberInfo member = expression.Members[index];
                    Expression innerExpression = arg;
                    if (ParseDataFieldInfo(innerExpression, state, out DataFieldInfo fieldInfo)) {
                        if (state.MutliEntity) {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionUnsupportRelateField);
                        }
                        if (fieldInfo is LightAggregateDataFieldInfo) {
                            hasAggregateField = true;
                            model.AddAggregateField(member.Name, fieldInfo);
                        }
                        else {
                            model.AddGroupByField(member.Name, fieldInfo);
                        }
                    }
                    else {
                        object value = ConvertObject(innerExpression);
                        fieldInfo = new LightConstantDataFieldInfo(value);
                        model.AddAggregateField(member.Name, fieldInfo);
                        //throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                    }
                    index++;
                }
                if (!hasAggregateField) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainAggregateFunction);
                }
            }
            else {
                throw new LambdaParseException(LambdaParseMessage.ExpressionNoArguments);
            }
            return model;
        }

        private static bool ParseDataFieldInfo(Expression expression, LambdaState state, out DataFieldInfo fieldInfo)
        {
            fieldInfo = null;
            if (expression.NodeType == ExpressionType.Constant) {
                return false;
            }
            if (expression is BinaryExpression binaryObj) {
                object leftValue = null;
                bool left;
                if (ParseDataFieldInfo(binaryObj.Left, state, out DataFieldInfo leftFieldInfo)) {
                    left = true;
                    CheckFieldInfo(leftFieldInfo);
                }
                else {
                    left = false;
                    leftValue = ConvertObject(binaryObj.Left);
                }

                object rightValue = null;
                bool right;
                if (ParseDataFieldInfo(binaryObj.Right, state, out DataFieldInfo rightFieldInfo)) {
                    right = true;
                    CheckFieldInfo(rightFieldInfo);
                }
                else {
                    right = false;
                    rightValue = ConvertObject(binaryObj.Right);
                }
                if (!left && !right) {
                    return false;
                }

                if (binaryObj.Method != null && binaryObj.NodeType == ExpressionType.Add && binaryObj.Method.DeclaringType == typeof(string) && binaryObj.Method.Name == "Concat") {
                    if (left && right) {
                        fieldInfo = new LightStringConcatDataFieldInfo(leftFieldInfo.TableMapping, leftFieldInfo, rightFieldInfo);
                        return true;
                    }
                    else if (left && !right) {
                        fieldInfo = new LightStringConcatDataFieldInfo(leftFieldInfo.TableMapping, leftFieldInfo, rightValue);
                        return true;
                    }
                    else if (!left && right) {
                        fieldInfo = new LightStringConcatDataFieldInfo(rightFieldInfo.TableMapping, leftValue, rightFieldInfo);
                        return true;
                    }
                }
                else {
                    if (CheckMathOperator(binaryObj.NodeType, out MathOperator mathOperator)) {
                        if (left && right) {
                            fieldInfo = new LightMathCalculateDataFieldInfo(leftFieldInfo.TableMapping, mathOperator, leftFieldInfo, rightFieldInfo);
                            return true;
                        }
                        else if (left && !right) {
                            fieldInfo = new LightMathCalculateDataFieldInfo(leftFieldInfo.TableMapping, mathOperator, leftFieldInfo, rightValue);
                            return true;
                        }
                        else if (!left && right) {
                            fieldInfo = new LightMathCalculateDataFieldInfo(rightFieldInfo.TableMapping, mathOperator, leftValue, rightFieldInfo);
                            return true;
                        }
                    }
                    if (CheckQueryPredicate(binaryObj.NodeType, out QueryPredicate queryPredicate)) {
                        QueryExpression queryExpression;
                        if (left && right) {
                            queryExpression = new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate, leftFieldInfo, rightFieldInfo);
                        }
                        else if (left && !right) {
                            queryExpression = new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate, leftFieldInfo, rightValue);
                        }
                        else if (!left && right) {
                            queryExpression = new LightBinaryQueryExpression(rightFieldInfo.TableMapping, queryPredicate, rightFieldInfo, leftValue);
                        }
                        else {
                            throw new LambdaParseException(LambdaParseMessage.BinaryExpressionNotAllowBothConstantValue);
                        }
                        fieldInfo = new LightQueryDataFieldInfo(queryExpression);
                        return true;
                    }
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNodeTypeUnsuppore, binaryObj.NodeType);
                }
            }
            if (expression is UnaryExpression unaryObj) {
                if (unaryObj.NodeType == ExpressionType.Not) {
                    if (ParseDataFieldInfo(unaryObj.Operand, state, out DataFieldInfo notfieldInfo)) {
                        CheckFieldInfo(notfieldInfo);
                    }
                    else {
                        return false;
                    }
                    if (notfieldInfo is ISupportNotDefine notDefine) {
                        notDefine.SetNot();
                        fieldInfo = notfieldInfo;
                    }
                    else {
                        fieldInfo = new LightNotDataFieldInfo(notfieldInfo);
                    }
                    return true;
                }
                else if (unaryObj.NodeType == ExpressionType.Convert) {
                    if (ParseDataFieldInfo(unaryObj.Operand, state, out DataFieldInfo convertfieldInfo)) {
                        CheckFieldInfo(convertfieldInfo);
                        fieldInfo = convertfieldInfo;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else if (unaryObj.NodeType == ExpressionType.TypeAs) {
                    if (unaryObj.Type.GetTypeInfo().IsGenericType) {
                        Type frameType = unaryObj.Type.GetGenericTypeDefinition();
                        if (frameType.FullName != "System.Nullable`1") {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
                        }
                    }

                    if (ParseDataFieldInfo(unaryObj.Operand, state, out DataFieldInfo convertfieldInfo)) {
                        CheckFieldInfo(convertfieldInfo);
                        fieldInfo = convertfieldInfo;
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            if (expression is MemberExpression memberObj) {
                if (memberObj.Expression != null) {
                    if (memberObj.Expression is ParameterExpression param) {
                        if (state.CheckPamramter(param.Name, param.Type)) {
                            string fullPath = memberObj.ToString();
                            LambdaPathType pathType = state.ParsePath(fullPath);
                            if (pathType == LambdaPathType.Field) {
                                DataFieldInfo myinfo = state.GetDataFieldInfo(fullPath);
                                fieldInfo = myinfo;
                                return true;
                            }
                            else if (pathType == LambdaPathType.RelateEntity) {
                                return true;
                            }
                            else {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionMemberInvalid);
                            }
                        }
                        else {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionParameterTypeError, param.Name, param.Type);
                        }
                    }
                    if (memberObj.Expression is UnaryExpression unary && unary.NodeType == ExpressionType.Convert && unary.Operand is ParameterExpression param1) {
                        if (state.CheckPamramter(param1.Name, param1.Type)) {
                            string fullPath = memberObj.ToString().Replace(unary.ToString(), param1.ToString());
                            LambdaPathType pathType = state.ParsePath(fullPath);
                            if (pathType == LambdaPathType.Field) {
                                DataFieldInfo myinfo = state.GetDataFieldInfo(fullPath);
                                fieldInfo = myinfo;
                                return true;
                            }
                            else if (pathType == LambdaPathType.RelateEntity) {
                                return true;
                            }
                            else {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionMemberInvalid);
                            }
                        }
                        else {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionParameterTypeError, param1.Name, param1.Type);
                        }
                    }

                    if (ParseDataFieldInfo(memberObj.Expression, state, out fieldInfo)) {
                        if (Object.Equals(fieldInfo, null)) {
                            string fullPath = memberObj.ToString();
                            LambdaPathType pathType = state.ParsePath(fullPath);
                            if (pathType == LambdaPathType.Field) {
                                DataFieldInfo myinfo = state.GetDataFieldInfo(fullPath);
                                fieldInfo = myinfo;
                                state.MutliEntity = true;
                                return true;
                            }
                            else if (pathType == LambdaPathType.RelateEntity) {
                                return true;
                            }
                            else {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionMemberInvalid);
                            }
                        }
                        else {
                            if (memberObj.Expression.Type.GetTypeInfo().IsGenericType) {
                                Type frameType = memberObj.Expression.Type.GetGenericTypeDefinition();
                                if (frameType.FullName == "System.Nullable`1") {
                                    if (memberObj.Member.Name == "Value") {
                                        return true;
                                    }
                                    if (memberObj.Member.Name == "HasValue") {
                                        fieldInfo = new LightNullDataFieldInfo(fieldInfo, false);
                                        return true;
                                    }
                                }
                            }
                            else {
                                if (memberObj.Expression.Type == typeof(DateTime)) {
                                    fieldInfo = CreateDateDataFieldInfo(memberObj.Member, fieldInfo);
                                    return true;
                                }
                                if (memberObj.Expression.Type == typeof(string)) {
                                    fieldInfo = CreateStringMemberDataFieldInfo(memberObj.Member, fieldInfo);
                                    return true;
                                }
                            }
                            throw new LambdaParseException(LambdaParseMessage.MemberExpressionTypeUnsupport, memberObj.Expression.Type);

                        }
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
            if (expression is MethodCallExpression methodcallObj) {
                MethodInfo methodInfo = methodcallObj.Method;
                if ((methodInfo.Attributes & MethodAttributes.Static) == MethodAttributes.Static) {
                    if (methodInfo.DeclaringType == typeof(Function)) {
                        fieldInfo = ParseAggregateData(methodcallObj, state);
                        return true;
                    }
                    if (methodInfo.DeclaringType == typeof(ExtendQuery)) {
                        fieldInfo = ParseExtendQueryData(methodcallObj, state);
                        return true;
                    }
                    if (ParseArguments(methodcallObj.Arguments, state, out object[] argObjects, out DataFieldInfo mainFieldInfo)) {
                        CheckFieldInfo(mainFieldInfo);
                        if (methodInfo.DeclaringType == typeof(Math)) {
                            fieldInfo = ParseMathFunctionDataFieldInfo(methodInfo, mainFieldInfo, argObjects, state);
                            return true;
                        }
                        if (methodInfo.DeclaringType == typeof(string)) {
                            fieldInfo = ParseStaticStringFunctionDataFieldInfo(methodInfo, mainFieldInfo, argObjects, state);
                            return true;
                        }
                        else {
                            throw new LambdaParseException(LambdaParseMessage.MethodExpressionTypeUnsupport, methodInfo.DeclaringType);
                        }
                    }
                    else {
                        return false;
                    }
                }
                else {
                    DataFieldInfo mainFieldInfo = null;
                    object callObject = null;
                    if (ParseDataFieldInfo(methodcallObj.Object, state, out DataFieldInfo callFieldInfo)) {
                        CheckFieldInfo(callFieldInfo);
                        mainFieldInfo = callFieldInfo;
                        callObject = callFieldInfo;
                    }
                    else {
                        callObject = ConvertObject(methodcallObj.Object);
                    }

                    if (ParseArguments(methodcallObj.Arguments, state, out object[] argObjects, out DataFieldInfo mainArgFieldInfo)) {
                        if (Object.Equals(mainFieldInfo, null)) {
                            mainFieldInfo = mainArgFieldInfo;
                        }
                    }
                    if (Object.Equals(mainFieldInfo, null)) {
                        return false;
                    }

                    if (methodcallObj.Object.Type == typeof(string)) {
                        fieldInfo = ParseInstanceStringFunctionDataFieldInfo(methodInfo, mainFieldInfo, callObject, argObjects, state);
                        return true;
                    }
                    if (methodcallObj.Object.Type == typeof(DateTime)) {
                        fieldInfo = ParseInstanceDateTimeFunctionDataFieldInfo(methodInfo, mainFieldInfo, callObject, argObjects, state);
                        return true;
                    }
                    if (Object.Equals(callFieldInfo, null) && argObjects != null && argObjects.Length == 1 && methodInfo.Name == "Contains" && IEnumerableTypeInfo.IsAssignableFrom(methodInfo.DeclaringType)) {
                        fieldInfo = ParseContainsDataFieldInfo(methodInfo, mainFieldInfo, callObject, state);
                        return true;
                    }
                }
            }
            if (expression is NewArrayExpression newarrayObj) {
                if (ParseArguments(newarrayObj.Expressions, state, out object[] argsObjects, out DataFieldInfo arrayFieldInfo)) {
                    fieldInfo = new LightNewArrayDataFieldInfo(arrayFieldInfo.TableMapping, argsObjects);
                    return true;
                }
                else {
                    return false;
                }
            }
            if (expression is ConditionalExpression conditionObj) {
                QueryExpression query = ResolveQueryExpression(conditionObj.Test, state);
                object ifTrueValue = null;
                if (ParseDataFieldInfo(conditionObj.IfTrue, state, out DataFieldInfo ifTrueFieldInfo)) {
                    CheckFieldInfo(ifTrueFieldInfo);
                    ifTrueValue = ifTrueFieldInfo;
                }
                else {
                    ifTrueValue = ConvertObject(conditionObj.IfTrue);
                }
                object ifFalseValue = null;
                if (ParseDataFieldInfo(conditionObj.IfFalse, state, out DataFieldInfo ifFalseFieldInfo)) {
                    CheckFieldInfo(ifFalseFieldInfo);
                    ifFalseValue = ifFalseFieldInfo;
                }
                else {
                    ifFalseValue = ConvertObject(conditionObj.IfFalse);
                }
                fieldInfo = new LightConditionDataFieldInfo(query, ifTrueValue, ifFalseValue);
                return true;
            }
            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
        }

        private static bool ParseArguments(ReadOnlyCollection<Expression> arguments, LambdaState state, out object[] argObjects, out DataFieldInfo fieldInfo)
        {
            fieldInfo = null;
            argObjects = null;
            if (arguments.Count == 0) {
                return false;
            }
            object[] array = new object[arguments.Count];
            bool hasFieldInfo = false;
            for (int i = 0; i < arguments.Count; i++) {
                Expression arg = arguments[i];
                if (ParseDataFieldInfo(arg, state, out DataFieldInfo argFieldInfo)) {
                    CheckFieldInfo(argFieldInfo);
                    hasFieldInfo = true;
                    if (Object.Equals(fieldInfo, null)) {
                        fieldInfo = argFieldInfo;
                    }
                    array[i] = argFieldInfo;
                }
                else {
                    array[i] = ConvertObject(arg);
                }
            }
            if (!hasFieldInfo) {
                argObjects = array;
                return false;
            }
            else {
                argObjects = array;
                return true;
            }
        }

        private static bool CheckCatchOperatorsType(ExpressionType expressionType, out CatchOperatorsType catchType)
        {
            if (expressionType == ExpressionType.And || expressionType == ExpressionType.AndAlso) {
                catchType = CatchOperatorsType.AND;
                return true;
            }
            else if (expressionType == ExpressionType.Or || expressionType == ExpressionType.OrElse) {
                catchType = CatchOperatorsType.OR;
                return true;
            }
            else {
                catchType = CatchOperatorsType.AND;
                return false;
            }
        }

        private static bool CheckQueryPredicate(ExpressionType expressionType, out QueryPredicate queryPredicate)
        {
            bool ret;
            switch (expressionType) {
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
            switch (expressionType) {
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
            switch (member.Name) {
                case "Date":
                    fieldInfo = new LightDateDataFieldInfo(fieldInfo);
                    break;
                default:
                    DatePart datePart;
                    if (Enum.TryParse<DatePart>(member.Name, out datePart)) {
                        fieldInfo = new LightDatePartDataFieldInfo(fieldInfo, datePart);
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.MemberExpressionMemberUnsupport, "DateTime", member.Name);
                    }
                    break;
            }
            return fieldInfo;
        }

        private static DataFieldInfo CreateStringMemberDataFieldInfo(MemberInfo member, DataFieldInfo fieldInfo)
        {
            switch (member.Name) {
                case "Length":
                    fieldInfo = new LightStringLengthDataFieldInfo(fieldInfo);
                    break;
                default:
                    throw new LambdaParseException(LambdaParseMessage.MemberExpressionMemberUnsupport, "string", member.Name);
            }
            return fieldInfo;
        }

        private static DataFieldInfo ParseMathFunctionDataFieldInfo(MethodInfo method, DataFieldInfo mainFieldInfo, object[] argObjects, LambdaState state)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();
            if (Enum.TryParse(method.Name, out MathFunction mathFunction)) {
                if (parameterInfos == null || parameterInfos.Length == 0) {
                    throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "Math", method.Name);
                }
                else if (mathFunction == MathFunction.Atan2 || mathFunction == MathFunction.Max || mathFunction == MathFunction.Min || mathFunction == MathFunction.Pow) {
                    if (parameterInfos.Length != 2) {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "Math", method.Name);
                    }
                }
                else if (mathFunction == MathFunction.Log || mathFunction == MathFunction.Round) {
                    if (parameterInfos.Length > 2) {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "Math", method.Name);
                    }
                }
                return new LightMathFunctionDataFieldInfo(mainFieldInfo.TableMapping, mathFunction, argObjects);
            }
            else {
                throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport, "Math", method.Name);
            }
        }

        private static DataFieldInfo ParseStaticStringFunctionDataFieldInfo(MethodInfo method, DataFieldInfo mainFieldInfo, object[] argObjects, LambdaState state)
        {
            if (method.Name == "Concat") {
                if (argObjects.Length == 1) {
                    LightNewArrayDataFieldInfo newarray = argObjects[0] as LightNewArrayDataFieldInfo;
                    if (!Object.Equals(newarray, null)) {
                        return new LightStringConcatDataFieldInfo(newarray.TableMapping, newarray.Values);
                    }
                    else {
                        return new LightStringConcatDataFieldInfo(mainFieldInfo.TableMapping, argObjects);
                    }
                }
                else {
                    return new LightStringConcatDataFieldInfo(mainFieldInfo.TableMapping, argObjects);
                }
            }
            throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport, "string", method.Name);
        }

        private static DataFieldInfo ParseInstanceStringFunctionDataFieldInfo(MethodInfo method, DataFieldInfo mainFieldInfo, object callObject, object[] argObjects, LambdaState state)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();
            switch (method.Name) {
                case "StartsWith":
                    if (parameterInfos.Length != 1) {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string", method.Name);
                    }
                    return new LightStringMatchDataFieldInfo(mainFieldInfo.TableMapping, false, true, callObject, argObjects[0]);
                case "EndsWith":
                    if (parameterInfos.Length != 1) {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string", method.Name);
                    }
                    return new LightStringMatchDataFieldInfo(mainFieldInfo.TableMapping, true, false, callObject, argObjects[0]);
                case "Contains":
                    if (parameterInfos.Length != 1) {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string", method.Name);
                    }
                    return new LightStringMatchDataFieldInfo(mainFieldInfo.TableMapping, true, true, callObject, argObjects[0]);
                default:
                    StringFunction stringFunction;
                    if (Enum.TryParse<StringFunction>(method.Name, out stringFunction)) {
                        if (stringFunction == StringFunction.IndexOf && !(parameterInfos.Length == 1 || (parameterInfos.Length == 2 && parameterInfos[1].ParameterType == typeof(int)))) {
                            throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string", method.Name);
                        }
                        else if (stringFunction == StringFunction.Trim && parameterInfos.Length > 0) {
                            throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "string", method.Name);
                        }
                        return new LightStringFunctionDataFieldInfo(mainFieldInfo.TableMapping, stringFunction, callObject, argObjects);
                    }

                    break;
            }

            throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport, "string", method.Name);
        }

        private static DataFieldInfo ParseInstanceDateTimeFunctionDataFieldInfo(MethodInfo method, DataFieldInfo mainFieldInfo, object callObject, object[] argObjects, LambdaState state)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();
            if (method.Name == "ToString") {
                if (parameterInfos.Length == 0) {
                    return new LightDateFormatDataFieldInfo(mainFieldInfo, null);
                }
                else if (parameterInfos.Length == 1) {
                    if (parameterInfos[0].ParameterType == typeof(string)) {
                        object o = ConvertLambdaObject(argObjects[0]);
                        return new LightDateFormatDataFieldInfo(mainFieldInfo, o as string);
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "DateTime", method.Name);
                    }
                }
                else {
                    throw new LambdaParseException(LambdaParseMessage.MethodExpressionArgumentError, "DateTime", method.Name);
                }
            }
            throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport, "DateTime", method.Name);
        }

        private static DataFieldInfo ParseContainsDataFieldInfo(MethodInfo methodInfo, DataFieldInfo mainFieldInfo, object collections, LambdaState state)
        {
            return new LightContainsDataFieldInfo(mainFieldInfo, collections);
        }

        private static DataFieldInfo ParseExtendQueryData(MethodCallExpression expression, LambdaState state)
        {
            MethodInfo method = expression.Method;

            ReadOnlyCollection<Expression> paramExpressions = expression.Arguments;
            DataFieldInfo data = null;
            switch (method.Name) {
                case "Exists": {
                        UnaryExpression unaryExpresion = paramExpressions[0] as UnaryExpression;
                        if (unaryExpresion == null) {
                            throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                        }
                        LambdaExpression lambdaExpresion = unaryExpresion.Operand as LambdaExpression;
                        if (lambdaExpresion == null) {
                            throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                        }
                        RelateParameterLambdaState mstate = new RelateParameterLambdaState(lambdaExpresion.Parameters[0], state);
                        QueryExpression query = ResolveQueryExpression(lambdaExpresion.Body, mstate);
                        query.MutliQuery = state.MutliEntity;
                        data = new LightExistsDataFieldInfo(mstate.MainMapping, query, true);
                    }
                    break;
                case "In": {
                        if (!ParseDataFieldInfo(paramExpressions[0], state, out DataFieldInfo fieldInfo)) {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                        }
                        UnaryExpression unaryExpresion = paramExpressions[1] as UnaryExpression;
                        if (unaryExpresion == null) {
                            throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                        }
                        LambdaExpression lambdaExpresion = unaryExpresion.Operand as LambdaExpression;
                        if (lambdaExpresion == null) {
                            throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                        }
                        RelateParameterLambdaState mstate = new RelateParameterLambdaState(lambdaExpresion.Parameters[0], state);
                        if (!ParseDataFieldInfo(lambdaExpresion.Body, mstate, out DataFieldInfo selectFieldInfo)) {
                            throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                        }

                        QueryExpression query = null;
                        if (paramExpressions.Count == 3) {
                            UnaryExpression unaryExpresion1 = paramExpressions[2] as UnaryExpression;
                            if (unaryExpresion1 == null) {
                                throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                            }
                            LambdaExpression lambdaExpresion1 = unaryExpresion1.Operand as LambdaExpression;
                            if (lambdaExpresion == null) {
                                throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                            }
                            query = ResolveQueryExpression(lambdaExpresion1.Body, mstate);
                            query.MutliQuery = state.MutliEntity;
                        }
                        data = new LightInQueryDataFieldInfo(mstate.MainMapping, fieldInfo, selectFieldInfo, query, true);
                    }
                    break;
                case "GtAll":
                case "LtAll":
                case "GtAny":
                case "LtAny":
                case "GtEqAll":
                case "LtEqAll":
                case "GtEqAny":
                case "LtEqAny": {
                        if (Enum.TryParse(method.Name, out QueryCollectionPredicate predicate)) {
                            if (!ParseDataFieldInfo(paramExpressions[0], state, out DataFieldInfo fieldInfo)) {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                            }
                            UnaryExpression unaryExpresion = paramExpressions[1] as UnaryExpression;
                            if (unaryExpresion == null) {
                                throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                            }
                            LambdaExpression lambdaExpresion = unaryExpresion.Operand as LambdaExpression;
                            if (lambdaExpresion == null) {
                                throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                            }
                            RelateParameterLambdaState mstate = new RelateParameterLambdaState(lambdaExpresion.Parameters[0], state);
                            if (!ParseDataFieldInfo(lambdaExpresion.Body, mstate, out DataFieldInfo selectFieldInfo)) {
                                throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                            }

                            QueryExpression query = null;
                            if (paramExpressions.Count == 3) {
                                UnaryExpression unaryExpresion1 = paramExpressions[2] as UnaryExpression;
                                if (unaryExpresion1 == null) {
                                    throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                                }
                                LambdaExpression lambdaExpresion1 = unaryExpresion1.Operand as LambdaExpression;
                                if (lambdaExpresion == null) {
                                    throw new LambdaParseException(LambdaParseMessage.ExtendExpressionError);
                                }
                                query = ResolveQueryExpression(lambdaExpresion1.Body, mstate);
                                query.MutliQuery = state.MutliEntity;
                            }
                            data = new LightSubQueryDataFieldInfo(mstate.MainMapping, fieldInfo, selectFieldInfo, predicate, query);
                        }
                        else {
                            throw new LambdaParseException(LambdaParseMessage.MethodExpressionMethodUnsupport, "ExtendQuery", method.Name);
                        }
                    }
                    break;
                case "IsNull": {
                        if (!ParseDataFieldInfo(paramExpressions[0], state, out DataFieldInfo fieldInfo)) {
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
            MethodInfo method = expression.Method;

            ReadOnlyCollection<Expression> paramExpressions = expression.Arguments;
            DataFieldInfo data = null;

            if (paramExpressions.Count == 0) {
                switch (method.Name) {
                    case "Count":
                    case "LongCount":
                        data = new LightAggregateCountDataFieldInfo();
                        break;
                }
            }
            else if (paramExpressions.Count == 1) {
                if (method.Name == "CountCondition" || method.Name == "LongCountCondition") {
                    QueryExpression queryExpression = ResolveQueryExpression(paramExpressions[0], state);
                    data = new LightAggregateCountDataFieldInfo(queryExpression);
                }
                else {
                    if (!ParseDataFieldInfo(paramExpressions[0], state, out DataFieldInfo fieldInfo)) {
                        throw new LambdaParseException(LambdaParseMessage.ExpressionNotContainDataField);
                    }
                    switch (method.Name) {
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

        private static QueryExpression ResolveQueryExpression(Expression expression, LambdaState state)
        {
            if (expression is BinaryExpression binaryObj) {
                if (CheckCatchOperatorsType(binaryObj.NodeType, out CatchOperatorsType catchType)) {
                    var left = ResolveQueryExpression(binaryObj.Left, state);
                    var right = ResolveQueryExpression(binaryObj.Right, state);
                    return QueryExpression.Catch(left, catchType, right);
                }
                else {
                    if (CheckQueryPredicate(binaryObj.NodeType, out QueryPredicate queryPredicate)) {
                        object leftValue = null;
                        bool left;
                        if (ParseDataFieldInfo(binaryObj.Left, state, out DataFieldInfo leftFieldInfo)) {
                            left = true;
                            CheckFieldInfo(leftFieldInfo);
                        }
                        else {
                            left = false;
                            leftValue = ConvertObject(binaryObj.Left);
                        }

                        object rightValue = null;
                        bool right;
                        if (ParseDataFieldInfo(binaryObj.Right, state, out DataFieldInfo rightFieldInfo)) {
                            right = true;
                            CheckFieldInfo(rightFieldInfo);
                        }
                        else {
                            right = false;
                            rightValue = ConvertObject(binaryObj.Right);
                        }

                        if (left && right) {
                            return new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate, leftFieldInfo, rightFieldInfo);
                        }
                        else if (left && !right) {
                            return new LightBinaryQueryExpression(leftFieldInfo.TableMapping, queryPredicate, leftFieldInfo, rightValue);
                        }
                        else if (!left && right) {
                            return new LightBinaryQueryExpression(rightFieldInfo.TableMapping, queryPredicate, rightFieldInfo, leftValue);
                        }
                        else {
                            throw new LambdaParseException(LambdaParseMessage.BinaryExpressionNotAllowBothConstantValue);
                        }
                    }
                }
            }
            if (expression is UnaryExpression unaryObj) {
                if (unaryObj.NodeType == ExpressionType.Not) {
                    QueryExpression queryExpression = ResolveQueryExpression(unaryObj.Operand, state);
                    if (queryExpression is ISupportNotDefine notDefine) {
                        notDefine.SetNot();
                        return queryExpression;
                    }
                    else {
                        return new LightNotQueryExpression(queryExpression);
                    }
                }
            }
            if (expression is MemberExpression memberObj) {
                if (!ParseDataFieldInfo(expression, state, out DataFieldInfo fieldInfo)) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, memberObj);
                }
                CheckFieldInfo(fieldInfo);
                IDataFieldInfoConvert convertFieldInfo = fieldInfo as IDataFieldInfoConvert;
                if (Object.Equals(convertFieldInfo, null)) {
                    return new LightBinaryQueryExpression(fieldInfo.TableMapping, QueryPredicate.Eq, fieldInfo, true);
                }
                else {
                    return convertFieldInfo.ConvertToExpression();
                }
            }
            if (expression is MethodCallExpression methodcallObj) {
                if (!ParseDataFieldInfo(expression, state, out DataFieldInfo fieldInfo)) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, methodcallObj);
                }
                CheckFieldInfo(fieldInfo);
                IDataFieldInfoConvert convertFieldInfo = fieldInfo as IDataFieldInfoConvert;
                if (Object.Equals(convertFieldInfo, null)) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, methodcallObj);
                }
                else {
                    return convertFieldInfo.ConvertToExpression();
                }
            }
            if (expression is ConditionalExpression conditionObj) {
                if (!ParseDataFieldInfo(expression, state, out DataFieldInfo fieldInfo)) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, conditionObj);
                }
                CheckFieldInfo(fieldInfo);
                IDataFieldInfoConvert convertFieldInfo = fieldInfo as IDataFieldInfoConvert;
                if (Object.Equals(convertFieldInfo, null)) {
                    throw new LambdaParseException(LambdaParseMessage.ExpressionNotAllowNoDataField, conditionObj);
                }
                else {
                    return convertFieldInfo.ConvertToExpression();
                }
            }
            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
        }

        private static DataFieldExpression ResolveOnExpression(Expression expression, LambdaState state)
        {
            if (expression is BinaryExpression binary) {
                if (CheckCatchOperatorsType(binary.NodeType, out CatchOperatorsType catchType)) {
                    var left = ResolveOnExpression(binary.Left, state);
                    var right = ResolveOnExpression(binary.Right, state);
                    return DataFieldExpression.Catch(left, catchType, right);
                }
                if (CheckQueryPredicate(binary.NodeType, out QueryPredicate queryPredicate)) {
                    bool left;
                    if (ParseDataFieldInfo(binary.Left, state, out DataFieldInfo leftFieldInfo)) {
                        left = true;
                        CheckFieldInfo(leftFieldInfo);
                    }
                    else {
                        left = false;
                    }

                    bool right;
                    if (ParseDataFieldInfo(binary.Right, state, out DataFieldInfo rightFieldInfo)) {
                        right = true;
                        CheckFieldInfo(rightFieldInfo);
                    }
                    else {
                        right = false;
                    }

                    if (left && right) {
                        return new DataFieldMatchExpression(leftFieldInfo, rightFieldInfo, queryPredicate);
                    }
                    else {
                        throw new LambdaParseException(LambdaParseMessage.BinaryExpressionNotAllowBothConstantValue);
                    }
                }
            }
            throw new LambdaParseException(LambdaParseMessage.ExpressionTypeInvalid);
        }
    }
}

