using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Base;
using MiddleWare.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace CFT.MiddleWare.Validation.XSD
{
    public class ValidateByXSDMiddleWare
    {
        XmlSchemaSet _schemas;
        MiddlewareDelegate<CFTFileContext> _next;

        ValidateByXSDOptions _option;
        public ValidateByXSDMiddleWare(
            ValidateByXSDOptions options,
            MiddlewareDelegate<CFTFileContext> next)
        {
            try
            {
                options.ValidationParams();

                _schemas = new XmlSchemaSet();
                _schemas.Add(options.TargetNamespace, options.XSDPath);
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля проверки по XSD схеме.", e);
            }

            _next = next;
            _option = options;
        }

        public Task InvokeAsync(CFTFileContext context)
        {
            List<string> errors = new List<string>();

            XmlReaderSettings xmlSettings = new XmlReaderSettings();
            xmlSettings.Schemas.Add(_schemas);
            xmlSettings.ValidationType = ValidationType.Schema;
            xmlSettings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            xmlSettings.ValidationEventHandler += (o, e) =>
            {
                errors.Add(e.Message);
            };
            xmlSettings.IgnoreComments = true;
            xmlSettings.IgnoreWhitespace = true;

            using (var readStream = new MemoryStream(context.OutputFile.FileContent))
            {
                using (var xmlReader = XmlReader.Create(readStream, xmlSettings))
                {
                    while (xmlReader.Read()) { }
                }
            }

            if (errors.Count > 0)
                throw new CFTFileXSDValidationException(errors);

            return _next(context);
        }
    }
}
