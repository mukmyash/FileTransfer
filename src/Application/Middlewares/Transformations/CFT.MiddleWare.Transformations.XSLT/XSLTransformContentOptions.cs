using CFT.Application.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CFT.MiddleWare.Transformations.XSLT
{
    internal class XSLTransformContentOptions
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
