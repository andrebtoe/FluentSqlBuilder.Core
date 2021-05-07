using FluentSqlBuilder.Core.Middlewares.Inputs;
using FluentSqlBuilder.Core.Middlewares.Services;
using FluentSqlBuilder.Data.DataModel;
using SqlBuilderFluent.Exceptions;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectPaginationTest
    {
        private readonly static FluentSqlBuilderMiddlewareOptions _fluentSqlBuilderMiddlewareOptions = new FluentSqlBuilderMiddlewareOptions()
        {
            SqlAdapterType = SqlAdapterType.SqlServer2019,
            Formatting = SqlBuilderFormatting.Indented
        };

        [Fact]
        public void Test_Select_Pagination_Sucess()
        {
            // Arrange
            var tableName = "order";
            var tableNameAlias = "order_alias";
            var sqlBuilderWithoutAlias = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                             .From<OrderDataModel>()
                                             .OrderBy(x => x.CustomerId)
                                             .Pagination(10, 1);

            var sqlBuilderAlias = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                      .From<OrderDataModel>(tableNameAlias)
                                      .OrderBy(tableNameAlias, x => x.CustomerId)
                                      .Pagination(10, 1);

            // Act
            var sqlSelectWithoutAlias = sqlBuilderWithoutAlias.ToString();
            var sqlSelectWithAlias = sqlBuilderAlias.ToString();

            // Assert
            Assert.True(sqlSelectWithoutAlias.Contains($"SELECT"), $"FROM invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[Id]"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[customer_id] AS CustomerId"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[Status]"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"FROM [checkout].[{tableName}]"), $"FROM invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"ORDER BY [{tableName}].[customer_id] ASC"), $"ORDER BY invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY"), $"ORDER BY invalid");

            Assert.True(sqlSelectWithAlias.Contains($"SELECT"), $"FROM invalid");
            Assert.True(sqlSelectWithAlias.Contains($"[{tableNameAlias}].[Id]"), $"Column not found");
            Assert.True(sqlSelectWithAlias.Contains($"[{tableNameAlias}].[customer_id] AS CustomerId"), $"Column not found");
            Assert.True(sqlSelectWithAlias.Contains($"[{tableNameAlias}].[Status]"), $"Column not found");
            Assert.True(sqlSelectWithAlias.Contains($"FROM [checkout].[{tableName}] AS {tableNameAlias}"), $"FROM invalid");
            Assert.True(sqlSelectWithAlias.Contains($"ORDER BY [{tableNameAlias}].[customer_id] ASC"), $"ORDER BY invalid");
            Assert.True(sqlSelectWithAlias.Contains($"OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY"), $"ORDER BY invalid");
        }

        [Fact]
        public void Test_Select_Pagination_Fail_Without_OrderBy()
        {
            // Arrange
            var sqlBuilderWithoutAlias = new FluentSqlBuilderService(_fluentSqlBuilderMiddlewareOptions)
                                             .From<OrderDataModel>()
                                             .Pagination(10, 1);

            // Assert
            Assert.Throws<SqlBuilderException>(() => sqlBuilderWithoutAlias.ToString());
        }
    }
}