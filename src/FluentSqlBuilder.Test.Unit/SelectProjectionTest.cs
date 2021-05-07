using FluentSqlBuilder.Core.Middlewares.Inputs;
using FluentSqlBuilder.Core.Middlewares.Services;
using FluentSqlBuilder.Data.DataModel;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectProjectionTest
    {
        private readonly static FluentSqlBuilderMiddlewareOptions _fluentSqlBuilderMiddlewareOptions = new FluentSqlBuilderMiddlewareOptions()
        {
            SqlAdapterType = SqlAdapterType.SqlServer2019,
            Formatting = SqlBuilderFormatting.Indented
        };

        [Fact]
        public void Test_Select_Projection()
        {
            // Arrange
            var tableName = "order";
            var tableNameAlias = "order_alias";
            var sqlBuilderWithoutAlias = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                             .From<OrderDataModel>()
                                             .Projection(x => new { x.Id, x.CustomerId });

            var sqlBuilderAlias = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                      .From<OrderDataModel>(tableNameAlias)
                                      .Projection(x => new { x.Id, x.CustomerId }, tableNameAlias);

            // Act
            var sqlSelectWithoutAlias = sqlBuilderWithoutAlias.ToString();
            var sqlSelectAlias = sqlBuilderAlias.ToString();

            // Assert
            Assert.True(sqlSelectWithoutAlias.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"FROM [checkout].[{tableName}]"), $"FROM invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[Id]"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[customer_id] AS CustomerId"), $"Column not found");

            Assert.True(sqlSelectAlias.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelectAlias.Contains($"FROM [checkout].[{tableName}] AS {tableNameAlias}"), $"FROM invalid");
            Assert.True(sqlSelectAlias.Contains($"[{tableNameAlias}].[Id]"), $"Column not found");
            Assert.True(sqlSelectAlias.Contains($"[{tableNameAlias}].[customer_id] AS CustomerId"), $"Column not found");
        }
    }
}