using CFT.Application.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD
{
    /// <summary>
    /// Ошибка конфигурации модуля проверки по XSD
    /// </summary>
    public class XSDModuleConfigurationException : CFTModuleConfigurationException
    {
        private const string MODULE_NAME = "Проверка по XSD схеме.";

        /// <summary>
        /// Создает экземпляр класса <see cref="XSDModuleConfigurationException"/>
        /// </summary>
        public XSDModuleConfigurationException()
            : base(MODULE_NAME)
        {
        }

        /// <summary>
        /// Создает экземпляр класса <see cref="XSDModuleConfigurationException"/>
        /// </summary>
        /// <param name="innerException">Внутреннее исключение.</param>
        public XSDModuleConfigurationException(Exception innerException)
            : base(MODULE_NAME, innerException)
        {
        }
    }
}
