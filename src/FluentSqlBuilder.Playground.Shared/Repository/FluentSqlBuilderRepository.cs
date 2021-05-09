using FluentSqlBuilder.Core.Middlewares.Inputs;
using FluentSqlBuilder.Core.Middlewares.Services;
using FluentSqlBuilder.Data.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Types;
using System;

namespace FluentSqlBuilder.Playground.Shared.Repository
{
    public static class FluentSqlBuilderRepository
    {
        public readonly static FluentSqlBuilderMiddlewareOptions _fluentSqlBuilderMiddlewareOptions = new FluentSqlBuilderMiddlewareOptions()
        {
            SqlAdapterType = SqlAdapterType.SqlServer2019,
            Formatting = SqlBuilderFormatting.Indented
        };

        public static FluentSqlBuilder<OrderDataModel> SelectSimple01()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions).From<OrderDataModel>();

            return sqlBuilder;
        }

        public static FluentSqlBuilder<CustomerDataModel> SelectWithInnerJoin02()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<CustomerDataModel> SelectWithLeftJoin03()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .LeftJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<CustomerDataModel> SelectWithRightJoin04()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .RightJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithWhere05()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<CustomerDataModel> SelectWithWhereAndInnerJoin06_01()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                 .Where<CustomerDataModel>(x => x.Type == CustomerType.B2B)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<CustomerDataModel> SelectWithWhereAndInnerJoin07_02()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id)
                                 .Where(x => x.Type == CustomerType.B2B);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithOrderBy08()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .OrderBy(x => x.CustomerId);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithOrderByDescendingBy09()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .OrderByDescending(x => x.CustomerId);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithGroupBy10()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Min(x => x.CustomerId)
                                 .GroupBy(x => x.CustomerId);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithGroupByAndHaving11()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Min(x => x.CustomerId)
                                 .GroupBy(x => x.CustomerId)
                                 .Having(SelectFunction.Min, x => x.CustomerId > 1);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithProjection12()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Projection(x => new { x.Id, x.Status });

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithLimit13()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Limit(10);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithPagination14()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .OrderBy(x => x.Id)
                                 .Pagination(10, 1);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithAlias15()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>("order_alias");

            return sqlBuilder;
        }

        public static FluentSqlBuilder<CustomerDataModel> SelectWithFull16()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                   .From<OrderDataModel>("order_alias")
                                   .Projection((orderDataModel) => new { orderDataModel.CustomerId }, "order_alias")
                                   .Projection<CustomerDataModel>((customer) => customer, "customer_alias")
                                   .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                   .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, "customer_alias")
                                   .OrderBy(x => x.Id)
                                   .Limit(10);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithDistinct17()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Distinct(x => x.CustomerId);

            return sqlBuilder;
        }

        public static FluentSqlBuilder<OrderDataModel> SelectWithWhereDate18()
        {
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.DateTimeSave == DateTime.Now);

            return sqlBuilder;
        }
    }
}