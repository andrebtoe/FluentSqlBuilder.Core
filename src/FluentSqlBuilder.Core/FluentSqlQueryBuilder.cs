using FluentSqlBuilder.Core.Inputs;
using SqlBuilderFluent.Core.Extensions.Common;
using SqlBuilderFluent.Exceptions;
using SqlBuilderFluent.Implementations.Interfaces;
using SqlBuilderFluent.Inputs;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Parameters.Interfaces;
using SqlBuilderFluent.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlBuilderFluent
{
    public class FluentSqlQueryBuilder
    {
        private readonly ISqlBuilderFluentAdapter _sqlAdapter;
        private readonly SqlBuilderFormatting _formatting;
        private readonly IDictionary<string, object> _parameters = new Dictionary<string, object>();
        private SqlQueryBuilderExtension _sqlQueryBuilderExtension;
        private string _tableNameRealOverride;
        private int? _limit = null;
        private int? _pageSize = null;
        private int? _pageNumber = null;

        public FluentSqlQueryBuilder(ISqlBuilderFluentAdapter sqlAdapter, Type typeTable, SqlBuilderFormatting formatting, IParameterNameStrategy parameterNameStrategy = null, string tableNameAliasOverride = null)
        {
            _sqlAdapter = sqlAdapter;
            _formatting = formatting;

            DefineTableNameAliasOverride(tableNameAliasOverride);
            DefineSqlQueryBuilderExtension(typeTable);

            _sqlQueryBuilderExtension.DefineTableNameFrom(typeTable);
            _sqlQueryBuilderExtension.DefineSchemaNameFrom(typeTable);
            _sqlQueryBuilderExtension.DefineParameterNameStrategy(parameterNameStrategy);
        }

        public void AddOperatorAnd(TargetClauseType targetClauseType)
        {
            _sqlQueryBuilderExtension.AddOperator(ClauseInputOperator.And, targetClauseType);
        }

        public void AddOperatorOr(TargetClauseType targetClauseType)
        {
            _sqlQueryBuilderExtension.AddOperator(ClauseInputOperator.Or, targetClauseType);
        }

        public void AddOperatorNot(TargetClauseType targetClauseType)
        {
            _sqlQueryBuilderExtension.AddOperator(ClauseInputOperator.Not, targetClauseType);
        }

        public void AddClauseByOperation(string tableName, string columnName, string operation, object columnValue, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            var tableNameToUse = _sqlQueryBuilderExtension.GetTableNameUseConsideringAlias(tableName, tableAlias);
            var parameterNameByColumnName = _sqlQueryBuilderExtension.GetParameterNameByColumnName(columnName);
            var columnNameWithConvention = _sqlAdapter.GetColumnName(tableNameToUse, columnName);
            var parameterFormated = _sqlAdapter.GetParameterFormated(parameterNameByColumnName);
            var clauseInput = new ClauseInput(columnNameWithConvention, operation, parameterFormated, ClauseInputType.ByOperation);

            _sqlQueryBuilderExtension.AddClauseOperation(clauseInput, targetClauseType, selectFunction);
            _sqlQueryBuilderExtension.AddParameter(parameterNameByColumnName, columnValue);
        }

        public void AddClauseByOperationComparison(string leftTableName, string leftColumnName, string operation, string rightTableName, string rightColumnName, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            var columnNameLeft = _sqlAdapter.GetColumnName(leftTableName, leftColumnName);
            var columnNameRight = _sqlAdapter.GetColumnName(rightTableName, rightColumnName);
            var clauseInput = new ClauseInput(columnNameLeft, operation, columnNameRight, ClauseInputType.ByOperationComparison);

            _sqlQueryBuilderExtension.AddClauseOperation(clauseInput, targetClauseType, selectFunction);
        }

        public void AddClauseLike(string tableName, string columnName, string columnValue, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            var parameterNameByColumnName = _sqlQueryBuilderExtension.GetParameterNameByColumnName(columnName);
            var tableNameToUse = _sqlQueryBuilderExtension.GetTableNameUseConsideringAlias(tableName, tableAlias);
            var columnNameWithConvention = _sqlAdapter.GetColumnName(tableNameToUse, columnName);
            var parameterFormated = _sqlAdapter.GetParameterFormated(parameterNameByColumnName);
            var clauseInput = new ClauseInput(columnNameWithConvention, parameterFormated, ClauseInputType.ByLike);

            _sqlQueryBuilderExtension.AddClauseOperation(clauseInput, targetClauseType, selectFunction);
            _sqlQueryBuilderExtension.AddParameter(parameterNameByColumnName, columnValue);
        }

        public void AddClauseIsNull(string tableName, string columnName, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            var tableNameToUse = _sqlQueryBuilderExtension.GetTableNameUseConsideringAlias(tableName, tableAlias);
            var columnNameWithConvention = _sqlAdapter.GetColumnName(tableNameToUse, columnName);
            var clauseInput = new ClauseInput(columnNameWithConvention, ClauseInputType.ByIsNull);

            _sqlQueryBuilderExtension.AddClauseOperation(clauseInput, targetClauseType, selectFunction);
        }

        public void AddClauseIsNotNull(string tableName, string columnName, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            var tableNameToUse = _sqlQueryBuilderExtension.GetTableNameUseConsideringAlias(tableName, tableAlias);
            var columnNameWithConvention = _sqlAdapter.GetColumnName(tableNameToUse, columnName);
            var clauseInput = new ClauseInput(columnNameWithConvention, ClauseInputType.ByIsNotNull);

            _sqlQueryBuilderExtension.AddClauseOperation(clauseInput, targetClauseType, selectFunction);
        }

        public void AddClauseIn(string tableName, string columnName, List<object> values, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            var parameters = values.Select(x =>
            {
                var parameterNameByColumnName = _sqlQueryBuilderExtension.GetParameterNameByColumnName(columnName);
                var parameterFormated = _sqlAdapter.GetParameterFormated(parameterNameByColumnName);

                _sqlQueryBuilderExtension.AddParameter(parameterNameByColumnName, x);

                return parameterFormated;
            });

            var tableNameToUse = _sqlQueryBuilderExtension.GetTableNameUseConsideringAlias(tableName, tableAlias);
            var columnNameWithConvention = _sqlAdapter.GetColumnName(tableNameToUse, columnName);
            var parametersToAdd = String.Join(",", parameters);
            var clauseInput = new ClauseInput(columnNameWithConvention, parametersToAdd, ClauseInputType.ByIn);

            _sqlQueryBuilderExtension.AddClauseOperation(clauseInput, targetClauseType, selectFunction);
        }

        public void AddClauseNotIn(string tableName, string columnName, List<object> values, string tableAlias, TargetClauseType targetClauseType, SelectFunction? selectFunction)
        {
            var parameters = values.Select(x =>
            {
                var parameterNameByColumnName = _sqlQueryBuilderExtension.GetParameterNameByColumnName(columnName);
                var parameterFormated = _sqlAdapter.GetParameterFormated(parameterNameByColumnName);

                _sqlQueryBuilderExtension.AddParameter(parameterNameByColumnName, x);

                return parameterFormated;
            });

            var tableNameToUse = _sqlQueryBuilderExtension.GetTableNameUseConsideringAlias(tableName, tableAlias);
            var columnNameWithConvention = _sqlAdapter.GetColumnName(tableNameToUse, columnName);
            var parametersToAdd = String.Join(",", parameters);
            var clauseInput = new ClauseInput(columnNameWithConvention, parametersToAdd, ClauseInputType.ByNotIn);

            _sqlQueryBuilderExtension.AddClauseOperation(clauseInput, targetClauseType, selectFunction);
        }

        public void AddGroupBy(string tableName, string fieldName)
        {
            var columnName = _sqlAdapter.GetColumnName(tableName, fieldName);

            _sqlQueryBuilderExtension.AddGroupingColumn(columnName);
        }

        public void Select(string tableName, Type tableToProjection = null)
        {
            var columnsToAppendSelect = _sqlQueryBuilderExtension.GetColumnsToAppendSelectFromTableType(tableName, tableToProjection);

            _sqlQueryBuilderExtension.AddProjectionColumns(columnsToAppendSelect);
        }

        public void Select(string tableName, string fieldName, string columnNameAlias)
        {
            var columnName = _sqlAdapter.GetColumnName(tableName, fieldName);
            var hasColumnNameAlias = !String.IsNullOrEmpty(columnNameAlias);

            if (hasColumnNameAlias)
                columnName += $" AS {columnNameAlias}";

            _sqlQueryBuilderExtension.AddProjectionColumns(columnName);
        }

        public void AddJoinByType(string tableSchemaName, string originalTableName, string joinTableName, string leftField, string rightField, JoinType joinType, string tableAlias)
        {
            var existsTableNameRealOverride = !String.IsNullOrEmpty(_tableNameRealOverride);
            var tableNameSource = existsTableNameRealOverride ? _tableNameRealOverride : originalTableName;
            var tableSchemaNameToUse = _sqlAdapter.GetSchemaName(tableSchemaName);
            var tableNameToJoin = _sqlAdapter.GetTableName(joinTableName);
            var columnJoinLeft = _sqlAdapter.GetColumnName(tableNameSource, leftField);
            var existsTableAlias = !String.IsNullOrEmpty(tableAlias);
            var joinTableNameToUse = existsTableAlias ? tableAlias : joinTableName;
            var columnJoinRight = _sqlAdapter.GetColumnName(joinTableNameToUse, rightField);
            var typeJoinNormalized = joinType.ToString().ToUpper();
            var charToUse = _formatting == SqlBuilderFormatting.Indented ? "\n" : "";
            var joinClause = new StringBuilder($"{charToUse}{typeJoinNormalized} JOIN {tableSchemaNameToUse}.{tableNameToJoin} ");

            if (existsTableAlias)
                joinClause.Append($"AS {tableAlias} ");

            joinClause.Append($"ON ({columnJoinLeft} = {columnJoinRight}) ");

            _sqlQueryBuilderExtension.AddJoinExpression(joinClause.ToString());
        }

        public void AddOrderBy(string tableName, IList<string> columnsName, bool descending, string tableAlias)
        {
            var tableNameToUse = _sqlQueryBuilderExtension.GetTableNameUseConsideringAlias(tableName, tableAlias);

            foreach (var columnNameItem in columnsName)
            {
                var columnNameWithConvention = _sqlAdapter.GetColumnName(tableNameToUse, columnNameItem);

                _sqlQueryBuilderExtension.AddOrderBy(columnNameWithConvention, descending);
            }
        }

        public void AddOperatorPrecedenceStart()
        {
            _sqlQueryBuilderExtension.AddOperatorPrecedenceStart();
        }

        public void AddOperatorPrecedenceEnd()
        {
            _sqlQueryBuilderExtension.AddOperatorPrecedenceEnd();
        }

        internal void Select(string tableName, string columnName, SelectFunction selectFunction, string tableAlias, string columnAlias)
        {
            var functionNoHasColumnName = !String.IsNullOrEmpty(columnName);
            var functionName = selectFunction.ToString();
            var existsAlias = !String.IsNullOrEmpty(columnAlias);
            var tableNameToUse = String.IsNullOrEmpty(tableAlias) ? tableName : tableAlias;
            var columnNameToUserFunction = functionNoHasColumnName ? _sqlAdapter.GetColumnName(tableNameToUse, columnName) : "1";
            var functionClause = new StringBuilder($"{functionName.ToUpper()}({columnNameToUserFunction}) ");

            if (existsAlias)
                functionClause.Append($"AS {columnAlias} ");

            _sqlQueryBuilderExtension.AddFunctionsName(functionClause.ToString());
        }

        public IDictionary<string, object> GetParameters()
        {
            var parameters = _parameters;

            return parameters;
        }

        public string GetSelectString()
        {
            var columns = _sqlQueryBuilderExtension.GetColumns();
            var schemaName = _sqlQueryBuilderExtension.GetSchemaName();
            var tableName = _sqlQueryBuilderExtension.GetTableNameFrom();
            var joins = _sqlQueryBuilderExtension.GetJoins();
            var where = _sqlQueryBuilderExtension.GetWhere();
            var order = _sqlQueryBuilderExtension.GetOrder();
            var grouping = _sqlQueryBuilderExtension.GetGrouping();
            var having = _sqlQueryBuilderExtension.GetHaving();

            var sqlSelect = _sqlAdapter.GetSelectString(columns, _limit, schemaName, tableName, _tableNameRealOverride, joins, where, order, grouping, having, _formatting);

            return sqlSelect;
        }

        public string GetSelectPaginationString()
        {
            var columns = _sqlQueryBuilderExtension.GetColumns();
            var schemaName = _sqlQueryBuilderExtension.GetSchemaName();
            var tableName = _sqlQueryBuilderExtension.GetTableNameFrom();
            var joins = _sqlQueryBuilderExtension.GetJoins();
            var where = _sqlQueryBuilderExtension.GetWhere();
            var order = _sqlQueryBuilderExtension.GetOrder();
            var grouping = _sqlQueryBuilderExtension.GetGrouping();
            var having = _sqlQueryBuilderExtension.GetHaving();
            var existsOrder = !String.IsNullOrEmpty(order);
            var pageNumberUse = _pageNumber ?? 1;

            if (!existsOrder)
                throw new SqlBuilderException("To use pagination it is necessary to define 'ORDER BY'");

            if (!_pageSize.HasValue)
                throw new SqlBuilderException("It is necessary to define pagination size");

            var sqlSelect = _sqlAdapter.GetSelectPaginationString(columns, schemaName, tableName, _tableNameRealOverride, joins, where, order, grouping, having, _pageSize.Value, pageNumberUse, _formatting);

            return sqlSelect;
        }

        public void DefineSqlQueryBuilderExtension(Type typeTable)
        {
            _sqlQueryBuilderExtension = new SqlQueryBuilderExtension(_sqlAdapter, _formatting, typeTable, _tableNameRealOverride, _parameters);
        }

        public void DefineTableNameAliasOverride(string tableAlias)
        {
            _tableNameRealOverride = tableAlias;
        }

        public void DefineLimit(int limit)
        {
            _limit = limit;
        }

        public void DefinePageSize(int pageSize)
        {
            _pageSize = pageSize;
        }

        public void DefinePageNumber(int? pageNumber)
        {
            _pageNumber = pageNumber;
        }
    }
}