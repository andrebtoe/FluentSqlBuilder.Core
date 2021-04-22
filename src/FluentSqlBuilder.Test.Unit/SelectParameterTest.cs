using FluentSqlBuilder.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectParameterTest
    {
        private static SqlAdapterType _typeDefault = SqlAdapterType.SqlServer2019;
        private static SqlBuilderFormatting _formattingDefault = SqlBuilderFormatting.Indented;

        [Fact]
        public void Test_Select_Parameters_Case01()
        {
            // Arrange
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
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
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
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
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
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