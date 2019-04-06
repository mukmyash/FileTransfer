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
    internal class CFTReadAllProcess : ICFTReadAllProcess
    {
        readonly Semaphore _semaphore = new Semaphore(5, 5);

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
        }

        public async Task ProcessAllAsync(MiddlewareDelegate<CFTFileContext> applicationFlow)
        {
            foreach (var fileInfo in _fileProvider.GetDirectoryContents(_options.WatchPath))
            {
                _semaphore.WaitOne();

                CFTFileContext context;
                using (var stream = fileInfo.CreateReadStream())
                {
                    context = new CFTFileContext(
                        _applicationServices,
                        new CFTFileInfo(
                            fileInfo.Name,
                            ReadFully(stream),
                            fileInfo.PhysicalPath));
                }

                ThreadPool.QueueUserWorkItem<CFTFileContext>(
                    ctx =>
                    {
                        applicationFlow(ctx).GetAwaiter().GetResult();
                        // fileInfo.Delete();
                        _semaphore.Release();
                    },
                    context,
                   false);
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
