using System.Collections.Generic;

namespace SqlBuilderFluent
{
    public abstract class BaseFluentSqlBuilder
    {
        protected FluentSqlQueryBuilder _sqlQueryBuilder;

        public IDictionary<string, object> GetParameters()
        {
            var parameters = _sqlQueryBuilder.GetParameters();

            return parameters;
        }
    }
}