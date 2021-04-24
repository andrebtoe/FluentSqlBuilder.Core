using FluentSqlBuilder.Core.Implementations.MySql.Inputs;
using SqlBuilderFluent.Implementations.Interfaces;
using SqlBuilderFluent.Types;
using System;

namespace FluentSqlBuilder.Core.Implementations.MySql
{
    public class MySqlAdapter : ISqlBuilderFluentAdapter
    {
        public string GetSelectString(string columns, int? limit, string schemaTable, string tableName, string tableNameAlias, string joins, string where, string order, string grouping, string having, SqlBuilderFormatting formatting)
        {
            var mySqlSelectBuildInput = new MySqlSelectBuildInput(columns, limit, schemaTable, tableName, tableNameAlias, joins, where, order, grouping, having, formatting);

            return mySqlSelectBuildInput.ToString();
        }

        public string GetSchemaName(string schemaName)
        {
            return schemaName;
        }

        public string GetTableName(string tableName)
        {
            return tableName;
        }

        public string GetColumnName(string tableName, string columnName)
        {
            var columnNameWithConvention = $"{tableName}.{columnName}";

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

        public string GetColumnNameAndResolveByFunctionYear(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }
    }
}