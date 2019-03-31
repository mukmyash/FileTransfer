using cft.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FlowStep.Transformation
{
    internal class TransformFileNameStep : IFlowStep
    {
        readonly string _fileMask;

        public TransformFileNameStep(ITransformFileNameStepOptions options)
        {
            try
            {
                options.ValidationParams();
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля переименования файла.", e);
            }

            _fileMask = options.FileMask;
        }

        public Task RunAsync(FileContext context)
        {
            var newFileName = _fileMask;
            var allParams = GetFileNameParameters(context.FileInfo);

            foreach (var param in allParams)
            {
                // Если параметров больше нет то и дальше искать ни чего не надо.
                if (!newFileName.Contains("@{"))
                    break;
                newFileName = newFileName.Replace(param.Key, param.Value);
            }

            try
            {
                context.FileInfo.MoveTo(Path.Combine(context.FileInfo.Directory.FullName, newFileName));
            }
            catch (Exception e)
            {
                throw new CFTApplicationException("Ошибка при переименовании файла.", e);
            }

            return Task.CompletedTask;
        }

        private Dictionary<string, string> GetFileNameParameters(FileInfo fileInfo)
        {
            int paramNumber = 0;

            // Параметры из имени файла имеют формат: @{FP1}, @{FP2}, ....
            return fileInfo.Name
                .Split("_").SelectMany(n => n.Split("-"))
                .ToDictionary(v => $"@{{FP{++paramNumber}}}");
        }
    }
}
