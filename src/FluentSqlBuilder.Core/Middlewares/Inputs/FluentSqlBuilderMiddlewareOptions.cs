using SqlBuilderFluent.Exceptions;
using SqlBuilderFluent.Lambdas;
using SqlBuilderFluent.Types;
using System;
using System.Collections.Generic;

namespace FluentSqlBuilder.Core.Middlewares.Inputs
{
    public class FluentSqlBuilderMiddlewareOptions
    {
        private IDictionary<Type, Type> _providers = new Dictionary<Type, Type>();

        public SqlAdapterType SqlAdapterType { get; set; }
        public SqlBuilderFormatting Formatting { get; set; }
        public IDictionary<Type, Type> Providers
        {
            get
            {
                return _providers;
            }
        }

        public void AddProvider<TProvider>(Type type)
            where TProvider : IFluentSqlBuilderProvider
        {
            var keyExists = _providers.ContainsKey(type);

            if (keyExists)
                throw new SqlBuilderException($"Type '{type.Name}' already added");

            _providers.Add(type, typeof(TProvider));
        }
    }
}