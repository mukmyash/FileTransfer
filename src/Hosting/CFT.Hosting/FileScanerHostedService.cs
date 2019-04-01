using CFT.FileProvider.Abstractions;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MiddleWare.Abstractions;
using SharpCifs.Smb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CFT.Hosting
{
    internal class FileScanerHostedService : BackgroundService
    {
        //string _uniqueName = Environment.MachineName + "_" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();

        Semaphore _semaphore = new Semaphore(5, 5);

        FileScanerOptions _options;
        ICFTFileProvider _fileProvider;
        IServiceProvider _applicationServices;
        ICFTMiddlewareBuilder _cftMiddlewareBuilder;

        public FileScanerHostedService(
            IOptions<FileScanerOptions> options,
            IFileProviderFactory fileProviderFactory,
            IServiceProvider applicationServices,
            ICFTMiddlewareBuilder cftMiddlewareBuilder)
        {
            if (fileProviderFactory == null)
                throw new ArgumentNullException(nameof(fileProviderFactory));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _applicationServices = applicationServices ?? throw new ArgumentNullException(nameof(applicationServices));
            _cftMiddlewareBuilder = cftMiddlewareBuilder ?? throw new ArgumentNullException(nameof(cftMiddlewareBuilder));
            _fileProvider = fileProviderFactory.GetFileProvider(options.Value.FileProviderType, options.Value.FileProviderSettings);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var application = _cftMiddlewareBuilder
                .Build();

            var changeToken = _fileProvider.Watch(_options.WatchPath);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_options.ScanPeriodSeconds * 1000, stoppingToken);

                if (!changeToken.HasChanged)
                    continue;
                try
                {
                    foreach (var fileInfo in _fileProvider.GetDirectoryContents(_options.WatchPath))
                    {
                        _semaphore.WaitOne();
                        var sourceName = fileInfo.Name;

                        //try
                        //{
                        // Защита от 2-го инстонса
                        // fileInfo.Rename($"{_uniqueName}_{fileInfo.Name}");

                        var context = new CFTFileContext(
                            _applicationServices,
                            new CFTFileInfo(
                                fileInfo.Name,
                                ReadFully(fileInfo.CreateReadStream()),
                                fileInfo.PhysicalPath));

                        ThreadPool.QueueUserWorkItem<CFTFileContext>(
                            ctx =>
                            {
                                application(ctx).GetAwaiter().GetResult();
                                _semaphore.Release();
                            },
                            context,
                           false);
                        //}
                        //catch (Exception e)
                        //{
                        //    // Защита от 2-го инстонса
                        //    //if (sourceName.Equals(fileInfo.Name))
                        //    //    fileInfo.Rename(sourceName);
                        //}

                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private byte[] ReadFully(Stream input)
        {
            using (input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }
    }
}
