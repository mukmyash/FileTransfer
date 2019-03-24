using cft.Application;
using cft.Application.Extensions;
using cft.Service.FTPWorker;
using cft.Service.Options;
using FluentFTP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cft.Service.HostedService
{
    public class FTPListenerHostedService : BackgroundService
    {
        IServiceProvider _provider;

        int _scanPeriod = 0;

        public FTPListenerHostedService(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_scanPeriod, stoppingToken);

                using (var scopedProvider = _provider.CreateScope())
                {
                    var provider = scopedProvider.ServiceProvider;

                    FTPOptions options = provider.GetRequiredService<IOptionsSnapshot<FTPOptions>>().Value;
                    _scanPeriod = options.ScanPerion * 1000;

                    IFTPScaner ftpScaner = provider.GetRequiredService<IFTPScaner>();
                    IFlowBuilder flowBuilder = provider.GetRequiredService<IFlowBuilder>();

                    var flowRunner = flowBuilder
                        .RegisterFromConfig(options.Steps)
                        .Build();

                    await ftpScaner.Scan(file => flowRunner.Run(new FileContext(file)).GetAwaiter().GetResult());
                }
            }
        }
    }
}
