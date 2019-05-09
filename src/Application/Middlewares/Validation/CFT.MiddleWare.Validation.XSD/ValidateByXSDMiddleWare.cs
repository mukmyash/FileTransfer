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
    /// <summary>
    /// Мидлвара для проверки файла по XSD схеме.
    /// </summary>
    internal class ValidateByXSDMiddleWare : LogMiddlewareBase
    {
        XmlSchemaSet _schemas = new XmlSchemaSet();
        IValidateByXSDOptions _option;

        public ValidateByXSDMiddleWare(
            MiddlewareDelegate<CFTFileContext> next,
            ILogger<ValidateByXSDMiddleWare> logger,
            IValidateByXSDOptions options)
            : base(next, logger)
        {
            try
            {
                options.ValidationParams();

                try
                {
                    _schemas.Add(options.TargetNamespace, options.XSDPath);
                }
                catch (Exception e)
                {
                    throw new XSDFileFormatException(options.XSDPath, e);
                }
            }
            catch (Exception e)
            {
                throw new XSDModuleConfigurationException(e);
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
            // отключим проверку DTD т.к она не нужна. 
            // ЕСЛИ ВДРУГ КОГДА-ТО ПОНАДОБИТСЯ...
            // ТО пробросим в опции эту настройку.
            xmlSettings.DtdProcessing = DtdProcessing.Ignore;

            using (var readStream = new MemoryStream(context.OutputFile.FileContent))
            {
                using (var xmlReader = XmlReader.Create(readStream, xmlSettings))
                {
                    while (xmlReader.Read()) { }
                }
            }

            if (errors.Count > 0)
                throw new XSDValidationException(errors);

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
