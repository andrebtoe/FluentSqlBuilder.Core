using FluentSqlBuilder.Core.Inputs;
using SqlBuilderFluent.Core.Inputs;
using SqlBuilderFluent.Exceptions;
using SqlBuilderFluent.Helpers;
using SqlBuilderFluent.Implementations.Interfaces;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Parameters;
using SqlBuilderFluent.Parameters.Interfaces;
using SqlBuilderFluent.Types;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SqlBuilderFluent.Core.Extensions.Common
{
    public class SqlQueryBuilderExtension
    {
        private readonly ISqlBuilderFluentAdapter _sqlAdapter;
        private readonly SqlBuilderFormatting _formatting;
        private readonly IList<string> _joinsExpressions = new List<string>();
        private readonly IList<string> _functionsName = new List<string>();
        private readonly IList<string> _whereClause = new List<string>();
        private readonly IList<string> _havingClause = new List<string>();
        private readonly IList<string> _groupingColumns = new List<string>();
        private readonly IList<string> _projectionColumns = new List<string>();
        private readonly IList<OrderByInput> _orderByColumns = new List<OrderByInput>();
        private readonly string _tableNameRealOverride;
        private readonly Type _typeTable;
        private readonly IDictionary<string, object> _parameters = new ExpandoObject();
        private string _tableName;
        private string _schemaName;
        private IParameterNameStrategy _parameterNameStrategy;

        public SqlQueryBuilderExtension(ISqlBuilderFluentAdapter sqlAdapter, SqlBuilderFormatting formatting, Type typeTable, string tableNameRealOverride, IDictionary<string, object> parameters)
        {
            _sqlAdapter = sqlAdapter;
            _formatting = formatting;
            _typeTable = typeTable;
            _tableNameRealOverride = tableNameRealOverride;
            _parameters = parameters;
        }

        public string GetSchemaName()
        {
            var schemaName = _sqlAdapter.GetSchemaName(_schemaName);
            var schemaNameWithConvention = $"{schemaName}";

            return schemaNameWithConvention;
        }

        public string GetTableNameFrom()
        {
            var tableName = _sqlAdapter.GetTableName(_tableName);
            var tableNameWithConvention = $"{tableName}";

            return tableNameWithConvention;
        }

        public string GetTableNameToUseInClause(Type type)
        {
            var existsTableNameAliasOverride = !String.IsNullOrEmpty(_tableNameRealOverride);

            string tableName;

            if (existsTableNameAliasOverride)
                tableName = _tableNameRealOverride;
            else
                tableName = SqlBuilderFluentHelper.GetTableName(type);

            return tableName;
        }

        public string GetColumns()
        {
            var charsSplitColumns = GetCharsSplitColumns();
            var existsProjectionColumns = _projectionColumns.Count > 0;
            var functionsToAdd = _functionsName.Count > 0;
            var columnsToAppendSelect = String.Empty;

            if (existsProjectionColumns)
            {
                columnsToAppendSelect = String.Join(charsSplitColumns, _projectionColumns);
            }
            else if (functionsToAdd)
            {
                columnsToAppendSelect = String.Join(charsSplitColumns, _functionsName);
            }
            else if (!existsProjectionColumns)
            {
                var tableName = GetTableNameToUseInClause(_typeTable);
                columnsToAppendSelect = GetColumnsToAppendSelectFromTableType(tableName);
            }

            return columnsToAppendSelect;
        }

        private string GetCharsSplitColumns()
        {
            var charsSplitColumns = String.Empty;

            switch (_formatting)
            {
                case SqlBuilderFormatting.Indented:
                    charsSplitColumns = ",\n";
                    break;
                case SqlBuilderFormatting.None:
                    charsSplitColumns = ", ";
                    break;
            }

            return charsSplitColumns;
        }

        public string GetColumnsToAppendSelectFromTableType(string tableName, Type? typeTable = null)
        {
            var charsSplitColumns = GetCharsSplitColumns();
            var tableNameWithConvention = _sqlAdapter.GetTableName(tableName);
            var typeTableToUse = typeTable != null ? typeTable : _typeTable;
            var propertiesFromClassTable = typeTableToUse.GetProperties();
            var columnsToAppend = propertiesFromClassTable.Select(c =>
            {
                var columnName = SqlBuilderFluentHelper.GetColumnName(c);
                var columnNameWithConvention = _sqlAdapter.GetColumnName(tableName, columnName);
                var hasColumnAttribute = SqlBuilderFluentHelper.HasColumnAttribute(c);

                if (hasColumnAttribute)
                    columnNameWithConvention += $" AS {c.Name}";

                return columnNameWithConvention;
            });
            var columnsToAppendSelect = String.Join(charsSplitColumns, columnsToAppend);

            return columnsToAppendSelect;
        }

        public string GetWhere()
        {
            if (_whereClause.Count == 0)
                return "";

            var conditionsJoin = String.Join("", _whereClause);
            var whereContent = $"WHERE {conditionsJoin}";

            return whereContent;
        }

        public string GetHaving()
        {
            if (_havingClause.Count == 0)
                return "";

            var conditionsHaving = String.Join("", _havingClause);
            var whereHaving = $"HAVING {conditionsHaving}";

            return whereHaving;
        }

        public string GetOrder()
        {
            var notExistsOrderByColumnsToAdd = _orderByColumns.Count == 0;

            if (notExistsOrderByColumnsToAdd)
                return "";

            var countDistinctTypeOrder = _orderByColumns.Select(x => x.Descending).Distinct().Count();
            var uniqueTypeOrder = countDistinctTypeOrder == 1;
            var columnsName = _orderByColumns.Select(x =>
            {
                var columnName = x.ColumnName;

                if (!uniqueTypeOrder)
                {
                    var descriptionOrder = x.Descending ? "DESC" : "ASC";

                    columnName += $" {descriptionOrder}";
                }

                return columnName;
            });

            var columnsOrderBy = String.Join(", ", columnsName);
            var expressionOrderBy = $"ORDER BY {columnsOrderBy} ";

            if (uniqueTypeOrder)
            {
                var sampleOrderBy = _orderByColumns.First();
                var descriptionOrder = sampleOrderBy.Descending ? "DESC" : "ASC";

                expressionOrderBy += $"{descriptionOrder} ";
            }

            return expressionOrderBy;
        }

        public string GetGrouping()
        {
            var notExistsGroupingColumnsToAdd = _groupingColumns.Count == 0;

            if (notExistsGroupingColumnsToAdd)
                return "";

            var columnsGrouping = String.Join(", ", _groupingColumns);
            var expressionGrouping = $"GROUP BY {columnsGrouping}";

            return expressionGrouping;
        }

        public void AddOperator(ClauseInputOperator clauseInputOperator, TargetClauseType targetClauseType)
        {
            switch (targetClauseType)
            {
                case TargetClauseType.Where:
                    AddWhereOperator(clauseInputOperator);
                    break;
                case TargetClauseType.Having:
                    AddHavingOperator(clauseInputOperator);
                    break;
                default:
                    throw new SqlBuilderException("'targetClauseType' invalid");
            }
        }

        public string GetParameterNameByColumnName(string columnName)
        {
            var parameterName = _parameterNameStrategy.GetName(columnName);

            return parameterName;
        }

        public void AddParameter(string key, object value)
        {
            var parameterAdded = _parameters.ContainsKey(key);

            if (!parameterAdded)
                _parameters.Add(key, value);
        }

        public void DefineTableNameFrom(Type typeTable)
        {
            _tableName = SqlBuilderFluentHelper.GetTableName(typeTable);
        }

        public void DefineSchemaNameFrom(Type typeTable)
        {
            _schemaName = SqlBuilderFluentHelper.GetSchema(typeTable);
        }

        public void DefineParameterNameStrategy(IParameterNameStrategy parameterNameStrategy)
        {
            if (parameterNameStrategy == null)
                _parameterNameStrategy = new ByIndexParameterNameStrategy();
            else
                _parameterNameStrategy = parameterNameStrategy;
        }

        public string GetJoins()
        {
            var addJoinExpression = _joinsExpressions.Count > 0;

            if (!addJoinExpression)
                return "";

            var joinsExpression = String.Join(" ", _joinsExpressions);

            return joinsExpression;
        }

        public void AddClauseOperation(ClauseInput clauseInput, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            switch (targetClauseType)
            {
                case TargetClauseType.Where:
                    AddWhereClause(clauseInput);
                    break;
                case TargetClauseType.Having:
                    AddHavingClause(clauseInput, selectFunction.GetValueOrDefault());
                    break;
                default:
                    throw new SqlBuilderException("'targetClauseType' invalid");
            }
        }

        public void AddGroupingColumn(string columnName)
        {
            _groupingColumns.Add(columnName);
        }

        public void AddProjectionColumns(string columnsToAppendSelect)
        {
            _projectionColumns.Add(columnsToAppendSelect);
        }

        public void AddJoinExpression(string joinExpression)
        {
            _joinsExpressions.Add(joinExpression);
        }

        public void AddOrderBy(string columnName, bool descending)
        {
            var orderByInput = new OrderByInput(columnName, descending);

            _orderByColumns.Add(orderByInput);
        }

        public void AddFunctionsName(string name)
        {
            _functionsName.Add(name);
        }

        public string GetTableNameUseConsideringAlias(string tableName, string tableAlias)
        {
            var existsTableNameAliasOverrideToUse = !String.IsNullOrEmpty(_tableNameRealOverride);
            var existsTableAlias = !String.IsNullOrEmpty(tableAlias);
            var tableNameToUse = existsTableNameAliasOverrideToUse ? _tableNameRealOverride : tableName;

            if (existsTableAlias)
                tableNameToUse = tableAlias;

            return tableNameToUse;
        }

        public void AddOperatorPrecedenceStart()
        {
            _whereClause.Add("(");
        }

        public void AddOperatorPrecedenceEnd()
        {
            _whereClause.Add(")");
        }

        private void AddHavingClause(ClauseInput clauseInput, SelectFunction selectFunction)
        {
            var havingEpression = $"{selectFunction.ToString().ToUpper()}({clauseInput.Column}) {clauseInput.Operation} {clauseInput.ParameterFormated}";

            _havingClause.Add(havingEpression);
        }

        private void AddWhereClause(ClauseInput clauseInput)
        {
            _whereClause.Add(clauseInput.ToString());
        }

        private void AddWhereClause(ClauseOperatorInput clauseOperatorInput)
        {
            _whereClause.Add(clauseOperatorInput.ToString());
        }

        private void AddHavingClause(ClauseOperatorInput clauseOperatorInput)
        {
            _havingClause.Add(clauseOperatorInput.ToString());
        }

        private void AddWhereOperator(ClauseInputOperator clauseInputOperator)
        {
            var existsOperationsOrClauses = _whereClause.Count > 0;

            if (!existsOperationsOrClauses)
                return;

            var clauseOperatorInput = new ClauseOperatorInput(clauseInputOperator, _formatting);

            AddWhereClause(clauseOperatorInput);
        }

        private void AddHavingOperator(ClauseInputOperator clauseInputOperator)
        {
            var existsOperationsOrClauses = _whereClause.Count > 0;

            if (!existsOperationsOrClauses)
                return;

            var clauseOperatorInput = new ClauseOperatorInput(clauseInputOperator, _formatting);

            AddHavingClause(clauseOperatorInput);
        }
    }
}