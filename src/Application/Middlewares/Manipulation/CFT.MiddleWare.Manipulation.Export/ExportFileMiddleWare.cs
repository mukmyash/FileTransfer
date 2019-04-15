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

namespace CFT.MiddleWare.Manipulation.Export
{
    internal class ExportFileMiddleWare : LogMiddlewareBase
    {
        ExportFileOptions _option;
        ICFTFileProvider _fileProvider;

        public ExportFileMiddleWare(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger<ExportFileMiddleWare> logger,
            IFileProviderFactory fileProviderFactory,
            ExportFileOptions options)
            : base(next, logger)
        {
            try
            {
                options.ValidationParams();
                _fileProvider = fileProviderFactory.GetFileProvider(options.FileProviderType, options.FileProviderSettings);
                var exportFolder = _fileProvider.GetDirectoryContents(options.Path);
                if (!exportFolder.Exists)
                    throw new ArgumentException($"Каталог '{options.Path}' не существует.", nameof(options.Path));
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля проверки по XSD схеме.", e);
            }

            _option = options;
        }

        protected override Task ExecAsync(CFTFileContext context)
        {
            var exportFile = GetFileInfo(context);
            if (exportFile.Exists)
                throw new Exception("Файл существует.");

            using (var wStream = exportFile.CreateWriteStream())
            {
                using (var bwStream = new BinaryWriter(wStream))
                {
                    bwStream.Write(GetFileContent(context));
                }
            }

            if (!exportFile.Exists)
                throw new Exception("Ошибка при создании файла.");

            if (_option.FileType == ExportFileType.Output)
                context.OutputFile.FullName = exportFile.PhysicalPath;

            return Task.CompletedTask;
        }

        protected override string StartMessage => $"Перемещаем файл. '{_option.Path}'";

        protected override string EndSuccessMessage => $"Файл успешно перемещен. '{_option.Path}'";

        protected override string EndErrorMessage => $"Ошибка перемещения файла. '{_option.Path}'";

        private byte[] GetFileContent(CFTFileContext context)
        {
            switch (_option.FileType)
            {
                case ExportFileType.Input:
                    return context.InputFile.FileContent;
                case ExportFileType.Output:
                    return context.OutputFile.FileContent;
                case ExportFileType.Exception:
                    return Encoding.Default.GetBytes(context.Error.ToString());
                default:
                    throw new Exception("Не известный тип файла.");
            }
        }

        private ICFTFileInfo GetFileInfo(CFTFileContext context)
        {
            string fileName;
            switch (_option.FileType)
            {
                case ExportFileType.Input:
                    fileName = context.InputFile.FileName;
                    break;
                case ExportFileType.Exception:
                    fileName = string.Concat(context.InputFile.FileName, "_Error");
                    break;
                case ExportFileType.Output:
                    fileName = context.OutputFile.FileName;
                    break;
                default:
                    throw new Exception("Не известный тип файла.");
            }

            return _fileProvider.GetFileInfo(string.Concat(_option.Path, "\\", fileName));
        }

        protected override Task NextExceptionExecAsync(Exception e, CFTFileContext context)
        {
            return Task.CompletedTask;
        }
    }
}
