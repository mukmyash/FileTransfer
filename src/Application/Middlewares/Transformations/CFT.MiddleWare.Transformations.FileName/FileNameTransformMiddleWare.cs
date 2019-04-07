using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Base;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFT.MiddleWare.Transformations.FileName
{
    class FileNameTransformMiddleWare
    {
        FileNameTransformOptions _options;
        MiddlewareDelegate<CFTFileContext> _next;

        public FileNameTransformMiddleWare(
            FileNameTransformOptions options,
            MiddlewareDelegate<CFTFileContext> next)
        {
            try
            {
                options.ValidationParams();
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля преобразования имени файла.", e);
            }

            _options = options;
            _next = next;
        }

        public Task InvokeAsync(CFTFileContext context)
        {
            try
            {
                var allParams = GetFileNameParameters(context.InputFile.FileName);

                foreach (var param in allParams)
                {
                    if (!context.OutputFile.FileName.Contains("@{"))
                        break;
                    context.OutputFile.FileName = context.OutputFile.FileName.Replace(param.Key, param.Value);
                }

            }
            catch (Exception e)
            {
                throw new CFTApplicationException("Ошибка при переименовании файла.", e);
            }

            return _next(context);
        }

        private Dictionary<string, string> GetFileNameParameters(string fileName)
        {
            int paramNumber = 0;

            // Параметры из имени файла имеют формат: @{FP1}, @{FP2}, ....
            return fileName
                .Split("_").SelectMany(n => n.Split("-"))
                .ToDictionary(v => $"@{{FP{++paramNumber}}}");
        }

    }
}
