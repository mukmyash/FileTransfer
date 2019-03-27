using cft.Application.Exceptions;
using cft.Application.FlowStep.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace cft.Application.Options.FlowStep.Validation
{
    public class CheckXSDStepOptions : ICheckXSDStepOptions
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

            try
            {
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(TargetNamespace, XSDPath);
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при загрузке XSD документа.",e);
            }
        }
    }
}
