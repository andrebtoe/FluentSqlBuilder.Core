using SqlBuilderFluent.Exceptions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlBuilderFluent.Helpers
{
    public static class SqlBuilderFluentHelper
    {
        public static bool HasColumnAttribute(MemberInfo memberInfo)
        {
            var hasColumnAttribute = memberInfo.GetCustomAttributes(false).OfType<ColumnAttribute>().Any();

            return hasColumnAttribute;
        }

        public static bool HasColumnAttribute(Expression expression)
        {
            var member = GetMemberExpression(expression);
            var hasColumnAttribute = HasColumnAttribute(member.Member);

            return hasColumnAttribute;
        }

        public static string GetColumnName(MemberInfo memberInfo)
        {
            var columnAttribute = memberInfo.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

            if (columnAttribute != null)
                return columnAttribute.Name;
            else
                return memberInfo.Name;
        }

        public static string GetColumnNameWithAttribute(Expression expression)
        {
            var member = GetMemberExpression(expression);
            var nameProperty = member.Member.Name;

            return nameProperty;
        }

        public static string GetColumnName<TTable>(Expression<Func<TTable, object>> selector)
        {
            var memberExpression = GetMemberExpression(selector.Body);
            var columnName = GetColumnName(memberExpression);

            return columnName;
        }

        public static string GetColumnName(Expression expression)
        {
            var member = GetMemberExpression(expression);
            var columnName = GetColumnName(member.Member);

            return columnName;
        }

        public static string GetTableName<TTable>()
        {
            var tableName = GetTableName(typeof(TTable));

            return tableName;
        }

        public static string GetTableName(Type type)
        {
            var tableAttribute = type.GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();

            if (tableAttribute != null)
                return tableAttribute.Name;
            else
                return type.Name;
        }

        public static string GetSchema(Type type)
        {
            var tableAttribute = type.GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();

            if (tableAttribute != null)
                return tableAttribute.Schema;

            return null;
        }

        private static MemberExpression GetMemberExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberAccessExpression = expression as MemberExpression;

                    return memberAccessExpression;
                case ExpressionType.Convert:
                case ExpressionType.Lambda:
                    var unaryExpression = expression as UnaryExpression;
                    var operand = unaryExpression.Operand;
                    var memberExpressionLambda = GetMemberExpression(operand);

                    return memberExpressionLambda;
            }

            throw new SqlBuilderException("Member expression expected");
        }
    }
}