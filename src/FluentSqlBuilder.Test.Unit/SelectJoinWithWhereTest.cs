using FluentSqlBuilder.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Inputs;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectJoinWithWhereTest
    {
        private static SqlAdapterType _typeDefault = SqlAdapterType.SqlServer2019;
        private static SqlBuilderFormatting _formattingDefault = SqlBuilderFormatting.Indented;

        [Fact]
        public void Test_Select_InnerJoin()
        {
            Test_Select_Join_ByTypeJoin(JoinType.Inner);
        }

        [Fact]
        public void Test_Select_LeftJoin()
        {
            Test_Select_Join_ByTypeJoin(JoinType.Left);
        }

        [Fact]
        public void Test_Select_RightJoin()
        {
            Test_Select_Join_ByTypeJoin(JoinType.Right);
        }

        private void Test_Select_Join_ByTypeJoin(JoinType joinType)
        {
            // Arrange
            var tableName = "order";
            var tableNameJoin = "customer";
            var tableNameOrderAlias = "order_alias";
            var tableNameCustomerAlias = "customer_alias";
            var joinTypeNormalized = joinType.ToString().ToUpper();
            var sqlBuilderWithoutAlias = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                             .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1)
                                             .Where<CustomerDataModel>(x => x.Type == CustomerType.B2B);
            
            var sqlBuilderWithAlias = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault, tableNameOrderAlias)
                                          .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1, tableNameOrderAlias)
                                          .Where<CustomerDataModel>(x => x.Type == CustomerType.B2B, tableNameCustomerAlias);

            SetTypeJoinInQueryByRef(sqlBuilderWithoutAlias, joinType, null);
            SetTypeJoinInQueryByRef(sqlBuilderWithAlias, joinType, tableNameCustomerAlias);

            // Act
            var sqlSelectWithoutAlias = sqlBuilderWithoutAlias.ToString();
            var sqlSelectWithAlias = sqlBuilderWithAlias.ToString();

            // Assert
            Assert.True(sqlSelectWithoutAlias.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[Id]"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[customer_id]"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"[{tableName}].[Status]"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"FROM [checkout].[{tableName}]"), $"FROM invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"{joinTypeNormalized} JOIN [checkout].[{tableNameJoin}] ON ([{tableName}].[customer_id] = [{tableNameJoin}].[Id])"), $"{joinTypeNormalized} JOIN invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"WHERE ([{tableName}].[Status] = @Param1"), $"WHERE invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"AND [{tableName}].[customer_id] = @Param2)"), $"WHERE invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"AND [{tableNameJoin}].[Type] = @Param3"), $"WHERE invalid");

            Assert.True(sqlSelectWithAlias.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelectWithAlias.Contains($"[{tableNameOrderAlias}].[Id]"), $"Column not found");
            Assert.True(sqlSelectWithAlias.Contains($"[{tableNameOrderAlias}].[customer_id]"), $"Column not found");
            Assert.True(sqlSelectWithAlias.Contains($"[{tableNameOrderAlias}].[Status]"), $"Column not found");
            Assert.True(sqlSelectWithAlias.Contains($"FROM [checkout].[{tableName}] AS {tableNameOrderAlias}"), $"FROM invalid");
            Assert.True(sqlSelectWithAlias.Contains($"{joinTypeNormalized} JOIN [checkout].[{tableNameJoin}] AS {tableNameCustomerAlias} ON ([{tableNameOrderAlias}].[customer_id] = [{tableNameCustomerAlias}].[Id])"), $"{joinTypeNormalized} JOIN invalid");
            Assert.True(sqlSelectWithAlias.Contains($"WHERE ([{tableNameOrderAlias}].[Status] = @Param1"), $"WHERE invalid");
            Assert.True(sqlSelectWithAlias.Contains($"AND [{tableNameOrderAlias}].[customer_id] = @Param2)"), $"WHERE invalid");
            Assert.True(sqlSelectWithAlias.Contains($"AND [{tableNameCustomerAlias}].[Type] = @Param3"), $"WHERE invalid");
        }

        private void SetTypeJoinInQueryByRef(FluentSqlBuilder<OrderDataModel> fluentSqlBuilder, JoinType joinType, string tableNameAlias)
        {
            switch (joinType)
            {
                case JoinType.Left:
                    fluentSqlBuilder.LeftJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, tableNameAlias);
                    break;
                case JoinType.Right:
                    fluentSqlBuilder.RightJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, tableNameAlias);
                    break;
                case JoinType.Inner:
                    fluentSqlBuilder.InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id, tableNameAlias);
                    break;
            }
        }
    }
}