using CFT.FileProvider;
using CFT.FileProvider.Abstractions;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.Options;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CFT.Hosting
{
    /// <summary>
    /// Класс запускающий обработку всех файлов в прослушиваемом каталоге.
    /// </summary>
    internal class CFTReadAllProcess : ICFTReadAllProcess
    {
        readonly SemaphoreSlim _semaphore;
        readonly ICFTFileProvider _fileProvider;
        readonly FileScanerOptions _options;
        readonly IServiceProvider _applicationServices;

        public CFTReadAllProcess(
           IOptions<FileScanerOptions> options,
           IFileProviderFactory fileProviderFactory,
           IServiceProvider applicationServices,
           ICFTMiddlewareBuilder cftMiddlewareBuilder)
        {
            if (fileProviderFactory == null)
                throw new ArgumentNullException(nameof(fileProviderFactory));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _applicationServices = applicationServices ?? throw new ArgumentNullException(nameof(applicationServices));
            _fileProvider = fileProviderFactory.GetFileProvider(options.Value.FileProviderType, options.Value.FileProviderSettings);
            _semaphore = new SemaphoreSlim(_options.NumberParallelFileWork, _options.NumberParallelFileWork);
        }

        /// <summary>
        /// Достает из прослушиваемого каталога все файлы и запускает их в обработку.
        /// </summary>
        /// <param name="applicationFlow">Процесс обработки одного файла.</param>
        /// <returns></returns>
        public async Task ProcessAllAsync(MiddlewareDelegate<CFTFileContext> applicationFlow)
        {
            foreach (var fileInfo in _fileProvider.GetDirectoryContents(_options.WatchPath))
            {
                await _semaphore.WaitAsync();

                ThreadPool.QueueUserWorkItem(
                    ctx =>
                    {
                        try
                        {
                            applicationFlow(ctx).GetAwaiter().GetResult();
                        }
                        finally
                        {
                            _semaphore.Release();
                        }
                    },
                    state: CreateContext(fileInfo),
                    preferLocal: false);

            }

            await WaitAllThread();
        }

        private async Task WaitAllThread()
        {
            // Ждем окончания выполнения всех операций.
            while (_semaphore.CurrentCount != _options.NumberParallelFileWork)
            {
                await Task.Delay(10);
            }
        }

        private CFTFileContext CreateContext(ICFTFileInfo fileInfo)
        {
            using (var stream = fileInfo.CreateReadStream())
            {
                return new CFTFileContext(
                    _applicationServices,
                    new CFTFileInfo(
                        fileInfo.Name,
                        ReadFully(stream),
                        fileInfo.PhysicalPath));
            }
        }

        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
