using CFT.Application;
using CFT.FileProvider;
using CFT.Hosting.Decorators;
using CFT.Hosting.Extensions;
using CFT.Hosting.Middleware;
using CFT.MiddleWare.Base;
using CFT.MiddleWare.Transformations.FileName;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MiddleWare.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CFT.Hosting
{
    public class CFTHostBuilder : HostBuilder
    {
        public CFTHostBuilder()
        {
        }

        public new IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            void additionConfigureDelegate(HostBuilderContext ctx, IServiceCollection services)
            {
                configureDelegate?.Invoke(ctx, services);
                services.AddLogging();
                services.AddHostedService<FileScanerHostedService>();
                services.AddTransient<ICFTReadAllProcess, CFTReadAllProcess>();
                services.Decorate<ICFTReadAllProcess, CFTReadAllProcessLodDecorator>();

                services.AddSingleton<ICFTMiddlewareBuilder>(provider =>
                {
                    return GetCFTMiddlewareBuilder(ctx, provider);
                });
                services.AddSingleton<IFileProviderFactory, FileProviderFactory>();
                services.Configure<FileScanerOptions>(options => ctx.Configuration.GetSection("FileScannerHostedService").Bind(options));

                RegisterMiddlewareServices(services);
            }

            base.ConfigureServices(additionConfigureDelegate);
            return this;
        }

        private IServiceCollection RegisterMiddlewareServices(IServiceCollection services)
        {
            services.AddTransformFileNameServices();

            return services;
        }

        private ICFTMiddlewareBuilder GetCFTMiddlewareBuilder(HostBuilderContext ctx, IServiceProvider provider)
        {
            var stepSection = ctx.Configuration.GetSection("Steps");
            var options = provider.GetRequiredService<IOptions<FileScanerOptions>>().Value;

            var result = new CFTMiddlewareBuilder(provider);
            result.UseMiddleware<LogScopeMiddleware, CFTFileContext>();
            if (options.UseBackup)
                result.UseBackup(options.BackupPath);

            new AppicationConfiguration(options.FileProviderType, options.FileProviderSettings)
                .ConfigureApplication(result, stepSection);

            return result;
        }
    }
}
