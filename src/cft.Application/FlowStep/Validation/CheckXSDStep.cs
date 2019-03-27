using cft.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace cft.Application.FlowStep.Validation
{
    internal class CheckXSDStep : IFlowStep
    {
        readonly string _xsdPath;
        readonly string _targetNamespace;

        public CheckXSDStep(ICheckXSDStepOptions options)
        {
            try
            {
                options.ValidationParams();
            }
            catch (Exception e)
            {
                throw new CFTConfigurationException("Ошибка при конфигурации модуля проверки по XSD схеме.", e);
            }

            _xsdPath = options.XSDPath;
            _targetNamespace = options.TargetNamespace;
        }

        public Task RunAsync(FileContext context)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(_targetNamespace, _xsdPath);
            XDocument custOrdDoc = XDocument.Load(context.FileInfo.FullName);

            List<string> errors = new List<string>();
            custOrdDoc.Validate(schemas, (o, e) =>
            {
                if (e.Severity == XmlSeverityType.Error)
                    errors.Add(e.Message);
            });

            if (errors.Count > 0)
                throw new CFTFileXSDValidationException(errors);

            return Task.CompletedTask;
        }
    }
}
