using FluentSqlBuilder.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectOrderByTest
    {
        private static SqlAdapterType _typeDefault = SqlAdapterType.SqlServer2019;
        private static SqlBuilderFormatting _formattingDefault = SqlBuilderFormatting.Indented;

        [Fact]
        public void Test_Select_OrderBy_Asc_Without_Alias()
        {
            Test_Select_OrderBy(false);
        }

        [Fact]
        public void Test_Select_OrderBy_Desc_Without_Alias()
        {
            Test_Select_OrderBy(true);
        }

        public void Test_Select_OrderBy(bool desc)
        {
            // Arrange
            var tableName = "order";
            var tableNameAlias = "order_alias";
            var orderByDescription = desc ? "DESC" : "ASC";
            var sqlBuilderWithoutAlias = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault);
            var sqlBuilderAlias = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault, tableNameAlias);

            AddOrderBy(sqlBuilderWithoutAlias, desc, null);
            AddOrderBy(sqlBuilderAlias, desc, tableNameAlias);

            // Act
            var sqlSelectWithoutAlias = sqlBuilderWithoutAlias.ToString();
            var sqlSelectAlias = sqlBuilderAlias.ToString();

            // Assert
            Assert.True(sqlSelectWithoutAlias.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"FROM [checkout].[{tableName}]"), $"FROM invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[Id]"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[customer_id] AS CustomerId"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[Status]"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"ORDER BY [{tableName}].[customer_id] {orderByDescription}"), $"Column not found");

            Assert.True(sqlSelectAlias.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelectAlias.Contains($"FROM [checkout].[{tableName}] AS {tableNameAlias}"), $"FROM invalid");
            Assert.True(sqlSelectAlias.Contains($"[{tableNameAlias}].[Id]"), $"Column not found");
            Assert.True(sqlSelectAlias.Contains($"[{tableNameAlias}].[customer_id] AS CustomerId"), $"Column not found");
            Assert.True(sqlSelectAlias.Contains($"[{tableNameAlias}].[Status]"), $"Column not found");
            Assert.True(sqlSelectAlias.Contains($"ORDER BY [{tableNameAlias}].[customer_id] {orderByDescription}"), $"Column not found");
        }

        private void AddOrderBy(FluentSqlBuilder<OrderDataModel> fluentSqlBuilder, bool desc, string tableNameAlias)
        {
            switch (desc)
            {
                case true:
                    fluentSqlBuilder.OrderByDescending(tableNameAlias, x => x.CustomerId);
                    break;
                case false:
                    fluentSqlBuilder.OrderBy(tableNameAlias, x => x.CustomerId);
                    break;
            }
        }
    }
}