using FluentSqlBuilder.Core.Middlewares.Inputs;
using FluentSqlBuilder.Core.Middlewares.Services.Interfaces;
using Microsoft.Extensions.Options;
using SqlBuilderFluent;

namespace FluentSqlBuilder.Core.Middlewares.Services
{
    public class FluentSqlBuilderService : IFluentSqlBuilderService
    {
        private readonly FluentSqlBuilderMiddlewareOptions _fluentSqlBuilderMiddlewareOptions;

        public FluentSqlBuilderService(IOptions<FluentSqlBuilderMiddlewareOptions> fluentSqlBuilderMiddlewareOptions)
        {
            _fluentSqlBuilderMiddlewareOptions = fluentSqlBuilderMiddlewareOptions.Value;
        }

        public FluentSqlBuilderService(FluentSqlBuilderMiddlewareOptions fluentSqlBuilderMiddlewareOptions)
        {
            _fluentSqlBuilderMiddlewareOptions = fluentSqlBuilderMiddlewareOptions;
        }

        public FluentSqlBuilder<TTable> From<TTable>(string tableAlias = null)
        {
            var sqlBuilder = new FluentSqlBuilder<TTable>(_fluentSqlBuilderMiddlewareOptions.SqlAdapterType, _fluentSqlBuilderMiddlewareOptions.Formatting, tableAlias);

            return sqlBuilder;
        }
    }
}