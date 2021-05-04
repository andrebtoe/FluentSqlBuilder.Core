using FluentSqlBuilder.Data.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectLimitTest
    {
        private static SqlAdapterType _typeDefault = SqlAdapterType.SqlServer2019;
        private static SqlBuilderFormatting _formattingDefault = SqlBuilderFormatting.Indented;

        [Fact]
        public void Test_Select_Limit()
        {
            // Arrange
            var tableName = "order";
            var limit = 10;
            var sqlBuilderWithoutAlias = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                             .Limit(limit);

            // Act
            var sqlSelect = sqlBuilderWithoutAlias.ToString();

            // Assert
            Assert.True(sqlSelect.Contains($"SELECT TOP({limit})"), $"FROM invalid");
            Assert.True(sqlSelect.Contains($"FROM [checkout].[{tableName}]"), $"FROM invalid");
            Assert.True(sqlSelect.Contains($"[{tableName}].[Id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableName}].[customer_id] AS CustomerId"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableName}].[Status]"), $"Column not found");
        }
    }
}