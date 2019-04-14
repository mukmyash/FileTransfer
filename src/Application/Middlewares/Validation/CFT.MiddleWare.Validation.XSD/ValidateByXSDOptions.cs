using CFT.Application.Abstractions.Exceptions;
using System;
using System.IO;
using System.Xml.Schema;

namespace CFT.MiddleWare.Validation.XSD
{
    public class ValidateByXSDOptions : IValidateByXSDOptions
    {
        public string XSDPath { get; set; }

        public string TargetNamespace { get; set; }

        public void ValidationParams()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(XSDPath))
                    throw new ArgumentNullException("Не указан путь до XSD.");

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
