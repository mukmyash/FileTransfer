using cft.Application;
using cft.Service.FTPWorker;
using cft.Service.HostedService;
using cft.Service.Options;
using FluentFTP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace cft.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var startup = new Startup();

            await new HostBuilder()
                .ConfigureAppConfiguration(startup.Configure)
                 .ConfigureServices(startup.ConfigureService)
                 .Build()
                 .RunAsync();
        }
    }
}
