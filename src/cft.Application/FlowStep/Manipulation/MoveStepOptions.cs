using System;
using System.Collections.Generic;
using System.Text;
using cft.Application.Exceptions;
using Microsoft.Extensions.Configuration;

namespace cft.Application.FlowStep.Manipulation
{
    public class MoveStepOptions : IMoveStepOptions
    {
        public string FileProvider { get; set; }
        public string Path { get; set; }

        public IConfigurationSection Settings { get; set; }

        public void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(Path))
                throw new CFTConfigurationException("Не указан путь для сохранения файла.");

            if (string.IsNullOrWhiteSpace(FileProvider))
                throw new CFTConfigurationException("Не указан провайдер файлов.");
        }
    }
}
