using CFT.Application.Abstractions.Exceptions;
using System;
using System.IO;
using System.Xml.Schema;

namespace CFT.MiddleWare.Validation.XSD
{
    /// <summary>
    /// Настроки для мидлвары валидации по XSD схеме.
    /// </summary>
    public class ValidateByXSDOptions : IValidateByXSDOptions
    {
        /// <summary>
        /// Путь к XSD схеме.
        /// </summary>
        public string XSDPath { get; set; }

        /// <summary>
        /// TargetNamespace в XSD схеме. Почти всегда null. 
        /// </summary>
        public string TargetNamespace { get; set; } 

        /// <summary>
        /// Проверка всех настроек.
        /// </summary>
        public void ValidationParams()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(XSDPath))
                    throw new ArgumentException("Не указан путь до XSD.");

                var xsdFileInfo = new FileInfo(XSDPath);
                if (xsdFileInfo.Extension != ".xsd")
                {
                    throw new XSDFileFormatException(xsdFileInfo.FullName);
                }

                if (!xsdFileInfo.Exists)
                    throw new XSDFileNotFoundException(xsdFileInfo.FullName);
            }
            catch (Exception e)
            {
                throw new XSDOptionException(nameof(XSDPath), e);
            }
        }
    }
}
