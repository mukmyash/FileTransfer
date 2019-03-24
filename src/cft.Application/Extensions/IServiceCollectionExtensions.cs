using cft.Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterFlowBuilder(this IServiceCollection services)
        {
            services.TryAddScoped<IFlowBuilder, FlowBuilder>();
            return services;
        }
    }
}
