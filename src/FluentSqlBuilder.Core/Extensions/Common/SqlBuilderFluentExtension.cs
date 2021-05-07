using SqlBuilderFluent.Core.Inputs;
using SqlBuilderFluent.Inputs;
using SqlBuilderFluent.Lambdas;
using System;
using System.Linq.Expressions;

namespace SqlBuilderFluent.Core.Extensions.Common
{
    public class SqlBuilderFluentExtension<TTable>
    {
        private readonly LambdaResolver _resolver;
        private readonly FluentSqlQueryBuilder _sqlQueryBuilder;

        public SqlBuilderFluentExtension(LambdaResolver resolver, FluentSqlQueryBuilder sqlQueryBuilder)
        {
            _resolver = resolver;
            _sqlQueryBuilder = sqlQueryBuilder;
        }

        public FluentSqlBuilder<TTableJoin> AddJoin<TTableJoin>(Expression<Func<TTable, TTableJoin, bool>> expression, JoinType joinType, TargetToSelect targetToSelect, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = AddJoinCommon(expression, joinType, targetToSelect, tableAlias);

            return sqlBuilderFluentJoin;
        }

        public FluentSqlBuilder<TTableJoin> AddJoinWithDefineLeftTable<TTableLeft, TTableJoin>(Expression<Func<TTableLeft, TTableJoin, bool>> expression, JoinType joinType, TargetToSelect targetToSelect, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = AddJoinCommon(expression, joinType, targetToSelect, tableAlias);

            return sqlBuilderFluentJoin;
        }

        public FluentSqlBuilder<TTableJoin> AddJoinCommon<TTableLeft, TTableJoin>(Expression<Func<TTableLeft, TTableJoin, bool>> expression, JoinType joinType, TargetToSelect targetToSelect, string tableAlias = null)
        {
            var sqlBuilderFluentJoin = new FluentSqlBuilder<TTableJoin>(_sqlQueryBuilder, _resolver, targetToSelect);

            _resolver.AddJoinByType(expression, joinType, tableAlias);

            return sqlBuilderFluentJoin;
        }
    }
}