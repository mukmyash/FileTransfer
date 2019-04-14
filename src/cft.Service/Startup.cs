using CFT.Application;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace cft.Service
{
    public class Startup
    {
        public void Configure(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("Config/FileScannerConfig.json", optional: false, reloadOnChange: false);
        }

        public void ConfigureLoging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging
                .AddConfiguration(context.Configuration.GetSection("Logging"))
                .AddConsole();
        }
    }
}
