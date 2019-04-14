using CFT.Application.Abstractions.Exceptions;
using CFT.MiddleWare.Base;
using Microsoft.Extensions.Logging;
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
    internal class ValidateByXSDMiddleWare : LogMiddlewareBase
    {
        XmlSchemaSet _schemas;
        ValidateByXSDOptions _option;

        public ValidateByXSDMiddleWare(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger<ValidateByXSDMiddleWare> logger,
            ValidateByXSDOptions options)
            : base(next, logger)
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

            _option = options;
        }

        protected override Task ExecAsync(CFTFileContext context)
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

            return Task.CompletedTask;
        }

        protected override string StartMessage => "Проверка по XSD схеме.";

        protected override string EndSuccessMessage => "Успешно проверили по XSD.";

        protected override string EndErrorMessage => "Ошибка при проверке XSD схемы.";

        protected override Task NextExceptionExecAsync(Exception e, CFTFileContext context)
        {
            return Task.CompletedTask;
        }
    }
}
