using SqlBuilderFluent.Types;

namespace SqlBuilderFluent.Implementations.Interfaces
{
    public interface ISqlBuilderFluentAdapter
    {
        string GetSelectString(string columns, int? limit, string schemaTable, string tableName, string tableNameAlias, string joins, string where, string order, string grouping, string having, SqlBuilderFormatting formatting);
        string GetSelectPaginationString(string columns, string schemaName, string tableName, string tableNameAlias, string joins, string where, string order, string grouping, string having, int pageSize, int pageNumber, SqlBuilderFormatting formatting);
        string GetSchemaName(string schemaName);
        string GetTableName(string tableName);
        string GetColumnName(string tableName, string columnName);
        string GetParameterFormated(string parameterId);
    }
}