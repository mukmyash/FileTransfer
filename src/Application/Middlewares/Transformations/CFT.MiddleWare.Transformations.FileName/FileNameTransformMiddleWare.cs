﻿using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Base;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter;
using CFT.MiddleWare.Transformations.FileName.ParametersExtracter.Options;
using Microsoft.Extensions.Logging;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFT.MiddleWare.Transformations.FileName
{
    internal class FileNameTransformMiddleWare : LogMiddlewareBase
    {
        FileNameTransformOptions _options;
        ParameterExtracterBase _parameterExtracterBase;

        public FileNameTransformMiddleWare(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger<FileNameTransformMiddleWare> logger,
            IParameterExtracterFactory parameterExtracterFactory,
            FileNameTransformOptions options)
            : base(next, logger)
        {
            try
            {
                options.ValidationParams();
                _parameterExtracterBase = parameterExtracterFactory.GetParameterExtracterFlow(options.ParametersDescription);
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля преобразования имени файла.", e);
            }

            _options = options;
        }

        protected override Task ExecAsync(CFTFileContext context)
        {
            try
            {
                var allParams = _parameterExtracterBase.Extract((ParameterContext)context);

                var newFileName = _options.FileMask;
                foreach (var param in allParams)
                {
                    if (!newFileName.Contains("@{"))
                        break;
                    newFileName = newFileName.Replace($"@{{{param.Key}}}", param.Value);
                }
                context.OutputFile.FileName = newFileName;
            }
            catch (Exception e)
            {
                throw new CFTApplicationException("Ошибка при переименовании файла.", e);
            }

            return Task.CompletedTask;
        }

        protected override string StartMessage => "Начинаем преобразование имени файла.";

        protected override string EndSuccessMessage => "Успешно преобразовали имя файла.";

        protected override string EndErrorMessage => "Ошибка преобразования имени файла.";

        protected override Task NextExceptionExecAsync(Exception e, CFTFileContext context)
        {
            return Task.CompletedTask;
        }

    }
}
