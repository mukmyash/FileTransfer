using CFT.Application.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD
{
    public class XSDModuleConfigurationException : CFTModuleConfigurationException
    {
        private const string MODULE_NAME = "Проверка по XSD схеме.";
        public XSDModuleConfigurationException()
            : base(MODULE_NAME)
        {
        }

        public XSDModuleConfigurationException(Exception innerException)
            : base(MODULE_NAME, innerException)
        {
        }
    }
}
