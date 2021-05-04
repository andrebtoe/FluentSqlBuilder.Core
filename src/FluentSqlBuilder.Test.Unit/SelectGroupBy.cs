using FluentSqlBuilder.Data.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class SelectGroupBy
    {
        private readonly static SqlAdapterType _typeDefault = SqlAdapterType.SqlServer2019;
        private readonly static SqlBuilderFormatting _formattingDefault = SqlBuilderFormatting.Indented;

        [Fact]
        public void Test_Select_GroupBy_Min()
        {
            Test_Select_Group_And_Function(SelectFunction.Min);
        }

        [Fact]
        public void Test_Select_GroupBy_Max()
        {
            Test_Select_Group_And_Function(SelectFunction.Max);
        }

        [Fact]
        public void Test_Select_GroupBy_Sum()
        {
            Test_Select_Group_And_Function(SelectFunction.Sum);
        }

        [Fact]
        public void Test_Select_GroupBy_Avg()
        {
            Test_Select_Group_And_Function(SelectFunction.Avg);
        }

        [Fact]
        public void Test_Select_GroupBy_Count()
        {
            Test_Select_Group_And_Function(SelectFunction.Count);
        }

        public void Test_Select_Group_And_Function(SelectFunction selectFunction)
        {
            // Arrange
            var tableName = "order";
            var tableNameAlias = "order_alias";
            var functionNameNormalided = selectFunction.ToString().ToUpper();
            var sqlBuilderWithoutAlias = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault)
                                             .GroupBy(x => x.CustomerId);

            var sqlBuilderAlias = new FluentSqlBuilder<OrderDataModel>(_typeDefault, _formattingDefault, tableNameAlias)
                                      .GroupBy(tableNameAlias, x => x.CustomerId);

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

            Assert.True(sqlSelectAlias.Contains($"SELECT"), $"SELECT invalid");
            Assert.True(sqlSelectAlias.Contains($"FROM [checkout].[{tableName}] AS {tableNameAlias}"), $"FROM invalid");
            Assert.True(sqlSelectAlias.Contains($"{functionNameNormalided}([{tableNameAlias}].[customer_id])"), $"Column not found");
            Assert.True(sqlSelectAlias.Contains($"GROUP BY [{tableNameAlias}].[customer_id]"), $"Column not found");
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