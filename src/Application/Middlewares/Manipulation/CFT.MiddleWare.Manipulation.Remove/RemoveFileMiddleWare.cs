using CFT.Application.Abstractions.Exceptions;
using CFT.FileProvider;
using CFT.FileProvider.Abstractions;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.Logging;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CFT.MiddleWare.Manipulation.Remove
{
    internal class RemoveFileMiddleWare : LogMiddlewareBase
    {
        RemoveFileOptions _option;
        ICFTFileProvider _fileProvider;

        public RemoveFileMiddleWare(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger<RemoveFileMiddleWare> logger,
            IFileProviderFactory fileProviderFactory,
            RemoveFileOptions options)
            : base(next, logger)
        {
            try
            {
                options.ValidationParams();
                _fileProvider = fileProviderFactory.GetFileProvider(options.FileProviderType, options.FileProviderSettings);
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля проверки по XSD схеме.", e);
            }

            _option = options;
        }

        protected override Task ExecAsync(CFTFileContext context)
        {
            var RemoveFile = GetFileInfo(context);
            if (!RemoveFile.Exists)
                throw new Exception("Файл существует.");

            RemoveFile.Delete();

            if (RemoveFile.Exists)
                throw new Exception("Ошибка при удалении файла.");

            return Task.CompletedTask;
        }

        private ICFTFileInfo GetFileInfo(CFTFileContext context)
        {
            string fullName;
            switch (_option.FileType)
            {
                case RemoveFileType.Input:
                    fullName = context.InputFile.FullName;
                    break;
                case RemoveFileType.Output:
                    fullName = context.OutputFile.FullName;
                    break;
                default:
                    throw new Exception("Не известный тип файла.");
            }

            return _fileProvider.GetFileInfo(fullName);
        }

        protected override string StartMessage => "Удаляем файл.";

        protected override string EndSuccessMessage => "Файл успешно удален.";

        protected override string EndErrorMessage => "Ошибка удаления файла.";

        protected override Task NextExceptionExecAsync(Exception e, CFTFileContext context)
        {
            return Task.CompletedTask;
        }
    }
}
