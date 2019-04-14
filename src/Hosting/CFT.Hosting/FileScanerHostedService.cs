using CFT.Application.Abstractions.Exceptions;
using CFT.FileProvider;
using CFT.FileProvider.Abstractions;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CFT.Hosting
{
    internal class FileScanerHostedService : BackgroundService
    {
        //string _uniqueName = Environment.MachineName + "_" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();

        FileScanerOptions _options;
        ICFTFileProvider _fileProvider;
        ICFTMiddlewareBuilder _cftMiddlewareBuilder;
        ICFTReadAllProcess _readAllProcess;

        public FileScanerHostedService(
            ICFTReadAllProcess readAllProcess,
            IOptions<FileScanerOptions> options,
            IFileProviderFactory fileProviderFactory,
            ICFTMiddlewareBuilder cftMiddlewareBuilder)
        {
            if (fileProviderFactory == null)
                throw new ArgumentNullException(nameof(fileProviderFactory));
            _readAllProcess = readAllProcess ?? throw new ArgumentNullException(nameof(readAllProcess));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _cftMiddlewareBuilder = cftMiddlewareBuilder ?? throw new ArgumentNullException(nameof(cftMiddlewareBuilder));
            _fileProvider = fileProviderFactory.GetFileProvider(options.Value.FileProviderType, options.Value.FileProviderSettings);

            try
            {
                options.Value.ValidateOptions();
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка конфигурации службы.", e);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var applicationFlow = _cftMiddlewareBuilder
                .Build();

            var changeToken = _fileProvider.Watch(_options.WatchPath);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_options.ScanPeriodSeconds * 1000, stoppingToken);

                if (!changeToken.HasChanged)
                    continue;

                try
                {
                    await _readAllProcess.ProcessAllAsync(applicationFlow);
                }
                catch (Exception)
                {
                    // Ни чего делать не надо. Там ниже ошибка залогируется. 
                }
            }
        }
    }
}
