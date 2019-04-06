using CFT.FileProvider.Abstractions;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CFT.Hosting.Middleware
{
    internal class RemoveInputFileMiddleware : LogMiddlewareBase
    {
        ICFTFileProvider _fileProvider;

        public RemoveInputFileMiddleware(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger<RemoveInputFileMiddleware> logger,
            IOptions<FileScanerOptions> options,
            IFileProviderFactory fileProviderFactory)
            : base(next, logger)
        {
            _fileProvider = fileProviderFactory.GetFileProvider(options.Value.FileProviderType, options.Value.FileProviderSettings);
        }

        protected override string StartMessage => "Удаляем файл.";

        protected override string EndSuccessMessage => "Файл успешно удален.";

        protected override string EndErrorMessage => "Ошибка удаления файла.";

        protected override Task ExecAsync(CFTFileContext context)
        {
            var file = _fileProvider.GetFileInfo(context.InputFile.FullName);
            file.Delete();
            return Task.CompletedTask;
        }
    }
}
