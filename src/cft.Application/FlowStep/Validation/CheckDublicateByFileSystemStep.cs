using cft.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cft.Application.FlowStep.Validation
{
    internal class CheckDublicateByFileSystemStep : IFlowStep
    {
        readonly string _pathStore;

        public CheckDublicateByFileSystemStep(ICheckDublicateByFileSystemStepOptions options)
        {
            try
            {
                options.ValidationParams();
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля проверки на дубли.", e);
            }

            _pathStore = options.PathStore;
        }

        public Task Run(FileContext context)
        {
            var storedFileName = Path.Combine(_pathStore, context.FileInfo.Name);

            if (File.Exists(storedFileName))
            {
                throw new CFTDublicateFileException($"Файл {context.FileInfo.Name} уже обработан.");
            }

            using (File.Create(storedFileName)) { }

            return Task.CompletedTask;
        }
    }
}
