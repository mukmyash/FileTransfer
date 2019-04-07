using CFT.Application.Abstractions.Exceptions;
using System;
using System.IO;
using System.Xml.Schema;

namespace CFT.MiddleWare.Validation.XSD
{
    public class ValidateByXSDOptions
    {
        public string XSDPath { get; set; }

        public string TargetNamespace { get; set; }

        public void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(XSDPath))
                throw new CFTConfigurationException("Не указан путь до XSD файла.");

            var xsdFileInfo = new FileInfo(XSDPath);
            if (xsdFileInfo.Extension != ".xsd")
            {
                throw new CFTFileBadFormatException($"Файл '{xsdFileInfo.FullName}' не является файлом XSD.");
            }

            if (!xsdFileInfo.Exists)
                throw new CFTFileNotFoundException($"Файл '{xsdFileInfo.FullName}' не существует.");
        }
    }
}
