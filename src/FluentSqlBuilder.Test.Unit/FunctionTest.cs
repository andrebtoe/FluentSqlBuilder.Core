using FluentSqlBuilder.DataModel;
using SqlBuilderFluent;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Types;
using Xunit;

namespace FluentSqlBuilder.Test.Unit
{
    public class FunctionTest
    {
        [Theory]
        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.Indented, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.None, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.Indented, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.None, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.Indented, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.None, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.Indented, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.None, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.Indented, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.None, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, SelectFunction.Sum)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.None, SelectFunction.Sum)]

        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.Indented, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.None, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.Indented, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.None, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.Indented, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.None, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.Indented, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.None, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.Indented, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.None, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, SelectFunction.Min)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.None, SelectFunction.Min)]

        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.Indented, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.None, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.Indented, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.None, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.Indented, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.None, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.Indented, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.None, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.Indented, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.None, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, SelectFunction.Max)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.None, SelectFunction.Max)]

        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.Indented, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.None, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.Indented, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.None, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.Indented, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.None, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.Indented, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.None, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.Indented, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.None, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, SelectFunction.Avg)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.None, SelectFunction.Avg)]

        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.Indented, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2008, SqlBuilderFormatting.None, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.Indented, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2012, SqlBuilderFormatting.None, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.Indented, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2014, SqlBuilderFormatting.None, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.Indented, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2016, SqlBuilderFormatting.None, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.Indented, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2017, SqlBuilderFormatting.None, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, SelectFunction.Count)]
        [InlineData(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.None, SelectFunction.Count)]
        public void Test_Function_With_GroupBy_And_Having_SqlServer(SqlAdapterType sqlAdapterType, SqlBuilderFormatting formatting, SelectFunction selectFunction)
        {
            TestFunctionSelect(sqlAdapterType, formatting, selectFunction);
        }

        private void TestFunctionSelect(SqlAdapterType sqlAdapterType, SqlBuilderFormatting formatting, SelectFunction selectFunction)
        {
            // Arrange
            var tableNameAlis = "order_alias";
            var columnNameAlis = "Quantity";
            var functionName = selectFunction.ToString().ToUpper();
            var fluentSqlBuilderWithoutAlias = new FluentSqlBuilder<OrderDataModel>(sqlAdapterType, formatting)
                                               .GroupBy(x => x.CustomerId)
                                               .Having(selectFunction, x => x.CustomerId >= 1);

            var fluentSqlBuilderWithAlias = new FluentSqlBuilder<OrderDataModel>(sqlAdapterType, formatting, tableNameAlis)
                                                .GroupBy(tableNameAlis, x => x.CustomerId)
                                                .Having(selectFunction, x => x.CustomerId >= 0, tableNameAlis);


            AddFunctionByRef(fluentSqlBuilderWithoutAlias, selectFunction);
            AddFunctionByRef(fluentSqlBuilderWithAlias, selectFunction, tableNameAlis, columnNameAlis);

            // Act
            var sqlSelectWithoutAlias = fluentSqlBuilderWithoutAlias.ToString();
            var sqlSelectWithAlias = fluentSqlBuilderWithAlias.ToString();

            // Assert
            Assert.True(sqlSelectWithoutAlias.Contains($"{functionName}([order].[customer_id])"), $"'${functionName}' expected clause => {sqlAdapterType}:{formatting}");
            Assert.True(sqlSelectWithoutAlias.Contains("FROM [checkout].[order]"), $"'FROM' expected clause => {sqlAdapterType}:{formatting}");
            Assert.True(sqlSelectWithoutAlias.Contains("GROUP BY [order].[customer_id]"), $"'GROUP BY' expected clause => {sqlAdapterType}:{formatting}");
            Assert.True(sqlSelectWithoutAlias.Contains($"HAVING {functionName}([order].[customer_id]) >= @Param1"), $"'HAVING' expected clause => {sqlAdapterType}:{formatting}");

            Assert.True(sqlSelectWithAlias.Contains($"{functionName}([{tableNameAlis}].[customer_id]) AS {columnNameAlis}"), $"'{functionName}' expected clause => {sqlAdapterType}:{formatting}");
            Assert.True(sqlSelectWithAlias.Contains($"FROM [checkout].[{tableNameAlis}] AS order_alias"), $"'FROM' expected clause => {sqlAdapterType}:{formatting}");
            Assert.True(sqlSelectWithAlias.Contains($"GROUP BY [{tableNameAlis}].[customer_id]"), $"'GROUP BY' expected clause => {sqlAdapterType}:{formatting}");
            Assert.True(sqlSelectWithAlias.Contains($"HAVING {functionName}([{tableNameAlis}].[customer_id]) >= @Param1"), $"'HAVING' expected clause => {sqlAdapterType}:{formatting}");
        }

        private void AddFunctionByRef<TTable>(FluentSqlBuilder<TTable> fluentSqlBuilder, SelectFunction selectFunction, string tableAlias = null, string columnAlias = null)
        {
            switch (selectFunction)
            {
                case SelectFunction.Count:
                    fluentSqlBuilder.Count<OrderDataModel>(x => x.CustomerId, tableAlias, columnAlias);
                    break;
                case SelectFunction.Distinct:
                    fluentSqlBuilder.Distinct<OrderDataModel>(x => x.CustomerId, tableAlias, columnAlias);
                    break;
                case SelectFunction.Sum:
                    fluentSqlBuilder.Sum<OrderDataModel>(x => x.CustomerId, tableAlias, columnAlias);
                    break;
                case SelectFunction.Min:
                    fluentSqlBuilder.Min<OrderDataModel>(x => x.CustomerId, tableAlias, columnAlias);
                    break;
                case SelectFunction.Max:
                    fluentSqlBuilder.Max<OrderDataModel>(x => x.CustomerId, tableAlias, columnAlias);
                    break;
                case SelectFunction.Avg:
                    fluentSqlBuilder.Avg<OrderDataModel>(x => x.CustomerId, tableAlias, columnAlias);
                    break;
            }
        }
    }
}