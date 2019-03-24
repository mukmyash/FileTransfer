using cft.Application.Exceptions;
using cft.Application.FlowStep.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cft.Application.Options.FlowStep.Validation
{
    public class CheckDublicateByFileSystemStepOptions : ICheckDublicateByFileSystemStepOptions
    {
        public string PathStore { get; set; }

        public void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(PathStore))
                throw new CFTConfigurationException("Не указан путь до хранилиша обработанных файлов.");

            var dirInfo = new DirectoryInfo(PathStore);
            if (!dirInfo.Exists)
                throw new CFTDirectoryNotFoundException($"Каталог '{dirInfo.FullName}' не существует.");
        }
    }
}
