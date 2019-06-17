using CFT.MiddleWare.Base;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MiddleWare.Abstractions;
using MiddleWare.Abstractions.Extensions;
using System;

namespace CFT.MiddleWare.Transformations.FileName
{
    public static class MiddleWareBuilderExtension
    {
        public static IMiddlewareBuilder<CFTFileContext> UseTransformFileName(
           this IMiddlewareBuilder<CFTFileContext> app,
           IConfigurationSection configSection)
        {
            return app.UseTransformFileName(options =>
            {
                configSection.Bind(options);
            });
        }

        public static IMiddlewareBuilder<CFTFileContext> UseTransformFileName(
            this IMiddlewareBuilder<CFTFileContext> app,
            Action<FileNameTransformOptions> configOption)
        {
            var options = new FileNameTransformOptions();
            configOption(options);
            app.UseMiddleware<FileNameTransformMiddleWare, CFTFileContext>(options);
            return app;
        }

        public static IServiceCollection AddTransformFileNameServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IParameterExtracterFactory, ParameterExtracterFactory>();
            services.TryAddSingleton<IParameterDescriptionOptionFactory, ParameterDescriptionOptionFactory>();
            return services;
        }
    }
}
