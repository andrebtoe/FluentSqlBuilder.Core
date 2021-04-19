using SqlBuilderFluent.Core.Implementations.SqlServer.Inputs;
using SqlBuilderFluent.Implementations.Interfaces;
using SqlBuilderFluent.Types;
using System;

namespace SqlBuilderFluent.Implementations.SqlServer
{
    public abstract class SqlServerAdapterBase : ISqlBuilderFluentAdapter
    {
        public string GetSelectString(string columns, int? limit, string schemaTable, string tableName, string tableNameAlias, string joins, string where, string order, string grouping, string having, SqlBuilderFormatting formatting)
        {
            var sqlServerSelectBuildInput = new SqlServerSelectBuildInput(columns, limit, schemaTable, tableName, tableNameAlias, joins, where, order, grouping, having, formatting);

            return sqlServerSelectBuildInput.ToString();
        }

        public string GetSchemaName(string schemaName)
        {
            var schemaNameConvention = $"[{schemaName}]";

            return schemaNameConvention;
        }

        public string GetTableName(string tableName)
        {
            var tableNameWithConvention = $"[{tableName}]";

            return tableNameWithConvention;
        }

        public string GetColumnName(string tableName, string columnName)
        {
            var columnNameWithConvention = $"[{tableName}].[{columnName}]";

            return columnNameWithConvention;
        }

        public string GetParameterFormated(string parameterName)
        {
            var parameterWithNameConvention = $"@{parameterName}";

            return parameterWithNameConvention;
        }

        public virtual string GetSelectPaginationString(string columns, string schemaName, string tableName, string tableNameAlias, string joins, string where, string order, string grouping, string having, int pageSize, int pageNumber, SqlBuilderFormatting formatting)
        {
            throw new NotImplementedException();
        }
    }
}