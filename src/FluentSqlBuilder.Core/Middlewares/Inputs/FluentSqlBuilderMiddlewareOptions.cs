using SqlBuilderFluent.Types;

namespace FluentSqlBuilder.Core.Middlewares.Inputs
{
    public class FluentSqlBuilderMiddlewareOptions
    {
        public SqlAdapterType SqlAdapterType { get; set; }
        public SqlBuilderFormatting Formatting { get; set; }
    }
}