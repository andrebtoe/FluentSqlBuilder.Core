using FluentSqlBuilder.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectGroupByAndHaving
    {
        private static SqlAdapterType _typeDefault = SqlAdapterType.SqlServer2019;
        private static SqlBuilderFormatting _formattingDefault = SqlBuilderFormatting.Indented;

        [Fact]
        public void Test_Select_GroupBy_Min()
        {
            Test_Select_Group_And_Function_With_Having(SelectFunction.Min);
        }

        [Fact]
        public void Test_Select_GroupBy_Max()
        {
            Test_Select_Group_And_Function_With_Having(SelectFunction.Max);
        }

        [Fact]
        public void Test_Select_GroupBy_Sum()
        {
            Test_Select_Group_And_Function_With_Having(SelectFunction.Sum);
        }

        [Fact]
        public void Test_Select_GroupBy_Avg()
        {
            Test_Select_Group_And_Function_With_Having(SelectFunction.Avg);
        }

        [Fact]
        public void Test_Select_GroupBy_Count()
        {
            Test_Select_Group_And_Function_With_Having(SelectFunction.Count);
        }

        public void Test_Select_Group_And_Function_With_Having(SelectFunction selectFunction)
        {
            // Arrange
            var tableName = "order";
            var tableNameAlias = "order_alias";
            var functionNameNormalided = selectFunction.ToString().ToUpper();
            var sqlBuilderWithoutAlias = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                             .GroupBy(x => x.CustomerId)
                                             .Having(selectFunction, x => x.CustomerId > 1);

            var sqlBuilderAlias = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault, tableNameAlias)
                                      .GroupBy(tableNameAlias, x => x.CustomerId)
                                      .Having(selectFunction, x => x.CustomerId > 1, tableNameAlias);

            AddFunction(sqlBuilderWithoutAlias, null, selectFunction);
            AddFunction(sqlBuilderAlias, tableNameAlias, selectFunction);

            // Act
            var sqlSelectWithoutAlias = sqlBuilderWithoutAlias.ToString();
            var sqlSelectAlias = sqlBuilderAlias.ToString();

            // Assert
            Assert.True(sqlSelectWithoutAlias.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"FROM [checkout].[{tableName}]"), $"FROM invalid");
            Assert.True(sqlSelectWithoutAlias.Contains($"{functionNameNormalided}([{tableName}].[customer_id])"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"GROUP BY [{tableName}].[customer_id]"), $"Column not found");
            Assert.True(sqlSelectWithoutAlias.Contains($"HAVING {functionNameNormalided}([{tableName}].[customer_id]) > @Param1"), $"Having not found");

            Assert.True(sqlSelectAlias.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelectAlias.Contains($"FROM [checkout].[{tableName}] AS {tableNameAlias}"), $"FROM invalid");
            Assert.True(sqlSelectAlias.Contains($"{functionNameNormalided}([{tableNameAlias}].[customer_id])"), $"Column not found");
            Assert.True(sqlSelectAlias.Contains($"GROUP BY [{tableNameAlias}].[customer_id]"), $"Column not found");
            Assert.True(sqlSelectAlias.Contains($"HAVING {functionNameNormalided}([{tableNameAlias}].[customer_id]) > @Param1"), $"Having not found");
        }

        private void AddFunction(FluentSqlBuilder<OrderDataModel> fluentSqlBuilder, string tableNameAlias, SelectFunction selectFunction)
        {
            switch (selectFunction)
            {
                case SelectFunction.Count:
                    fluentSqlBuilder.Count(x => x.CustomerId, tableNameAlias);
                    break;
                case SelectFunction.Sum:
                    fluentSqlBuilder.Sum(x => x.CustomerId, tableNameAlias);
                    break;
                case SelectFunction.Min:
                    fluentSqlBuilder.Min(x => x.CustomerId, tableNameAlias);
                    break;
                case SelectFunction.Max:
                    fluentSqlBuilder.Max(x => x.CustomerId, tableNameAlias);
                    break;
                case SelectFunction.Avg:
                    fluentSqlBuilder.Avg(x => x.CustomerId, tableNameAlias);
                    break;
            }
        }
    }
}