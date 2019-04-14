using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD
{
    public class XSDValidationException : Exception
    {
        public IEnumerable<string> SchemaErrors { get; }

        public XSDValidationException(IEnumerable<string> schemaErrors)
            : this(schemaErrors, null)
        {
            SchemaErrors = schemaErrors;
        }

        public XSDValidationException(
            IEnumerable<string> schemaErrors,
            Exception innerException)
            : base(
                  message: $"Ошибка проверки файла по схеме XSD.{Environment.NewLine}{schemaErrors.Aggregate((i, j) => $"{i}{Environment.NewLine}{j}")}",
                  innerException: innerException)
        {
            SchemaErrors = schemaErrors;
        }
    }
}
