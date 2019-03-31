using cft.Application.Exceptions;
using cft.Application.FlowStep.Transformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cft.Application.FlowStep.Transformation
{
    public class TransformXSLContentStepOptions : ITransformXSLContentStepOptions
    {
        public string XSLTPath { get; set; }

        public void ValidationParams()
        {
            if (string.IsNullOrWhiteSpace(XSLTPath))
            {
                throw new CFTConfigurationException("Не указан путь к файлу XSLT.");
            }

            var xsltFileInfo = new FileInfo(XSLTPath);
            if (xsltFileInfo.Extension != ".xslt" && xsltFileInfo.Extension != ".xsl")
            {
                throw new CFTFileBadFormatException($"Файл '{xsltFileInfo.FullName}' не является файлом XSLT.");
            }

            if (!xsltFileInfo.Exists)
                throw new CFTFileNotFoundException($"Файл '{xsltFileInfo.FullName}' не существует.");
        }
    }
}
