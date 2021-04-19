using SqlBuilderFluent.Types;
using System;
using System.Text;

namespace FluentSqlBuilder.Core.Implementations.MySql.Inputs
{
    public class MySqlSelectBuildInput
    {
        private readonly string _columns;
        private readonly int? _limit;
        private readonly string _schemaTable;
        private readonly string _tableName;
        private readonly string _tableNameAlias;
        private readonly string _joins;
        private readonly string _where;
        private readonly string _order;
        private readonly string _grouping;
        private readonly string _having;
        private readonly SqlBuilderFormatting _formatting;
        private string _appendBottom;

        public MySqlSelectBuildInput(string columns, int? limit, string schemaTable, string tableName, string tableNameAlias, string joins, string where, string order, string grouping, string having, SqlBuilderFormatting formatting)
        {
            _columns = columns;
            _limit = limit;
            _schemaTable = schemaTable;
            _tableName = tableName;
            _tableNameAlias = tableNameAlias;
            _joins = joins;
            _where = where;
            _order = order;
            _grouping = grouping;
            _having = having;
            _formatting = formatting;
        }

        public void AddSqlBottom(string content)
        {
            _appendBottom += content;
        }

        public override string ToString()
        {
            string selectSql = null;

            switch (_formatting)
            {
                case SqlBuilderFormatting.Indented:
                    selectSql = GetSelectFormattingIndented();
                    break;
                case SqlBuilderFormatting.None:
                    selectSql = GetSelectFormattingNone();
                    break;
            }

            return selectSql;
        }

        private string GetSchema()
        {
            var schemaNameDefault = "";
            var notExistsSchemaName = String.IsNullOrEmpty(_schemaTable);
            var schemaName = notExistsSchemaName ? schemaNameDefault : _schemaTable;

            return schemaName;
        }

        private bool HasAliasToAdd()
        {
            var addTableNameAlias = !String.IsNullOrEmpty(_tableNameAlias);

            return addTableNameAlias;
        }

        private bool HasJoinsToAdd()
        {
            var addJoins = !String.IsNullOrEmpty(_joins);

            return addJoins;
        }

        private string GetSelectFormattingNone()
        {
            var addJoins = HasJoinsToAdd();
            var hasAliasToAdd = HasAliasToAdd();
            var schemaName = GetSchema();
            var sqlSelect = new StringBuilder("SELECT ");

            sqlSelect.Append($"{_columns} FROM {schemaName}.{_tableName} ");

            if (hasAliasToAdd)
                sqlSelect.Append($"AS {_tableNameAlias} ");

            if (addJoins)
                sqlSelect.Append($"{_joins} ");

            sqlSelect.Append($"{_where} {_order} {_grouping} {_having} ");

            if (_limit.HasValue)
                sqlSelect.Append($"LIMIT {_limit} ");

            sqlSelect.Append($"{_appendBottom} ");

            return sqlSelect.ToString();
        }

        private string GetSelectFormattingIndented()
        {
            var addJoins = HasJoinsToAdd();
            var hasAliasToAdd = HasAliasToAdd();
            var schemaName = GetSchema();
            var sqlSelect = new StringBuilder("SELECT ");

            sqlSelect.Append($"{_columns}\nFROM {schemaName}.{_tableName} ");

            if (hasAliasToAdd)
                sqlSelect.Append($"AS {_tableNameAlias} ");

            sqlSelect.AppendLine();

            if (addJoins)
                sqlSelect.AppendLine($"{_joins} ");

            if (!String.IsNullOrEmpty(_where))
                sqlSelect.AppendLine($"{_where} ");

            if (!String.IsNullOrEmpty(_order))
                sqlSelect.AppendLine($"{_order} ");

            if (!String.IsNullOrEmpty(_grouping))
                sqlSelect.AppendLine($"{_grouping} ");

            if (!String.IsNullOrEmpty(_having))
                sqlSelect.AppendLine($"{_having} ");

            if (_limit.HasValue)
                sqlSelect.Append($"LIMIT {_limit} ");

            if (!String.IsNullOrEmpty(_appendBottom))
                sqlSelect.AppendLine($"{_appendBottom} ");

            return sqlSelect.ToString();
        }
    }
}