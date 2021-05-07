using FluentSqlBuilder.Core.Middlewares.Inputs;
using FluentSqlBuilder.Core.Middlewares.Services;
using FluentSqlBuilder.Data.DataModel;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectParameterTest
    {
        private readonly static FluentSqlBuilderMiddlewareOptions _fluentSqlBuilderMiddlewareOptions = new FluentSqlBuilderMiddlewareOptions()
        {
            SqlAdapterType = SqlAdapterType.SqlServer2019,
            Formatting = SqlBuilderFormatting.Indented
        };

        [Fact]
        public void Test_Select_Parameters_Case01()
        {
            // Arrange
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.CustomerId == 1 && x.Status == OrderStatus.AwaitingPayment)
                                 .Where(x => x.Id > 0)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id)
                                 .Where(x => x.Type != CustomerType.B2B);

            // Act
            var parameters = sqlBuilder.GetParameters();

            // Assert
            Assert.True(parameters.Keys.Count == 4, $"Parametsr invalid");
        }

        [Fact]
        public void Test_Select_Parameters_Case02()
        {
            // Arrange
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.CustomerId == 1 && x.Status == OrderStatus.AwaitingPayment)
                                 .Where(x => x.Id > 0)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id)
                                 .Where(x => x.Type != CustomerType.B2B)
                                 .And(x => x.Type == CustomerType.B2C);

            // Act
            var parameters = sqlBuilder.GetParameters();

            // Assert
            Assert.True(parameters.Keys.Count == 5, $"Parametsr invalid");
        }

        [Fact]
        public void Test_Select_Parameters_Case03()
        {
            // Arrange
            var sqlBuilder = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                 .From<OrderDataModel>()
                                 .Where(x => x.CustomerId == 1 && x.Status == OrderStatus.AwaitingPayment)
                                 .Where(x => x.Id > 0)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id)
                                 .Where(x => x.Type != CustomerType.B2B)
                                 .And(x => x.Type == CustomerType.B2C)
                                 .Or(x => x.Type == CustomerType.B2C && x.Name != "");

            // Act
            var parameters = sqlBuilder.GetParameters();

            // Assert
            Assert.True(parameters.Keys.Count == 7, $"Parametsr invalid");
        }
    }
}