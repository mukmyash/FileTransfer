using cft.Application;
using FluentFTP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using CFT.Hosting;
using MiddleWare.Abstractions.Extensions;

namespace cft.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var startup = new Startup();

            await new CFTHostBuilder()
                .ConfigureApplication(startup.ConfigureApplication)
                .ConfigureServices(startup.ConfigureService)
                .ConfigureLogging(startup.ConfigureLoging)
                .ConfigureAppConfiguration(startup.Configure)
                .Build()
                .RunAsync();
        }
    }
}
