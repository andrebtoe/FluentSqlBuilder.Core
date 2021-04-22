using FluentSqlBuilder.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectWhereSimpleTest
    {
        private static SqlAdapterType _typeDefault = SqlAdapterType.SqlServer2019;
        private static SqlBuilderFormatting _formattingDefault = SqlBuilderFormatting.Indented;

        [Fact]
        public void Test_Select__Simple_Without_Alias()
        {
            // Arrange
            var tableName = "order";
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1);

            // Act
            var sqlSelect = sqlBuilder.ToString();

            // Assert
            Assert.True(sqlSelect.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelect.Contains($"FROM [checkout].[{tableName}]"), $"FROM invalid");
            Assert.True(sqlSelect.Contains($"[{tableName}].[Id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableName}].[customer_id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableName}].[Status]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"WHERE ([order].[Status] = @Param1"), $"WHERE invalid");
            Assert.True(sqlSelect.Contains($"AND [order].[customer_id] = @Param2)"), $"WHERE invalid");
        }

        [Fact]
        public void Test_Select_Simple_With_Alias()
        {
            // Arrange
            var tableName = "order";
            var tableNameAlias = "order_alias";
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault, tableNameAlias)
                                 .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1, tableNameAlias);

            // Act
            var sqlSelect = sqlBuilder.ToString();

            // Assert
            Assert.True(sqlSelect.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelect.Contains($"FROM [checkout].[{tableName}] AS {tableNameAlias}"), $"FROM invalid");
            Assert.True(sqlSelect.Contains($"[{tableNameAlias}].[Id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameAlias}].[customer_id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameAlias}].[Status]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"WHERE ([{tableNameAlias}].[Status] = @Param1"), $"WHERE invalid");
            Assert.True(sqlSelect.Contains($"AND [{tableNameAlias}].[customer_id] = @Param2)"), $"WHERE invalid");
        }
    }
}