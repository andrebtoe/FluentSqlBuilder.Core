using FluentSqlBuilder.Core.Middlewares.Inputs;
using FluentSqlBuilder.Core.Middlewares.Services;
using FluentSqlBuilder.Core.Middlewares.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentSqlBuilder.Core.Middlewares
{
    public static class FluentSqlBuilderMiddlewareService
    {
        public static void AddFluentSqlBuilder(this IServiceCollection services, Action<FluentSqlBuilderMiddlewareOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddTransient<IFluentSqlBuilderService, FluentSqlBuilderService>();
        }
    }
}