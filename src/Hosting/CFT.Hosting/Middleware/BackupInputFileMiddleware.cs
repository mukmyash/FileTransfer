using CFT.Application.Abstractions.Exceptions;
using CFT.FileProvider.Abstractions;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CFT.Hosting.Middleware
{
    internal class BackupInputFileMiddleware : LogMiddlewareBase
    {
        readonly string _backupPath;

        public BackupInputFileMiddleware(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger<BackupInputFileMiddleware> logger,
            string backupPath)
            : base(next, logger)
        {
            if (string.IsNullOrWhiteSpace(backupPath))
                throw new ArgumentException("Не указан каталог для бэкапа.");
            _backupPath = backupPath;
        }

        protected override string StartMessage => $"Бэкапим файл ({_backupPath}).";

        protected override string EndSuccessMessage => $"Файл успешно помещен в хранилище ({_backupPath}).";

        protected override string EndErrorMessage => $"Ошибка помещения файла в хранилище ({_backupPath}).";

        protected override Task ExecAsync(CFTFileContext context)
        {
            if (!Directory.Exists(_backupPath))
                Directory.CreateDirectory(_backupPath);

            var filePath = Path.Combine(_backupPath, context.InputFile.FileName);
            if (File.Exists(filePath))
                throw new CFTDublicateFileException($"Файл '{filePath}' уже существует.");
            return File.WriteAllBytesAsync(filePath, context.InputFile.FileContent);
        }

        protected override Task NextExceptionExecAsync(Exception e, CFTFileContext context)
        {
            var filePath = Path.Combine(_backupPath, context.InputFile.FileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
            return Task.CompletedTask;
        }
    }
}
