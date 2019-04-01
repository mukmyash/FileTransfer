using CFT.Hosting.Middleware;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddleWare.Abstractions.Extensions;
using System;
using System.Collections.Generic;
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
            Action<HostBuilderContext, IServiceCollection> additionConfigureDelegate = (ctx, services) =>
            {
                configureDelegate(ctx, services);
                services.AddLogging();
                services.AddHostedService<FileScanerHostedService>();
                services.AddSingleton<ICFTMiddlewareBuilder>(provider =>
                {
                    var result = new CFTMiddlewareBuilder(provider);
                    result.UseMiddleware<LogMiddleware, CFTFileContext>();
                    _configure?.Invoke(ctx, result);
                    return result;
                });
                services.AddSingleton<IFileProviderFactory, FileProviderFactory>();
                services.Configure<FileScanerOptions>(options => ctx.Configuration.GetSection("FileScannerHostedService").Bind(options));
            };
            base.ConfigureServices(additionConfigureDelegate);
            return this;
        }
    }
}
