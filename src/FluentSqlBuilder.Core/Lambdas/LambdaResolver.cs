using FluentSqlBuilder.Core.Inputs;
using SqlBuilderFluent.Core.Extensions.Common;
using SqlBuilderFluent.Exceptions;
using SqlBuilderFluent.Helpers;
using SqlBuilderFluent.Inputs;
using SqlBuilderFluent.Lambdas.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SqlBuilderFluent.Lambdas
{
    public interface IFluentSqlBuilderProvider
    {
        Node GetNodeResolved(MemberExpression memberExpression);
    }

    public class LambdaResolver
    {
        private readonly FluentSqlQueryBuilder _sqlQueryBuilder;
        private readonly LambdaResolverExtension _lambdaResolverExtension;
        private readonly IDictionary<Type, Type> _providers;

        public LambdaResolver(FluentSqlQueryBuilder sqlQueryBuilder, IDictionary<Type, Type> providers, string tableAlias)
        {
            _sqlQueryBuilder = sqlQueryBuilder;
            _providers = providers;
            _sqlQueryBuilder.DefineTableNameAliasOverride(tableAlias);

            _lambdaResolverExtension = new LambdaResolverExtension(sqlQueryBuilder);
        }

        public void AddClauseIn<TTable>(Expression<Func<TTable, object>> expression, List<object> values, string tableAlias, TargetClauseType targetClauseType)
        {
            var columnName = SqlBuilderFluentHelper.GetColumnName(expression);
            var tableName = SqlBuilderFluentHelper.GetTableName<TTable>();

            _sqlQueryBuilder.AddClauseIn(tableName, columnName, values, tableAlias, targetClauseType, null);
        }

        public void AddClauseNotIn<TTable>(Expression<Func<TTable, object>> expression, List<object> values, string tableAlias, TargetClauseType targetClauseType)
        {
            var columnName = SqlBuilderFluentHelper.GetColumnName(expression);
            var tableName = SqlBuilderFluentHelper.GetTableName<TTable>();

            _sqlQueryBuilder.AddOperatorNot(targetClauseType);
            _sqlQueryBuilder.AddClauseNotIn(tableName, columnName, values, tableAlias, targetClauseType, null);
        }

        public void AddJoinByType<TTableLeft, TTableRight>(Expression<Func<TTableLeft, TTableRight, bool>> expression, JoinType joinType, string tableAlias = null)
        {
            var joinExpression = LambdaResolverExtension.GetBinaryExpression(expression.Body);
            var leftMemberExpression = _lambdaResolverExtension.GetMemberExpression(joinExpression.Left);
            var rightMemberExpression = _lambdaResolverExtension.GetMemberExpression(joinExpression.Right);
            var tableSchemaName = SqlBuilderFluentHelper.GetSchema(typeof(TTableRight));
            var tableName = SqlBuilderFluentHelper.GetTableName<TTableLeft>();
            var tableNameToJoin = SqlBuilderFluentHelper.GetTableName<TTableRight>();
            var columnNameLeft = SqlBuilderFluentHelper.GetColumnName(leftMemberExpression);
            var columnNameRight = SqlBuilderFluentHelper.GetColumnName(rightMemberExpression);

            _sqlQueryBuilder.AddJoinByType(tableSchemaName, tableName, tableNameToJoin, columnNameLeft, columnNameRight, joinType, tableAlias);
        }

        public void AddOrderBy<TTable>(bool descending, string tableAlias, params Expression<Func<TTable, object>>[] expressions)
        {
            var columnsNameToOrderBy = new List<string>();
            var tableName = SqlBuilderFluentHelper.GetTableName<TTable>();

            foreach (var expressionItem in expressions)
            {
                var memberExpression = _lambdaResolverExtension.GetMemberExpression(expressionItem.Body);
                var columnName = SqlBuilderFluentHelper.GetColumnName(memberExpression);

                columnsNameToOrderBy.Add(columnName);
            }

            _sqlQueryBuilder.AddOrderBy(tableName, columnsNameToOrderBy, descending, tableAlias);
        }

        public void AddLimit(int count)
        {
            _sqlQueryBuilder.DefineLimit(count);
        }

        public void AddPagination(int pageSize, int? pageNumber = null)
        {
            _sqlQueryBuilder.DefinePageSize(pageSize);
            _sqlQueryBuilder.DefinePageNumber(pageNumber);
        }

        public void Select<TTableProjection>(Expression<Func<TTableProjection, object>> expression, string tableAlias, Type tableToProjection)
        {
            var expressionBody = expression.Body;

            switch (expressionBody.NodeType)
            {
                case ExpressionType.Parameter:
                    _lambdaResolverExtension.BuildSelectByParameter<TTableProjection>(expressionBody, tableAlias, tableToProjection);
                    break;
                case ExpressionType.Convert:
                case ExpressionType.MemberAccess:
                    _lambdaResolverExtension.SelectByMemberAccess<TTableProjection>(expressionBody, tableAlias, tableToProjection);
                    break;
                case ExpressionType.New:
                    _lambdaResolverExtension.SelectByAnonymous<TTableProjection>(expressionBody, tableAlias, tableToProjection);
                    break;
                default:
                    throw new SqlBuilderException("Invalid expression");
            }
        }

        internal void SelectWithFunctionWithoutColumn<TTable>(SelectFunction selectFunction, string tableAlias, string columnAlias)
        {
            var tableName = SqlBuilderFluentHelper.GetTableName<TTable>();

            _sqlQueryBuilder.Select(tableName, null, selectFunction, tableAlias, columnAlias);
        }

        internal void SelectWithFunction<TTable>(Expression<Func<TTable, object>> expression, SelectFunction selectFunction, string tableAlias, string columnAlias)
        {
            _lambdaResolverExtension.SelectWithFunction<TTable>(expression.Body, selectFunction, tableAlias, columnAlias);
        }

        public void AddGroupBy<TTable>(string tableAlias, params Expression<Func<TTable, object>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                var memberExpression = _lambdaResolverExtension.GetMemberExpression(expression.Body);

                _lambdaResolverExtension.GroupBy<TTable>(memberExpression, tableAlias);
            }
        }

        public void ResolveQuery<TTable>(Expression<Func<TTable, bool>> expression, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            var expressionTree = GetExpressionTree(expression);

            _lambdaResolverExtension.BuildSelect(expressionTree, tableAlias, targetClauseType, selectFunction);
        }

        public void AddHaving<TTable>(string tableAlias, Expression<Func<TTable, bool>> expressionFilter, TargetClauseType targetClauseType, SelectFunction selectFunction)
        {
            var expressionTree = GetExpressionTree(expressionFilter);

            _lambdaResolverExtension.BuildSelect(expressionTree, tableAlias, targetClauseType, selectFunction);
        }

        internal Node ResolveQuery(BinaryExpression binaryExpression)
        {
            var @operator = binaryExpression.NodeType;
            dynamic binaryExpressionLeft = binaryExpression.Left;
            dynamic binaryExpressionRight = binaryExpression.Right;
            dynamic queryLeft = ResolveQuery(binaryExpressionLeft);
            dynamic queryRight = ResolveQuery(binaryExpressionRight);
            var operationNode = new OperationNode(@operator, queryLeft, queryRight);

            return operationNode;
        }

        private static Node ResolveQuery(ConstantExpression constantExpression)
        {
            var valueNode = new ValueNode(constantExpression.Value, false);

            return valueNode;
        }

        private Node ResolveQuery(UnaryExpression unaryExpression)
        {
            dynamic operand = unaryExpression.Operand;
            dynamic nodeQuery = ResolveQuery(operand);
            var singleOperationNode = new SingleOperationNode(unaryExpression.NodeType, nodeQuery);

            return singleOperationNode;
        }

        private Node ResolveQuery(MethodCallExpression callExpression)
        {
            var functionParsed = Enum.TryParse(callExpression.Method.Name, true, out LikeMethod callFunction);

            if (functionParsed)
            {
                var memberExpression = callExpression.Object as MemberExpression;
                var argumentFunction = callExpression.Arguments.First();
                var columnValue = (string)_lambdaResolverExtension.GetExpressionValue(argumentFunction);
                var tableName = LambdaResolverExtension.GetTableName(memberExpression);
                var columnName = SqlBuilderFluentHelper.GetColumnName(callExpression.Object);
                var memberNode = new MemberNode(tableName, columnName);
                var likeNode = new LikeNode(callFunction, memberNode, columnValue);

                return likeNode;
            }
            else
            {
                var value = _lambdaResolverExtension.ResolveMethodCall(callExpression);
                var valueNode = new ValueNode(value, false);

                return valueNode;
            }
        }

        private Node ResolveQuery(MemberExpression memberExpression, MemberExpression rootExpression = null)
        {
            var rootExpressionToUse = rootExpression;

            if (rootExpressionToUse == null)
                rootExpressionToUse = memberExpression;

            var memberType = memberExpression.Type;
            var providerExistsByType = _providers.ContainsKey(memberType);
            Node nodeValue;

            if (!providerExistsByType)
                nodeValue = ResolveOperatorCommon(memberExpression, rootExpressionToUse);
            else
                nodeValue = ResolveOperatorByProvider(memberExpression, memberType);

            return nodeValue;
        }

        private Node ResolveOperatorCommon(MemberExpression memberExpression, MemberExpression rootExpressionToUse)
        {
            switch (memberExpression.Expression.NodeType)
            {
                case ExpressionType.Parameter:
                    var tableName = LambdaResolverExtension.GetTableName(rootExpressionToUse);
                    var columnName = SqlBuilderFluentHelper.GetColumnName(rootExpressionToUse);
                    var memberNode = new MemberNode(tableName, columnName);

                    return memberNode;
                case ExpressionType.MemberAccess:
                    var memberExpressionExpression = memberExpression.Expression as MemberExpression;
                    var nodeQuery = ResolveQuery(memberExpressionExpression, rootExpressionToUse);

                    return nodeQuery;
                case ExpressionType.Call:
                case ExpressionType.Constant:
                    var value = _lambdaResolverExtension.GetExpressionValue(rootExpressionToUse);
                    var valueNode = new ValueNode(value, false);

                    return valueNode;
                default:
                    throw new SqlBuilderException("Expected member expression");
            }
        }

        private Node ResolveOperatorByProvider(MemberExpression memberExpression, Type memberType)
        {
            var providerByType = _providers[memberType];
            var instanceProvider = Activator.CreateInstance(providerByType) as IFluentSqlBuilderProvider;
            var node = instanceProvider.GetNodeResolved(memberExpression);

            return node;
        }

        private dynamic GetExpressionTree<TTable>(Expression<Func<TTable, bool>> expression)
        {
            dynamic expressionBody = expression.Body;
            dynamic expressionTree = ResolveQuery(expressionBody);

            return expressionTree;
        }
    }
}