using FluentSqlBuilder.Data.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectJoinsTest
    {
        private readonly static SqlAdapterType _typeDefault = SqlAdapterType.SqlServer2019;
        private readonly static SqlBuilderFormatting _formattingDefault = SqlBuilderFormatting.Indented;

        [Fact]
        public void Test_Select_InnerJoin_Without_Alias()
        {
            // Arrange
            var tableSchemaName = "checkout";
            var tableNameSource = "order";
            var tableNameTarget = "customer";
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            // Act
            var sqlSelect = sqlBuilder.ToString();

            // Assert
            Assert.True(sqlSelect.Contains($"FROM [{tableSchemaName}].[{tableNameSource}]"), $"FROM invalid");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[Id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[customer_id] AS CustomerId"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[Status]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"INNER JOIN [{tableSchemaName}].[{tableNameTarget}] ON ([{tableNameSource}].[customer_id] = [{tableNameTarget}].[Id])"), $"INNER JOIN invalid");
        }

        [Fact]
        public void Test_Select_InnerJoin_With_Alias()
        {
            // Arrange
            var tableSchemaName = "checkout";
            var tableNameSource = "order";
            var tableNameTarget = "customer";
            var tableNameTargetAlias = "customer_alias";
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, tableNameTargetAlias);

            // Act
            var sqlSelect = sqlBuilder.ToString();

            // Assert
            Assert.True(sqlSelect.Contains($"FROM [{tableSchemaName}].[{tableNameSource}]"), $"FROM invalid");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[Id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[customer_id] AS CustomerId"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[Status]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"INNER JOIN [{tableSchemaName}].[{tableNameTarget}] AS {tableNameTargetAlias} ON ([{tableNameSource}].[customer_id] = [{tableNameTargetAlias}].[Id])"), $"INNER JOIN invalid");
        }

        [Fact]
        public void Test_Select_LeftJoin_Without_Alias()
        {
            // Arrange
            var tableSchemaName = "checkout";
            var tableNameSource = "order";
            var tableNameTarget = "customer";
            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .LeftJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

            // Act
            var sqlSelect = sqlBuilder.ToString();

            // Assert
            Assert.True(sqlSelect.Contains($"FROM [{tableSchemaName}].[{tableNameSource}]"), $"FROM invalid");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[Id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[customer_id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[Status]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"LEFT JOIN [{tableSchemaName}].[{tableNameTarget}] ON ([{tableNameSource}].[customer_id] = [{tableNameTarget}].[Id])"), $"INNER JOIN invalid");
        }

        [Fact]
        public void Test_Select_LeftJoin_With_Alias()
        {
            // Arrange
            var tableSchemaName = "checkout";
            var tableNameSource = "order";
            var tableNameTarget = "customer";
            var tableNameTargetAlias = "customer_alias";

            var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                 .LeftJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, tableNameTargetAlias);

            // Act
            var sqlSelect = sqlBuilder.ToString();

            // Assert
            Assert.True(sqlSelect.Contains($"FROM [{tableSchemaName}].[{tableNameSource}]"), $"FROM invalid");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[Id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[customer_id]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"[{tableNameSource}].[Status]"), $"Column not found");
            Assert.True(sqlSelect.Contains($"LEFT JOIN [{tableSchemaName}].[{tableNameTarget}] AS {tableNameTargetAlias} ON ([{tableNameSource}].[customer_id] = [{tableNameTargetAlias}].[Id])"), $"INNER JOIN invalid");
        }
    }
}