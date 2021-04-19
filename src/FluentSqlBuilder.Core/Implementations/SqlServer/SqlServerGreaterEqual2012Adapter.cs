using SqlBuilderFluent.Core.Implementations.SqlServer.Inputs;
using SqlBuilderFluent.Types;

namespace SqlBuilderFluent.Implementations.SqlServer
{
    public class SqlServerGreaterEqual2012Adapter : SqlServerAdapterBase
    {
        public override string GetSelectPaginationString(string columns, string schemaName, string tableName, string tableNameAlias, string joins, string where, string order, string grouping, string having, int pageSize, int pageNumber, SqlBuilderFormatting formatting)
        {
            var rowNumber = pageSize * (pageNumber - 1);
            var sqlServerSelectBuildInput = new SqlServerSelectBuildInput(columns, null, schemaName, tableName, tableNameAlias, joins, where, order, grouping, having, formatting);
            var offsetToAdd = $"OFFSET {rowNumber} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            sqlServerSelectBuildInput.AddSqlBottom(offsetToAdd);

            return sqlServerSelectBuildInput.ToString();
        }
    }
}