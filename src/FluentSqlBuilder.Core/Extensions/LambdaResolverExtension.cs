using FluentSqlBuilder.Core.Inputs;
using SqlBuilderFluent.Exceptions;
using SqlBuilderFluent.Helpers;
using SqlBuilderFluent.Lambdas.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlBuilderFluent.Core.Extensions
{
    public class LambdaResolverExtension
    {
        private readonly FluentSqlQueryBuilder _sqlQueryBuilder;
        private readonly Dictionary<ExpressionType, string> _operations = new()
        {
            { ExpressionType.Equal, "=" },
            { ExpressionType.NotEqual, "!=" },
            { ExpressionType.GreaterThan, ">" },
            { ExpressionType.LessThan, "<" },
            { ExpressionType.GreaterThanOrEqual, ">=" },
            { ExpressionType.LessThanOrEqual, "<=" }
        };

        public LambdaResolverExtension(FluentSqlQueryBuilder sqlQueryBuilder)
        {
            _sqlQueryBuilder = sqlQueryBuilder;
        }

        internal static string GetValueToLike(LikeNode likeNode)
        {
            var value = likeNode.Value;

            switch (likeNode.Method)
            {
                case LikeMethod.StartsWith:
                    value = $"{likeNode.Value}%";
                    break;
                case LikeMethod.EndsWith:
                    value = $"%{likeNode.Value}";
                    break;
                case LikeMethod.Contains:
                    value = $"%{likeNode.Value}%";
                    break;
            }

            return value;
        }

        public MemberExpression GetMemberExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberExpressionAccess = expression as MemberExpression;

                    return memberExpressionAccess;
                case ExpressionType.Convert:
                case ExpressionType.Lambda:
                    var unaryExpression = expression as UnaryExpression;
                    var operand = unaryExpression.Operand;
                    var memberExpressionLambda = GetMemberExpression(operand);

                    return memberExpressionLambda;
            }

            throw new SqlBuilderException("Member expression expected");
        }

        public static BinaryExpression GetBinaryExpression(Expression expression)
        {
            if (expression is BinaryExpression)
                return expression as BinaryExpression;

            throw new SqlBuilderException("Binary expression expected");
        }

        public void ResolveOperation(ExpressionType operation, TargetClauseType targetClauseType)
        {
            switch (operation)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _sqlQueryBuilder.AddOperatorAnd(targetClauseType);
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _sqlQueryBuilder.AddOperatorOr(targetClauseType);
                    break;
                default:
                    throw new SqlBuilderException($"Unrecognized binary expression operation '{operation}'");
            }
        }

        internal void ResolveNullValue(MemberNode memberNode, ExpressionType operation, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            switch (operation)
            {
                case ExpressionType.Equal:
                    _sqlQueryBuilder.AddClauseIsNull(memberNode.TableName, memberNode.ColumnName, tableAlias, targetClauseType, selectFunction);
                    break;
                case ExpressionType.NotEqual:
                    _sqlQueryBuilder.AddClauseIsNotNull(memberNode.TableName, memberNode.ColumnName, tableAlias, targetClauseType, selectFunction);
                    break;
            }
        }

        public void GroupBy<TTable>(MemberExpression expression, string tableAlias)
        {
            var memberExpression = GetMemberExpression(expression);
            var columnName = SqlBuilderFluentHelper.GetColumnName(memberExpression);
            var notExistsTableAlias = String.IsNullOrEmpty(tableAlias);
            var tableName = notExistsTableAlias ? SqlBuilderFluentHelper.GetTableName<TTable>() : tableAlias;

            _sqlQueryBuilder.AddGroupBy(tableName, columnName);
        }

        public void BuildSelectByParameter<TTable>(Expression expression, string tableAlias, Type tableToProjection)
        {
            var tableName = SqlBuilderFluentHelper.GetTableName(expression.Type);
            var addTableAlias = !String.IsNullOrEmpty(tableAlias);

            if (addTableAlias)
                tableName = tableAlias;

            _sqlQueryBuilder.Select(tableName, tableToProjection);
        }

        public void SelectByMemberAccess<TTable>(Expression expression, string tableAlias, Type tableToProjection)
        {
            var memberExpression = GetMemberExpression(expression);

            Select<TTable>(memberExpression, tableAlias, tableToProjection);
        }

        public void SelectByAnonymous<TTable>(Expression expression, string tableAlias, Type tableToProjection)
        {
            var newExpression = expression as NewExpression;
            var arguments = newExpression.Arguments;

            foreach (MemberExpression memberExpression in arguments)
                Select<TTable>(memberExpression, tableAlias, tableToProjection);
        }

        public static object ResolveValue(PropertyInfo property, object instance)
        {
            var value = property.GetValue(instance, null);

            return value;
        }

        public static object ResolveValue(FieldInfo field, object instance)
        {
            var value = field.GetValue(instance);

            return value;
        }

        public object GetExpressionValue(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return (expression as ConstantExpression).Value;
                case ExpressionType.Call:
                    return ResolveMethodCall(expression as MethodCallExpression);
                case ExpressionType.MemberAccess:
                    var memberExpr = (expression as MemberExpression);
                    var expressionValue = GetExpressionValue(memberExpr.Expression);
                    dynamic member = memberExpr.Member;
                    var value = ResolveValue(member, expressionValue);

                    return value;
                default:
                    throw new SqlBuilderException("Expected constant expression");
            }
        }

        public object ResolveMethodCall(MethodCallExpression callExpression)
        {
            var arguments = callExpression.Arguments.Select(GetExpressionValue).ToArray();
            var instanceIsNotNull = callExpression.Object != null;
            var instance = instanceIsNotNull ? GetExpressionValue(callExpression.Object) : arguments.First();
            var returnInvokedMethod = callExpression.Method.Invoke(instance, arguments);

            return returnInvokedMethod;
        }

        public static string GetTableName(MemberExpression expression)
        {
            var tableName = SqlBuilderFluentHelper.GetTableName(expression.Member.DeclaringType);

            return tableName;
        }

        internal void SelectWithFunction<TTable>(Expression expression, SelectFunction selectFunction, string tableAlias, string columnAlias)
        {
            var memberExpression = GetMemberExpression(expression);
            var columnName = SqlBuilderFluentHelper.GetColumnName(memberExpression);
            var existsTableAlias = String.IsNullOrEmpty(tableAlias);
            var tableName = existsTableAlias ? SqlBuilderFluentHelper.GetTableName<TTable>() : tableAlias;

            _sqlQueryBuilder.Select(tableName, columnName, selectFunction, tableAlias, columnAlias);
        }

        private void Select<TTable>(MemberExpression expression, string tableAlias, Type? tableToProjection = null)
        {
            var isClass = expression.Type.IsClass && expression.Type != typeof(String);
            var addTableAlias = !String.IsNullOrEmpty(tableAlias);

            if (isClass)
            {
                var tableTypeToUse = tableToProjection != null ? tableToProjection : expression.Type;
                var tableName = SqlBuilderFluentHelper.GetTableName(tableTypeToUse);

                if (addTableAlias)
                    tableName = tableAlias;

                _sqlQueryBuilder.Select(tableName);
            }
            else
            {
                var tableName = tableToProjection != null ? SqlBuilderFluentHelper.GetTableName(tableToProjection) : SqlBuilderFluentHelper.GetTableName<TTable>();
                var columnName = SqlBuilderFluentHelper.GetColumnName(expression);
                var hasColumnAttribute = SqlBuilderFluentHelper.HasColumnAttribute(expression);
                var columnNameAlias = hasColumnAttribute ? SqlBuilderFluentHelper.GetColumnNameWithAttribute(expression) : null;

                if (addTableAlias)
                    tableName = tableAlias;

                _sqlQueryBuilder.Select(tableName, columnName, columnNameAlias);
            }
        }

        internal void BuildSelect(OperationNodeType operationNodeType, Node node, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            dynamic nodeDynamic = node;

            BuildSelect(operationNodeType, nodeDynamic, tableAlias, targetClauseType, selectFunction, operationNodeResolveType);
        }

        private void BuildSelect(OperationNodeType operationNodeType, MemberNode memberNode, ValueNode valueNode, ExpressionType operation, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            var valueIsNull = valueNode.Value == null;

            if (valueIsNull)
            {
                ResolveNullValue(memberNode, operation, tableAlias, targetClauseType, selectFunction);
            }
            else if (operationNodeType == OperationNodeType.Common)
            {
                var operationValue = _operations[operation];

                _sqlQueryBuilder.AddClauseByOperation(memberNode.TableName, memberNode.ColumnName, operationValue, valueNode.Value, tableAlias, targetClauseType, selectFunction);
            }
            else if (operationNodeType == OperationNodeType.ColumnWithFunction)
            {
                var operationValue = _operations[operation];

                _sqlQueryBuilder.AddClauseByOperationWithFunction(operationNodeResolveType.GetValueOrDefault(), memberNode.TableName, memberNode.ColumnName, operationValue, valueNode.Value, tableAlias, targetClauseType, selectFunction);
            }
        }

        private void BuildSelect(OperationNodeType operationNodeType, SingleOperationNode leftMember, Node rightMember, ExpressionType operation, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            var operatorIsNot = leftMember.Operator == ExpressionType.Not;

            if (operatorIsNot)
            {
                var left = leftMember as Node;

                BuildSelect(operationNodeType, left, rightMember, operation, tableAlias, targetClauseType, selectFunction, operationNodeResolveType);
            }
            else
            {
                dynamic leftMemberNode = leftMember.Child;
                dynamic rightMemberNode = rightMember;

                BuildSelect(operationNodeType, leftMemberNode, rightMemberNode, operation, tableAlias, targetClauseType, selectFunction, operationNodeResolveType);
            }
        }

        private void BuildSelect(OperationNodeType operationNodeType, LikeNode likeNode, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            if (likeNode.Method == LikeMethod.Equals)
            {
                var operation = _operations[ExpressionType.Equal];

                _sqlQueryBuilder.AddClauseByOperation(likeNode.MemberNode.TableName, likeNode.MemberNode.ColumnName, operation, likeNode.Value, tableAlias, targetClauseType, selectFunction);
            }
            else
            {
                var value = GetValueToLike(likeNode);

                _sqlQueryBuilder.AddClauseLike(likeNode.MemberNode.TableName, likeNode.MemberNode.ColumnName, value, tableAlias, targetClauseType, selectFunction);
            }
        }

        private void BuildSelect(OperationNodeType operationNodeType, OperationNode node, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            dynamic left = node.Left;
            dynamic right = node.Right;

            BuildSelect(operationNodeType, left, right, node.Operator, tableAlias, targetClauseType, selectFunction, operationNodeResolveType);
        }

        private void BuildSelect(OperationNodeType operationNodeType, MemberNode memberNode, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            var operation = _operations[ExpressionType.Equal];

            _sqlQueryBuilder.AddClauseByOperation(memberNode.TableName, memberNode.ColumnName, operation, true, tableAlias, targetClauseType, selectFunction);
        }

        private void BuildSelect(OperationNodeType operationNodeType, SingleOperationNode node, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            var operatorIsNot = node.Operator == ExpressionType.Not;

            if (operatorIsNot)
                _sqlQueryBuilder.AddOperatorNot(targetClauseType);

            BuildSelect(operationNodeType, node.Child, tableAlias, targetClauseType, selectFunction, operationNodeResolveType);
        }

        private void BuildSelect(OperationNodeType operationNodeType, ValueNode valueNode, MemberNode memberNode, ExpressionType operation, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            BuildSelect(operationNodeType, memberNode, valueNode, operation, tableAlias, targetClauseType, selectFunction, operationNodeResolveType);
        }

        private void BuildSelect(OperationNodeType operationNodeType, MemberNode leftMember, MemberNode rightMember, ExpressionType operation, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            var operationValue = _operations[operation];

            _sqlQueryBuilder.AddClauseByOperationComparison(leftMember.TableName, leftMember.ColumnName, operationValue, rightMember.TableName, rightMember.ColumnName, targetClauseType, selectFunction);
        }

        private void BuildSelect(OperationNodeType operationNodeType, Node leftMember, SingleOperationNode rightMember, ExpressionType operation, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            BuildSelect(operationNodeType, rightMember, leftMember, operation, tableAlias, targetClauseType, selectFunction, operationNodeResolveType);
        }

        private void BuildSelect(OperationNodeType operationNodeType, Node leftNode, Node rightNode, ExpressionType operation, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction, OperationNodeResolveType? operationNodeResolveType)
        {
            dynamic leftNodeDynamic = leftNode;
            dynamic rightNodeDynamic = rightNode;

            _sqlQueryBuilder.AddOperatorPrecedenceStart();

            BuildSelect(operationNodeType, leftNodeDynamic, tableAlias, targetClauseType, selectFunction, operationNodeResolveType);
            ResolveOperation(operation, targetClauseType);
            BuildSelect(operationNodeType, rightNodeDynamic, tableAlias, targetClauseType, selectFunction, operationNodeResolveType);

            _sqlQueryBuilder.AddOperatorPrecedenceEnd();
        }
    }
}