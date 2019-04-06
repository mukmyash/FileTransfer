using CFT.Hosting.Decorators;
using CFT.Hosting.Extensions;
using CFT.Hosting.Middleware;
using CFT.MiddleWare.Base;
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
        Action<HostBuilderContext, ICFTMiddlewareBuilder> _configure;

        public CFTHostBuilder()
        {
        }

        public CFTHostBuilder ConfigureApplication(Action<HostBuilderContext, ICFTMiddlewareBuilder> configure)
        {
            _configure = configure;
            return this;
        }

        public new IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            void additionConfigureDelegate(HostBuilderContext ctx, IServiceCollection services)
            {
                configureDelegate(ctx, services);
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
            }

            base.ConfigureServices(additionConfigureDelegate);
            return this;
        }

        private ICFTMiddlewareBuilder GetCFTMiddlewareBuilder(HostBuilderContext ctx, IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<FileScanerOptions>>().Value;

            var result = new CFTMiddlewareBuilder(provider);
            result.UseMiddleware<LogScopeMiddleware, CFTFileContext>();
            if (options.UseBackup)
                result.UseBackup(options.BackupPath);
            _configure?.Invoke(ctx, result);

            // УДАЛЕНИЕ ДОЛЖНО БЫТЬ ВСЕГДА В САМОМ КОНЦЕ!!!
            result.UseRemoveSourceFile();
            return result;
        }
    }
}
