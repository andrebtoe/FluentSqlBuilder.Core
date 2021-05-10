using FluentSqlBuilder.Core.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlBuilderFluent.Core.Extensions.Common;
using SqlBuilderFluent.Exceptions;
using SqlBuilderFluent.Helpers;
using SqlBuilderFluent.Lambdas;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Types;
using System;
using System.Linq.Expressions;

namespace FluentSqlBuilder.Playground.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddFluentSqlBuilder(x =>
            {
                x.Formatting = SqlBuilderFormatting.Indented;
                x.SqlAdapterType = SqlAdapterType.SqlServer2014;

                x.AddProvider<DateTimeProvider>(typeof(DateTime));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }

    public class DateTimeProvider : IFluentSqlBuilderProvider
    {
        public Node GetNodeResolvedByProperty(MemberExpression memberExpression)
        {
            if (memberExpression.NodeType == ExpressionType.MemberAccess)
            {
                var memberName = memberExpression.Member.Name;

                if (memberName == "Now")
                {
                    var literalValue = "GETDATE()";
                    var valueNode = new ValueNode(literalValue, true);

                    return valueNode;
                }
                else if (memberName == "UtcNow")
                {
                    var literalValue = "GETUTCDATE()";
                    var valueNode = new ValueNode(literalValue, true);

                    return valueNode;
                }
                else if (memberName == "Today")
                {
                    var literalValue = "DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)";
                    var valueNode = new ValueNode(literalValue, true);

                    return valueNode;
                }
                else if (memberExpression.Expression.NodeType == ExpressionType.Parameter)
                {
                    var tableName = LambdaResolverExtension.GetTableName(memberExpression);
                    var columnName = SqlBuilderFluentHelper.GetColumnName(memberExpression);
                    var memberNode = new MemberNode(tableName, columnName);

                    return memberNode;
                }

                return null;
            }

            throw new SqlBuilderException("Expected member expression");
        }

        public Node GetNodeResolvedByMethod(MethodCallExpression callExpression)
        {
            var methodName = callExpression.Method.Name;
            var argumentsToUse = String.Join(',', callExpression.Arguments);
            var memberExpressionTableAndColumn = callExpression.Object as MemberExpression;
            var columnToUseInFunction = GetColumAndTablenWithConvention(memberExpressionTableAndColumn);
            var nameFunctionSql = String.Empty;

            switch (methodName)
            {
                case "AddDays":
                    nameFunctionSql = $"DATEADD(DAY, {argumentsToUse}, {columnToUseInFunction})";
                    break;
                case "AddHours":
                    nameFunctionSql = $"DATEADD(HOUR, {argumentsToUse}, {columnToUseInFunction})";
                    break;
                case "AddMilliseconds":
                    nameFunctionSql = $"DATEADD(MILLISECOND, {argumentsToUse}, {columnToUseInFunction})";
                    break;
                case "AddMinutes":
                    nameFunctionSql = $"DATEADD(MINUTE, {argumentsToUse}, {columnToUseInFunction})";
                    break;
                case "AddMonths":
                    nameFunctionSql = $"DATEADD(MONTH, {argumentsToUse}, {columnToUseInFunction})";
                    break;
                case "AddSeconds":
                    nameFunctionSql = $"DATEADD(SECOND, {argumentsToUse}, {columnToUseInFunction})";
                    break;
                case "AddYears":
                    nameFunctionSql = $"DATEADD(YEAR, {argumentsToUse}, {columnToUseInFunction})";
                    break;
            }

            if(String.IsNullOrEmpty(nameFunctionSql))
                throw new SqlBuilderException($"'{methodName}' not implemented");

            var valueNode = new ValueNode(nameFunctionSql, true);
            var valueMethodNode = new ValueMethodNode(nameFunctionSql, true);

            return valueMethodNode;
        }

        private string GetColumAndTablenWithConvention(MemberExpression memberExpressionTableAndColumn)
        {
            var node = GetNodeResolvedByProperty(memberExpressionTableAndColumn);
            var nodeAsMemberNode = node as MemberNode;
            var nodeAsValueNode = node as ValueNode;

            if (nodeAsMemberNode != null)
            {
                var tableName = LambdaResolverExtension.GetTableName(memberExpressionTableAndColumn);
                var columnName = SqlBuilderFluentHelper.GetColumnName(memberExpressionTableAndColumn);
                var columAndTablenWithConvention = $"{tableName}.{columnName}";

                return columAndTablenWithConvention;
            }
            else if (nodeAsValueNode != null)
            {
                var columnWithValueLiteral = nodeAsValueNode.Value.ToString();

                return columnWithValueLiteral;
            }

            return "";
        }
    }
}