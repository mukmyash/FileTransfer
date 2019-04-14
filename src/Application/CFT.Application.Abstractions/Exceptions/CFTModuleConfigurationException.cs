using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CFT.Application.Abstractions.Exceptions
{
    public class CFTModuleConfigurationException : CFTApplicationException
    {
        public CFTModuleConfigurationException(string moduleName)
            : this(moduleName, null)
        {
        }

        public CFTModuleConfigurationException(string moduleName, Exception innerException)
            : base($"Ошибка при конфигурации модуля: '{moduleName}'", innerException)
        {
        }
    }
}
