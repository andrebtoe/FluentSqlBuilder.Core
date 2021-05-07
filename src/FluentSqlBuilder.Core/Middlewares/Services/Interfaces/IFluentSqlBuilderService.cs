using SqlBuilderFluent;

namespace FluentSqlBuilder.Core.Middlewares.Services.Interfaces
{
    public interface IFluentSqlBuilderService
    {
        FluentSqlBuilder<TTable> From<TTable>(string tableAlias = null);
    }
}