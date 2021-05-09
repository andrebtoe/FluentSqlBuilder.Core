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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
        public Node GetNodeResolved(MemberExpression memberExpression, MemberExpression rootExpressionToUse)
        {
            switch (memberExpression.NodeType)
            {
                //case ExpressionType.Parameter:
                //    var tableName = LambdaResolverExtension.GetTableName(rootExpressionToUse);
                //    var columnName = SqlBuilderFluentHelper.GetColumnName(rootExpressionToUse);
                //    var memberNode = new MemberNode(tableName, columnName);

                //    return memberNode;
                case ExpressionType.MemberAccess:
                    //var memberExpressionExpression = memberExpression.Expression as MemberExpression;
                    //var memberExpr = (memberExpression as MemberExpression);

                    var name = memberExpression.Member.Name;
                    if (name == "Now")
                    {
                        var value = "GETDATE()";
                        return new ValueNode(value, true);
                    }
                    else
                    {
                        if (memberExpression.Expression.NodeType == ExpressionType.Parameter)
                        {
                            var tableName = LambdaResolverExtension.GetTableName(memberExpression);
                            var columnName = SqlBuilderFluentHelper.GetColumnName(memberExpression);
                            var memberNode = new MemberNode(tableName, columnName);

                            return memberNode;
                        }
                    }

                    return null;

                //var nodeQuery = ResolveQuery(memberExpressionExpression, rootExpressionToUse);

                //return nodeQuery;
                //case ExpressionType.Call:
                //case ExpressionType.Constant:
                //    var value = _lambdaResolverExtension.GetExpressionValue(rootExpressionToUse);
                //    var valueNode = new ValueNode(value);

                //return valueNode;
                default:
                    throw new SqlBuilderException("Expected member expression");
            }

            throw new NotImplementedException();
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
    }
}