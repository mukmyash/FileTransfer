using cft.Application.Extensions;
using CFT.MiddleWare.Base;
using FluentFTP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using MiddleWare.Abstractions;
using MiddleWare.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cft.Service
{
    public class Startup
    {
        public void Configure(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("Config/FileScannerConfig.json", optional: false, reloadOnChange: false);
        }

        public void ConfigureService(HostBuilderContext context, IServiceCollection services)
        {
        }

        public void Configure(HostBuilderContext context, ICFTMiddlewareBuilder services)
        {
        }
    }
}
