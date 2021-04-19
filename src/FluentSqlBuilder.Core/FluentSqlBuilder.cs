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
        /// <param name="expression">Indicates the columns to be used in the projection.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Projection(Expression<Func<TTable, object>> expression, string tableAlias = null)
        {
            _resolver.Select(expression, tableAlias);

            return this;
        }

        /// <summary>
        /// By default, queries return all columns. To limit, you can include a projection to specify or restrict columns to return.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the projection.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Projection<TTableProjection>(Expression<Func<TTableProjection, object>> expression, string tableAlias = null)
        {
            _resolver.Select(expression, tableAlias);

            return this;
        }

        #endregion

        #region Clause

        /// <summary>
        /// Method for page structure for select.
        /// </summary>
        /// <param name="pageSize">Indicates the amount of line per page.</param>
        /// <param name="pageNumber">Indicates the number on the page.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Pagination(int pageSize, int? pageNumber = null)
        {
            DefineTargetToSelect(TargetToSelect.Pagination);

            _resolver.AddPagination(pageSize, pageNumber);

            return this;
        }

        /// <summary>
        /// Method to limit the number of lines.
        /// </summary>
        /// <param name="count">Defines the number of rows that the query should return.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Limit(int count)
        {
            _resolver.AddLimit(count);

            return this;
        }

        /// <summary>
        /// Method to add DISTINCT function in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the AVG.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Distinct(Expression<Func<TTable, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.DISTINCT, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add DISTINCT function in select.
        /// </summary>
        /// <typeparam name="TDistinct">Indicates a new type to use as an DISTINCT.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the AVG.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Distinct<TDistinct>(Expression<Func<TDistinct, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.DISTINCT, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Where

        /// <summary>
        /// Method to add WHERE in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the WHERE.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Where(Expression<Func<TTable, bool>> expression, string tableAlias = null)
        {
            var sqlBuilderFluentWhereAnd = And(expression, tableAlias);

            return sqlBuilderFluentWhereAnd;
        }

        /// <summary>
        /// Method to add WHERE in select.
        /// </summary>
        /// <typeparam name="TWhere">Indicates a new type to use as an WHERE</typeparam>
        /// <param name="expression">Indicates the columns to be used in the WHERE.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Where<TWhere>(Expression<Func<TWhere, bool>> expression, string tableAlias = null)
        {
            var sqlBuilderFluentWhereAnd = And(expression, tableAlias);

            return sqlBuilderFluentWhereAnd;
        }

        #endregion

        #region Where => AND

        /// <summary>
        /// Method to add AND in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the AND.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> And(Expression<Func<TTable, bool>> expression, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.ResolveQuery(expression, tableAlias, TargetClauseType.Where, null);

            return this;
        }

        /// <summary>
        /// Method to add AND in select.
        /// </summary>
        /// <typeparam name="TAnd">Indicates a new type to use as an AND.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the AND.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> And<TAnd>(Expression<Func<TAnd, bool>> expression, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.ResolveQuery(expression, tableAlias, TargetClauseType.Where, null);

            return this;
        }

        #endregion

        #region Where => OR

        /// <summary>
        /// Method to add OR in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in OR.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Or(Expression<Func<TTable, bool>> expression, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorOr(TargetClauseType.Where);
            _resolver.ResolveQuery(expression, tableAlias, TargetClauseType.Where, null);

            return this;
        }

        /// <summary>
        /// Method to add OR in select.
        /// </summary>
        /// <typeparam name="TOr">Indicates a new type to use as an OR.</typeparam>
        /// <param name="expression">Indicates the columns to be used in OR.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Or<TOr>(Expression<Func<TOr, bool>> expression, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorOr(TargetClauseType.Where);
            _resolver.ResolveQuery(expression, tableAlias, TargetClauseType.Where, null);

            return this;
        }

        #endregion

        #region Where => IN

        /// <summary>
        /// Method to add IN function in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the IN.</param>
        /// <param name="values">Parameter to indicate the list of values to be used in the IN clause.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> In(Expression<Func<TTable, object>> expression, List<object> values, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.AddClauseIn(expression, values, tableAlias, TargetClauseType.Where);

            return this;
        }

        /// <summary>
        /// Method to add IN function in select.
        /// </summary>
        /// <typeparam name="TIn">Indicates a new type to use as an IN.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the IN.</param>
        /// <param name="values">Parameter to indicate the list of values to be used in the IN clause.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> In<TIn>(Expression<Func<TIn, object>> expression, List<object> values, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.AddClauseIn(expression, values, tableAlias, TargetClauseType.Where);

            return this;
        }

        /// <summary>
        /// Method to add NOT IN function in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the NOT IN.</param>
        /// <param name="values">Parameter to indicate the list of values to be used in the NOT IN clause.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> NotIn(Expression<Func<TTable, object>> expression, List<object> values, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.AddClauseNotIn(expression, values, tableAlias, TargetClauseType.Where);

            return this;
        }

        /// <summary>
        /// Method to add NOT IN function in select.
        /// </summary>
        /// <typeparam name="TNotIN">Indicates a new type to use as an NOT IN.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the NOT IN.</param>
        /// <param name="values">Parameter to indicate the list of values to be used in the NOT IN clause.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> NotIn<TNotIN>(Expression<Func<TNotIN, object>> expression, List<object> values, string tableAlias = null)
        {
            _sqlQueryBuilder.AddOperatorAnd(TargetClauseType.Where);
            _resolver.AddClauseNotIn(expression, values, tableAlias, TargetClauseType.Where);

            return this;
        }

        #endregion

        #region OrderBy

        /// <summary>
        /// Method to add ORDER BY COLUMNS ASC in select.
        /// </summary>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="expressions">Indicates the columns to be used in the ORDER BY COLUMNS ASC.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> OrderBy(string tableAlias, params Expression<Func<TTable, object>>[] expressions)
        {
            _resolver.AddOrderBy(false, tableAlias, expressions);

            return this;
        }

        /// <summary>
        /// Method to add ORDER BY COLUMNS ASC in select.
        /// </summary>
        /// <param name="expressions">Indicates the columns to be used in the ORDER BY COLUMNS ASC.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> OrderBy(params Expression<Func<TTable, object>>[] expressions)
        {
            _resolver.AddOrderBy(false, null, expressions);

            return this;
        }

        /// <summary>
        /// Method to add ORDER BY COLUMNS ASC in select.
        /// </summary>
        /// <typeparam name="TOrderBy">Indicates a new type to use as an ORDER BY parameter.</typeparam>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="expressions">Indicates the columns to be used in the ORDER BY COLUMNS ASC.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> OrderBy<TOrderBy>(string tableAlias, params Expression<Func<TOrderBy, object>>[] expressions)
        {
            _resolver.AddOrderBy(false, tableAlias, expressions);

            return this;
        }

        /// <summary>
        /// Method to add ORDER BY COLUMNS ASC in select.
        /// </summary>
        /// <typeparam name="TOrderBy">Indicates a new type to use as an ORDER BY parameter.</typeparam>
        /// <param name="expressions">Indicates the columns to be used in the ORDER BY COLUMNS ASC.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> OrderBy<TOrderBy>(params Expression<Func<TOrderBy, object>>[] expressions)
        {
            _resolver.AddOrderBy(false, null, expressions);

            return this;
        }

        /// <summary>
        /// Method to add ORDER BY COLUMNS DESC in select.
        /// </summary>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="expressions">Indicates the columns to be used in the ORDER BY COLUMNS DESC.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> OrderByDescending(string tableAlias, params Expression<Func<TTable, object>>[] expressions)
        {
            _resolver.AddOrderBy(true, tableAlias, expressions);

            return this;
        }

        /// <summary>
        /// Method to add ORDER BY COLUMNS DESC in select.
        /// </summary>
        /// <param name="expressions">Indicates the columns to be used in the ORDER BY COLUMNS DESC.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> OrderByDescending(params Expression<Func<TTable, object>>[] expressions)
        {
            _resolver.AddOrderBy(true, null, expressions);

            return this;
        }

        /// <summary>
        /// Method to add ORDER BY COLUMNS DESC in select.
        /// </summary>
        /// <typeparam name="TOrderBy">Indicates a new type to use as an ORDER BY COLUMN DESC.</typeparam>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="expressions">Indicates the columns to be used in the ORDER BY COLUMNS DESC.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> OrderByDescending<TOrderBy>(string tableAlias, params Expression<Func<TOrderBy, object>>[] expressions)
        {
            _resolver.AddOrderBy(true, tableAlias, expressions);

            return this;
        }

        /// <summary>
        /// Method to add ORDER BY COLUMNS DESC in select.
        /// </summary>
        /// <typeparam name="TOrderBy">Indicates a new type to use as an ORDER BY COLUMN DESC.</typeparam>
        /// <param name="expressions">Indicates the columns to be used in the ORDER BY COLUMNS DESC.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> OrderByDescending<TOrderBy>(params Expression<Func<TOrderBy, object>>[] expressions)
        {
            _resolver.AddOrderBy(true, null, expressions);

            return this;
        }

        #endregion

        #region Function => Count

        /// <summary>
        /// Method to add COUNT function in select. SELECT COUNT(1)
        /// </summary>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Count()
        {
            _resolver.SelectWithFunctionWithoutColumn<TTable>(SelectFunction.COUNT, null, null);

            return this;
        }

        /// <summary>
        /// Method to add COUNT function in select.
        /// </summary>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Count(string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunctionWithoutColumn<TTable>(SelectFunction.COUNT, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add COUNT function in select.
        /// </summary>
        /// <typeparam name="TCount">Indicates a new type to use as an COUNT parameter.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the COUNT.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Count<TCount>(Expression<Func<TCount, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.COUNT, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add COUNT function in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the COUNT.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Count(Expression<Func<TTable, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.COUNT, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Function => Sum

        /// <summary>
        /// Method to add SUM function in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the AVG.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Sum(Expression<Func<TTable, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.SUM, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add SUM function in select.
        /// </summary>
        /// <typeparam name="TSum">Indicates a new type to use as an SUM parameter.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the AVG.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Sum<TSum>(Expression<Func<TSum, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.SUM, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Function => Max

        /// <summary>
        /// Method to add MAX function in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the AVG.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Max(Expression<Func<TTable, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.MAX, tableAlias, columnAlias);

            return this;
        }


        /// <summary>
        /// Method to add MAX function in select.
        /// </summary>
        /// <typeparam name="TMax">Indicates a new type to use as an MAX parameter.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the AVG.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Max<TMax>(Expression<Func<TMax, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.MAX, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Function => Min

        /// <summary>
        /// Method to add MIN function in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the MIN.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Min(Expression<Func<TTable, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.MIN, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add MIN function in select.
        /// </summary>
        /// <typeparam name="TMin">Indicates a new type to use as an MIN parameter</typeparam>
        /// <param name="expression">Indicates the columns to be used in the MIN.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Min<TMin>(Expression<Func<TMin, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.MIN, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Function => Avg

        /// <summary>
        /// Method to add AVG function in select.
        /// </summary>
        /// <param name="expression">Indicates the columns to be used in the AVG.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Avg(Expression<Func<TTable, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.AVG, tableAlias, columnAlias);

            return this;
        }

        /// <summary>
        /// Method to add AVG function in select.
        /// </summary>
        /// <typeparam name="TAvg">Indicates a new type to use as an AVG parameter.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the AVG.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="columnAlias">Use this parameter when it is necessary to reference the column by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Avg<TAvg>(Expression<Func<TAvg, object>> expression, string tableAlias = null, string columnAlias = null)
        {
            _resolver.SelectWithFunction(expression, SelectFunction.AVG, tableAlias, columnAlias);

            return this;
        }

        #endregion

        #region Joins

        /// <summary>
        /// Method for adding inner join to the query.
        /// </summary>
        /// <typeparam name="TTableJoin">Indicates the type to which the inner join is targeted.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the inner join.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder with the new type used in the inner join.</returns>
        public FluentSqlBuilder<TTableJoin> InnerJoin<TTableJoin>(Expression<Func<TTable, TTableJoin, bool>> expression, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoin(expression, JoinType.Inner, _targetToSelect, tableAlias);
            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method for grouping columns and using grouping functions.
        /// </summary>
        /// <typeparam name="TTableLeft">Indicates the type of table source to perform the inner join.</typeparam>
        /// <typeparam name="TTableJoin">Indicates the type to which the inner join is targeted.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the inner join.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder with the new type used in the inner join.</returns>
        public FluentSqlBuilder<TTableJoin> InnerJoin<TTableLeft, TTableJoin>(Expression<Func<TTableLeft, TTableJoin, bool>> expression, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoinWithDefineLeftTable(expression, JoinType.Inner, _targetToSelect, tableAlias);
            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method for adding left join to the query.
        /// </summary>
        /// <typeparam name="TTableJoin">Indicates the type to which the left join is targeted.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the left join.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder with the new type used in the left join.</returns>
        public FluentSqlBuilder<TTableJoin> LeftJoin<TTableJoin>(Expression<Func<TTable, TTableJoin, bool>> expression, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoin(expression, JoinType.Left, _targetToSelect, tableAlias);
            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method for adding left join to the query.
        /// </summary>
        /// <typeparam name="TTableLeft">Indicates the type of table source to perform the left join.</typeparam>
        /// <typeparam name="TTableJoin">Indicates the type to which the left join is targeted.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the left join.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder with the new type used in the left join.</returns>
        public FluentSqlBuilder<TTableJoin> LeftJoin<TTableLeft, TTableJoin>(Expression<Func<TTableLeft, TTableJoin, bool>> expression, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoinWithDefineLeftTable(expression, JoinType.Left, _targetToSelect, tableAlias);
            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method for adding right join to the query.
        /// </summary>
        /// <typeparam name="TTableJoin">Indicates the type to which the right join is targeted.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the right join.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder with the new type used in the right join.</returns>
        public FluentSqlBuilder<TTableJoin> RightJoin<TTableJoin>(Expression<Func<TTable, TTableJoin, bool>> expression, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoin(expression, JoinType.Right, _targetToSelect, tableAlias);
            return sqlBuilderFluentJoin;
        }

        /// <summary>
        /// Method for adding right join to the query.
        /// </summary>
        /// <typeparam name="TTableLeft">Indicates the type of table source to perform the right join.</typeparam>
        /// <typeparam name="TTableJoin">Indicates the type to which the right join is targeted.</typeparam>
        /// <param name="expression">Indicates the columns to be used in the right join.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder with the new type used in the right join.</returns>
        public FluentSqlBuilder<TTableJoin> RightJoin<TTableLeft, TTableJoin>(Expression<Func<TTableLeft, TTableJoin, bool>> expression, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = _sqlBuilderFluentExtension.AddJoinWithDefineLeftTable(expression, JoinType.Right, _targetToSelect, tableAlias);
            return sqlBuilderFluentJoin;
        }

        #endregion

        #region GroupBy

        /// <summary>
        /// Method for grouping columns and using grouping functions.
        /// </summary>
        /// <param name="expressions">Indicates the columns to be used in the grouping.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> GroupBy(params Expression<Func<TTable, object>>[] expressions)
        {
            _resolver.AddGroupBy(null, expressions);

            return this;
        }

        /// <summary>
        /// Method for grouping columns and using grouping functions.
        /// </summary>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="expressions">Indicates the columns to be used in the grouping.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> GroupBy(string tableAlias, params Expression<Func<TTable, object>>[] expressions)
        {
            _resolver.AddGroupBy(tableAlias, expressions);

            return this;
        }

        /// <summary>
        /// Method for grouping columns and using grouping functions.
        /// </summary>
        /// <typeparam name="TGroupBy">Indicates a new type to use for table columns.</typeparam>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <param name="expressions">Indicates the columns to be used in the grouping.</param>
        /// <returns></returns>
        public FluentSqlBuilder<TTable> GroupBy<TGroupBy>(string tableAlias, params Expression<Func<TGroupBy, object>>[] expressions)
        {
            _resolver.AddGroupBy(tableAlias, expressions);

            return this;
        }

        /// <summary>
        /// Method for grouping columns and using grouping functions.
        /// </summary>
        /// <typeparam name="TGroupBy">Indicates a new type to use for table columns.</typeparam>
        /// <param name="expressions">Indicates the columns to be used in the grouping.</param>
        /// <returns></returns>
        public FluentSqlBuilder<TTable> GroupBy<TGroupBy>(params Expression<Func<TGroupBy, object>>[] expressions)
        {
            _resolver.AddGroupBy(null, expressions);

            return this;
        }

        #endregion

        #region Having

        /// <summary>
        /// Method for grouping columns and using grouping functions.
        /// </summary>
        /// <param name="expressions">Indicates the columns to be used in the grouping.</param>
        /// <param name="tableAlias">Use this parameter when it is necessary to reference the table by alias.</param>
        /// <returns>Returns the instance of FluentSqlBuilder.</returns>
        public FluentSqlBuilder<TTable> Having(SelectFunction selectFunction, Expression<Func<TTable, bool>> expressionFilter, string tableAlias = null)
        {
            _resolver.AddHaving(tableAlias, expressionFilter, TargetClauseType.Having, selectFunction);

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