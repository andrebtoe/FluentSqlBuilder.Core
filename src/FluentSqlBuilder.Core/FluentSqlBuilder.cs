using FluentSqlBuilder.Core.Implementations.MySql;
using FluentSqlBuilder.Core.Inputs;
using SqlBuilderFluent.Core.Extensions;
using SqlBuilderFluent.Core.Inputs;
using SqlBuilderFluent.Exceptions;
using SqlBuilderFluent.Implementations.Interfaces;
using SqlBuilderFluent.Implementations.SqlServer;
using SqlBuilderFluent.Inputs;
using SqlBuilderFluent.Lambdas;
using SqlBuilderFluent.Lambdas.Inputs;
using SqlBuilderFluent.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlBuilderFluent
{
    /// <summary>
    /// Class to generate sql select with fluent.
    /// </summary>
    /// <typeparam name="TTable">Table type referenced by generic type.</typeparam>
    public class FluentSqlBuilder<TTable> : BaseFluentSqlBuilder
    {
        internal LambdaResolver _resolver;
        private SqlBuilderFluentExtension<TTable> _sqlBuilderFluentExtension;
        private TargetToSelect _targetToSelect;

        public FluentSqlBuilder(SqlAdapterType sqlAdapterType, SqlBuilderFormatting formatting = SqlBuilderFormatting.None, string tableAlias = null)
        {
            DefineSqlAdapter(sqlAdapterType, typeof(TTable), formatting, tableAlias);
            DefineTargetToSelect(TargetToSelect.Common);
            DefineExtension();
        }

        internal FluentSqlBuilder(FluentSqlQueryBuilder sqlQueryBuilder, LambdaResolver resolver, TargetToSelect targetToSelect)
        {
            _sqlQueryBuilder = sqlQueryBuilder;
            _resolver = resolver;

            DefineTargetToSelect(targetToSelect);
        }

        #region Projection

        /// <summary>
        /// By default, queries return all columns. To limit, you can include a projection to specify or restrict columns to return.
        /// </summary>
        /// <param name="columnExp">Indicates projection of columns through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Projection(Expression<Func<TTable, object>> columnExp, string tableAlias = null)
        {
            _resolver.Select(columnExp, tableAlias, typeof(TTable));

            return this;
        }

        /// <summary>
        /// By default, queries return all columns. To limit, you can include a projection to specify or restrict columns to return.
        /// </summary>
        /// <param name="columnExp">Indicates projection of columns through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> Projection<TTableProjection>(Expression<Func<TTableProjection, object>> columnExp, string tableAlias = null)
        {
            _resolver.Select(columnExp, tableAlias, typeof(TTableProjection));

            return this;
        }

        #endregion

        #region Clause

        /// <summary>
        /// Method for abstraction select pagination clauses.
        /// </summary>
        /// <param name="pageSize">Indicates number of rows per page.</param>
        /// <param name="pageNumber">Indicates the page number, within the pagination.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Pagination(int pageSize, int? pageNumber = null)
        {
            DefineTargetToSelect(TargetToSelect.Pagination);

            _resolver.AddPagination(pageSize, pageNumber);

            return this;
        }

        /// <summary>
        /// Method to limit the number of rows.
        /// </summary>
        /// <param name="count">Defines the number of rows that the query should return.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Limit(int count)
        {
            _resolver.AddLimit(count);

            return this;
        }

        /// <summary>
        /// Method to add DISTINCT in select.
        /// </summary>
        /// <param name="columnExp">Indicates column to use in the DISTINCT clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> Distinct(Expression<Func<TTable, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Distinct, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add DISTINCT in select.
        /// </summary>
        /// <typeparam name="TDistinct">Indicates a new type to use as a basis for column reference.</typeparam>
        /// <param name="columnExp">Indicates column to use in the DISTINCT clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> Distinct<TDistinct>(Expression<Func<TDistinct, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Distinct, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Where

        /// <summary>
        /// Method for adding 'WHERE' to the query. it can be invoked any times.
        /// </summary>
        /// <param name="conditionsExp">Indicates conditions to add to the 'WHERE' clause through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> Where(Expression<Func<TTable, bool>> conditionsExp, string tableAlias = null)
        {
            var sqlBuilderFluentWhereAnd = And(conditionsExp, tableAlias);

            return sqlBuilderFluentWhereAnd;
        }

        /// <summary>
        /// Method for adding 'WHERE' to the query. it can be invoked any times.
        /// </summary>
        /// <typeparam name="TWhere">Indicates a new type to use as a reference in the 'WHERE'.</typeparam>
        /// <param name="conditionsExp">Indicates conditions to add to the 'WHERE' clause through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> Where<TWhere>(Expression<Func<TWhere, bool>> conditionsExp, string tableAlias = null)
        {
            var sqlBuilderFluentWhereAnd = And(conditionsExp, tableAlias);

            return sqlBuilderFluentWhereAnd;
        }

        #endregion

        #region Where => AND

        /// <summary>
        /// Method for adding 'AND' operator to SELECT 'WHERE'.
        /// </summary>
        /// <param name="conditionsExp">Indicates conditions to add to the 'AND' in 'WHERE' clause through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> And(Expression<Func<TTable, bool>> conditionsExp, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.ResolveQuery(conditionsExp, tableAlias, TargetClauseType.Where, null);

            return this;
        }

        /// <summary>
        /// Method for adding 'AND' operator to SELECT 'WHERE'.
        /// </summary>
        /// <typeparam name="TAnd">Indicates a new type to use in the 'AND' in the 'WHERE'.</typeparam>
        /// <param name="conditionsExp">Indicates conditions to add to the 'WHERE' clause through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> And<TAnd>(Expression<Func<TAnd, bool>> conditionsExp, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.ResolveQuery(conditionsExp, tableAlias, TargetClauseType.Where, null);

            return this;
        }

        #endregion

        #region Where => OR

        /// <summary>
        /// Method for adding 'OR' operator to SELECT 'WHERE'.
        /// </summary>
        /// <param name="conditionsExp">Indicates conditions to add to the 'OR' in 'WHERE' clause through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> Or(Expression<Func<TTable, bool>> conditionsExp, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorOr(TargetClauseType.Where);
            _resolver.ResolveQuery(conditionsExp, tableAlias, TargetClauseType.Where, null);

            return this;
        }

        /// <summary>
        /// Method for adding 'OR' operator to SELECT 'WHERE'.
        /// </summary>
        /// <typeparam name="TOr">Indicates a new type to use in the 'OR' in the 'WHERE'.</typeparam>
        /// <param name="conditionsExp">Indicates conditions to add to the 'OR' in 'WHERE' clause through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> Or<TOr>(Expression<Func<TOr, bool>> conditionsExp, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorOr(TargetClauseType.Where);
            _resolver.ResolveQuery(conditionsExp, tableAlias, TargetClauseType.Where, null);

            return this;
        }

        #endregion

        #region Where => IN

        /// <summary>
        /// Method for adding 'IN' operator to SELECT 'WHERE'.
        /// </summary>
        /// <param name="conditionsExp">Indicates conditions to add to the 'IN' in 'WHERE' clause through expression.</param>
        /// <param name="values">Parameter to indicate the list of values to be used in the 'IN' clause.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> In(Expression<Func<TTable, object>> conditionsExp, List<object> values, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.AddClauseIn(conditionsExp, values, tableAlias, TargetClauseType.Where);

            return this;
        }

        /// <summary>
        /// Method for adding 'IN' operator to SELECT 'WHERE'.
        /// </summary>
        /// <typeparam name="TIn">Indicates a new type to use in the 'IN' in the 'WHERE'.</typeparam>
        /// <param name="conditionsExp">Indicates conditions to add to the 'IN' in 'WHERE' clause through expression.</param>
        /// <param name="values">Parameter to indicate the list of values to be used in the 'IN' clause.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> In<TIn>(Expression<Func<TIn, object>> conditionsExp, List<object> values, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.AddClauseIn(conditionsExp, values, tableAlias, TargetClauseType.Where);

            return this;
        }

        /// <summary>
        /// Method for adding 'NOT IN' operator to SELECT 'WHERE'.
        /// </summary>
        /// <param name="columnExp">Indicates column to use to the 'NOT IN' in 'WHERE' clause through expression.</param>
        /// <param name="values">Parameter to indicate the list of values to be used in the 'NOT IN' clause.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance</returns>
        public FluentSqlBuilder<TTable> NotIn(Expression<Func<TTable, object>> columnExp, List<object> values, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.AddClauseNotIn(columnExp, values, tableAlias, TargetClauseType.Where);

            return this;
        }

        /// <summary>
        /// Method to add 'NOT IN' function in select.
        /// </summary>
        /// <typeparam name="TNotIN">Indicates a new type to use as an 'NOT IN'.</typeparam>
        /// <param name="columnExp">Indicates column to use to the 'NOT IN' in 'WHERE' clause through expression.</param>
        /// <param name="values">Parameter to indicate the list of values to be used in the 'NOT IN' clause.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> NotIn<TNotIN>(Expression<Func<TNotIN, object>> columnExp, List<object> values, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.AddClauseNotIn(columnExp, values, tableAlias, TargetClauseType.Where);

            return this;
        }

        #endregion

        #region OrderBy

        /// <summary>
        /// Method to add 'ORDER BY ASC' function in select.
        /// </summary>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnsExp">Indicates columns to use to the 'ORDER BY ASC'.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> OrderBy(string tableAlias, params Expression<Func<TTable, object>>[] columnsExp)
        {
            _resolver.AddOrderBy(false, tableAlias, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'ORDER BY ASC' function in select.
        /// </summary>
        /// <param name="columnsExp">Indicates columns to use to the 'ORDER BY ASC'.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> OrderBy(params Expression<Func<TTable, object>>[] columnsExp)
        {
            _resolver.AddOrderBy(false, null, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'ORDER BY ASC' function in select.
        /// </summary>
        /// <typeparam name="TOrderBy">Indicates a new type to use as an ORDER BY parameter.</typeparam>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnsExp">Indicates columns to use to the 'ORDER BY ASC'.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> OrderBy<TOrderBy>(string tableAlias, params Expression<Func<TOrderBy, object>>[] columnsExp)
        {
            _resolver.AddOrderBy(false, tableAlias, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'ORDER BY ASC' function in select.
        /// </summary>
        /// <typeparam name="TOrderBy">Indicates a new type to use as an ORDER BY parameter.</typeparam>
        /// <param name="columnsExp">Indicates columns to use to the 'ORDER BY ASC'.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> OrderBy<TOrderBy>(params Expression<Func<TOrderBy, object>>[] columnsExp)
        {
            _resolver.AddOrderBy(false, null, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'ORDER BY DESC' function in select.
        /// </summary>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnsExp">Indicates columns to use to the 'ORDER BY DESC'.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> OrderByDescending(string tableAlias, params Expression<Func<TTable, object>>[] columnsExp)
        {
            _resolver.AddOrderBy(true, tableAlias, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'ORDER BY DESC' function in select.
        /// </summary>
        /// <param name="columnsExp">Indicates columns to use to the 'ORDER BY DESC'.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> OrderByDescending(params Expression<Func<TTable, object>>[] columnsExp)
        {
            _resolver.AddOrderBy(true, null, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'ORDER BY DESC' function in select.
        /// </summary>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnsExp">Indicates columns to use to the 'ORDER BY DESC'.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> OrderByDescending<TOrderBy>(string tableAlias, params Expression<Func<TOrderBy, object>>[] columnsExp)
        {
            _resolver.AddOrderBy(true, tableAlias, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'ORDER BY DESC' function in select.
        /// </summary>
        /// <typeparam name="TOrderBy">Indicates a new type to use as an 'ORDER BY DESC'.</typeparam>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnsExp">Indicates columns to use to the 'ORDER BY DESC'.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> OrderByDescending<TOrderBy>(params Expression<Func<TOrderBy, object>>[] columnsExp)
        {
            _resolver.AddOrderBy(true, null, columnsExp);

            return this;
        }

        #endregion

        #region Function => Count

        /// <summary>
        /// Method to add 'COUNT' function in select.
        /// </summary>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Count()
        {
            _resolver.SelectWithFunctionWithoutColumn<TTable>(SelectFunction.Count, null, null);

            return this;
        }

        /// <summary>
        /// Method to add 'COUNT' function in select.
        /// </summary>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Count(string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunctionWithoutColumn<TTable>(SelectFunction.Count, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add 'COUNT' function in select.
        /// </summary>
        /// <typeparam name="TCount">Indicates a new type to use as an 'COUNT' parameter.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'COUNT' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Count<TCount>(Expression<Func<TCount, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Count, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add 'COUNT' function in select.
        /// </summary>
        /// <param name="columnExp">Indicates column to use in the 'COUNT' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Count(Expression<Func<TTable, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Count, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Function => Sum

        /// <summary>
        /// Method to add 'SUM' function in select.
        /// </summary>
        /// <param name="columnExp">Indicates column to use in the 'SUM' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Sum(Expression<Func<TTable, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Sum, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add 'SUM' function in select.
        /// </summary>
        /// <typeparam name="TSum">Indicates a new type to use as an 'SUM' parameter.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'SUM' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Sum<TSum>(Expression<Func<TSum, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Sum, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Function => Max

        /// <summary>
        /// Method to add 'MAX' function in select.
        /// </summary>
        /// <param name="columnExp">Indicates column to use in the 'MAX' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Max(Expression<Func<TTable, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Max, tableAlias, columnAlias);

            return this;
        }


        /// <summary>
        /// Method to add 'MAX' function in select.
        /// </summary>
        /// <typeparam name="TMax">Indicates a new type to use as an 'MAX' parameter.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'MAX' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Max<TMax>(Expression<Func<TMax, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Max, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Function => Min

        /// <summary>
        /// Method to add 'MIN' function in select.
        /// </summary>
        /// <param name="columnExp">Indicates column to use in the 'MIN' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Min(Expression<Func<TTable, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Min, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add 'MIN' function in select.
        /// </summary>
        /// <typeparam name="TMin">Indicates a new type to use as an 'MIN' parameter</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'MIN' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Min<TMin>(Expression<Func<TMin, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Min, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Function => Avg

        /// <summary>
        /// Method to add 'AVG' function in select.
        /// </summary>
        /// <param name="columnExp">Indicates column to use in the 'AVG' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Avg(Expression<Func<TTable, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Avg, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add 'AVG' function in select.
        /// </summary>
        /// <typeparam name="TAvg">Indicates a new type to use as an 'AVG' parameter.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'AVG' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Avg<TAvg>(Expression<Func<TAvg, object>> columnExp, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(columnExp, SelectFunction.Avg, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Joins

        /// <summary>
        /// Method to add 'INNER JOIN' function in select.
        /// </summary>
        /// <typeparam name="TTableJoin">Indicates the type to which the 'INNER JOIN' is targeted.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'INNER JOIN' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTableJoin> InnerJoin<TTableJoin>(Expression<Func<TTable, TTableJoin, bool>> columnExp, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoin(columnExp, JoinType.Inner, _targetToSelect, tableAlias);

            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method to add 'INNER JOIN' function in select.
        /// </summary>
        /// <typeparam name="TTableLeft">Indicates the source table for the 'INNER JOIN'.</typeparam>
        /// <typeparam name="TTableJoin">Indicates the type to which the 'INNER JOIN' is targeted.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'INNER JOIN' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTableJoin> InnerJoin<TTableLeft, TTableJoin>(Expression<Func<TTableLeft, TTableJoin, bool>> columnExp, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoinWithDefineLeftTable(columnExp, JoinType.Inner, _targetToSelect, tableAlias);

            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method to add 'LEFT JOIN' function in select.
        /// </summary>
        /// <typeparam name="TTableJoin">Indicates the type to which the 'LEFT JOIN' is targeted.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'LEFT JOIN' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTableJoin> LeftJoin<TTableJoin>(Expression<Func<TTable, TTableJoin, bool>> columnExp, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoin(columnExp, JoinType.Left, _targetToSelect, tableAlias);

            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method to add 'LEFT JOIN' function in select.
        /// </summary>
        /// <typeparam name="TTableJoin">Indicates the type to which the 'LEFT JOIN' is targeted.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'LEFT JOIN' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTableJoin> LeftJoin<TTableLeft, TTableJoin>(Expression<Func<TTableLeft, TTableJoin, bool>> columnExp, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoinWithDefineLeftTable(columnExp, JoinType.Left, _targetToSelect, tableAlias);

            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method to add 'RIGHT JOIN' function in select.
        /// </summary>
        /// <typeparam name="TTableJoin">Indicates the type to which the 'RIGHT JOIN' is targeted.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'RIGHT JOIN' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTableJoin> RightJoin<TTableJoin>(Expression<Func<TTable, TTableJoin, bool>> columnExp, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoin(columnExp, JoinType.Right, _targetToSelect, tableAlias);

            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method to add 'RIGHT JOIN' function in select.
        /// </summary>
        /// <typeparam name="TTableLeft">Indicates the type to which the 'RIGHT JOIN' is targeted.</typeparam>
        /// <typeparam name="TTableJoin">Indicates the type to which the 'RIGHT JOIN' is targeted.</typeparam>
        /// <param name="columnExp">Indicates column to use in the 'RIGHT JOIN' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTableJoin> RightJoin<TTableLeft, TTableJoin>(Expression<Func<TTableLeft, TTableJoin, bool>> expression, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoinWithDefineLeftTable(expression, JoinType.Right, _targetToSelect, tableAlias);

            return sqlBuilderFluentJoin;
        }

        #endregion

        #region GroupBy

        /// <summary>
        /// Method to add 'GROUP BY' function in select.
        /// </summary>
        /// <param name="columnsExp">Indicates column to use in the 'MIN' clause, through expression.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> GroupBy(params Expression<Func<TTable, object>>[] columnsExp)
        {
            _resolver.AddGroupBy(null, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'GROUP BY' function in select.
        /// </summary>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnsExp">Indicates column to use in the 'MIN' clause, through expression.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> GroupBy(string tableAlias, params Expression<Func<TTable, object>>[] columnsExp)
        {
            _resolver.AddGroupBy(tableAlias, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'GROUP BY' function in select.
        /// </summary>
        /// <typeparam name="TGroupBy">Indicates a new type to use for table columns.</typeparam>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnsExp">Indicates column to use in the 'MIN' clause, through expression.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> GroupBy<TGroupBy>(string tableAlias, params Expression<Func<TGroupBy, object>>[] columnsExp)
        {
            _resolver.AddGroupBy(tableAlias, columnsExp);

            return this;
        }

        /// <summary>
        /// Method to add 'GROUP BY' function in select.
        /// </summary>
        /// <typeparam name="TGroupBy">Indicates a new type to use for table columns.</typeparam>
        /// <param name="columnsExp">Indicates column to use in the 'MIN' clause, through expression.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> GroupBy<TGroupBy>(params Expression<Func<TGroupBy, object>>[] columnsExp)
        {
            _resolver.AddGroupBy(null, columnsExp);

            return this;
        }

        #endregion

        #region Having

        /// <summary>
        /// Method to add 'HAVING' function in select.
        /// </summary>
        /// <param name="conditionsExp">Indicates column to use in the 'HAVING' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Having(SelectFunction selectFunction, Expression<Func<TTable, bool>> conditionsExp, string tableAlias = null)
        {
            _resolver.AddHaving(tableAlias, conditionsExp, TargetClauseType.Having, selectFunction);

            return this;
        }

        /// <summary>
        /// Method to add 'HAVING' function in select.
        /// </summary>
        /// <typeparam name="THaving">Indicates a new type to use for table columns.</typeparam>
        /// <param name="conditionsExp">Indicates column to use in the 'HAVING' clause, through expression.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder<TTable>. Corresponds to the root instance.</returns>
        public FluentSqlBuilder<TTable> Having<THaving>(SelectFunction selectFunction, Expression<Func<THaving, bool>> conditionsExp, string tableAlias = null)
        {
            _resolver.AddHaving(tableAlias, conditionsExp, TargetClauseType.Having, selectFunction);

            return this;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Override method to generate final select string.
        /// </summary>
        /// <returns>Returns select ready to run in the database, with necessary parameters.</returns>
        public override string ToString()
        {
            var selectSql = String.Empty;

            switch (_targetToSelect)
            {
                case TargetToSelect.Common:
                    selectSql = _sqlQueryBuilder.GetSelectString();
                    break;
                case TargetToSelect.Pagination:
                    selectSql = _sqlQueryBuilder.GetSelectPaginationString();
                    break;
            }

            return selectSql;
        }

        #endregion

        #region Privates

        private void DefineSqlAdapter(SqlAdapterType sqlAdapterType, Type typeTable, SqlBuilderFormatting formatting, string tableAlias)
        {
            var sqlAdapter = GetInstanceSqlAdapter(sqlAdapterType);

            _sqlQueryBuilder = new FluentSqlQueryBuilder(sqlAdapter, typeTable, formatting, null, tableAlias);
            _resolver = new LambdaResolver(_sqlQueryBuilder, tableAlias);
        }

        private static ISqlBuilderFluentAdapter GetInstanceSqlAdapter(SqlAdapterType sqlAdapterType)
        {
            ISqlBuilderFluentAdapter sqlAdapter;

            switch (sqlAdapterType)
            {
                case SqlAdapterType.SqlServer2008:
                    sqlAdapter = new SqlServer2008Adapter();
                    break;
                case SqlAdapterType.SqlServer2012:
                case SqlAdapterType.SqlServer2014:
                case SqlAdapterType.SqlServer2016:
                case SqlAdapterType.SqlServer2017:
                case SqlAdapterType.SqlServer2019:
                    sqlAdapter = new SqlServerGreaterEqual2012Adapter();
                    break;
                case SqlAdapterType.MySql:
                    sqlAdapter = new MySqlAdapter();
                    break;
                default:
                    throw new SqlBuilderException("'SqlAdapterType' not defined");
            }

            return sqlAdapter;
        }

        private void DefineExtension()
        {
            _sqlBuilderFluentExtension = new SqlBuilderFluentExtension<TTable>(_resolver, _sqlQueryBuilder);
        }

        private void DefineTargetToSelect(TargetToSelect targetToSelect)
        {
            _targetToSelect = targetToSelect;
        }

        #endregion
    }
}