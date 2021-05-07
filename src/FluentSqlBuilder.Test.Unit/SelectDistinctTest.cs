using FluentSqlBuilder.Core.Middlewares.Inputs;
using FluentSqlBuilder.Core.Middlewares.Services;
using FluentSqlBuilder.Data.DataModel;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectDistinctTest
    {
        private readonly static FluentSqlBuilderMiddlewareOptions _fluentSqlBuilderMiddlewareOptions = new FluentSqlBuilderMiddlewareOptions()
        {
            SqlAdapterType = SqlAdapterType.SqlServer2019,
            Formatting = SqlBuilderFormatting.Indented
        };

        [Fact]
        public void Test_Select_Distinct()
        {
            // Arrange
            var tableSchemaName = "checkout";
            var tableNameAlias = "order_alias";
            var columnNameAlias = "dist_alias";
            var tableNameSource = "order";
            var sqlBuilderWithoutAlias = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                             .From<OrderDataModel>()
                                             .Distinct(x => x.CustomerId);

            var sqlBuilderWithAlias = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                          .From<OrderDataModel>(tableNameAlias)
                                          .Distinct(x => x.CustomerId, tableNameAlias, columnNameAlias);

            // Act
            var sqlSelectWithoutAlias = sqlBuilderWithoutAlias.ToString();
            var sqlSelectWithAlias = sqlBuilderWithAlias.ToString();

            // Assert
            Assert.True(sqlSelectWithoutAlias.Contains($"SELECT DISTINCT([order].[customer_id])"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"FROM [{tableSchemaName}].[{tableNameSource}]"), $"FROM invalid");

            Assert.True(sqlSelectWithAlias.Contains($"SELECT DISTINCT([{tableNameAlias}].[customer_id]) AS {columnNameAlias}"), $"Column not found");
            Assert.True(sqlSelectWithAlias.Contains($"FROM [{tableSchemaName}].[{tableNameSource}] AS {tableNameAlias}"), $"FROM invalid");
        }
    }
}