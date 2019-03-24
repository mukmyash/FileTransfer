using cft.Application.Extensions;
using cft.Service.FTPWorker;
using cft.Service.HostedService;
using cft.Service.Options;
using FluentFTP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace cft.Service
{
    public class Startup
    {
        public void Configure(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("Config/DevelopmentConfig.json", optional: false, reloadOnChange: true);
        }

        public void ConfigureService(HostBuilderContext context, IServiceCollection services)
        {
            services.AddScoped<IFTPScaner, FTPScaner>();
            services.TryAddScoped<IFtpClient>(provider =>
            {
                var options = provider.GetRequiredService<IOptionsSnapshot<FTPOptions>>().Value;

                var client = new FtpClient();
                client.Host = options.HOST;
                if (options.Port.HasValue)
                    client.Port = options.Port.Value;
                return client;
            });

            services.AddScoped<IFTPClientFactory, FTPClientFactory>();
            services.AddHostedService<FTPListenerHostedService>();
            services.Configure<FTPOptions>(option => context.Configuration.GetSection("FTPHostedService").Bind(option));
            services.RegisterFlowBuilder();
        }
    }
}
