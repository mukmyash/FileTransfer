using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CFT.Application.Abstractions.Exceptions
{
    public class CFTFileXSDValidationException : CFTFileException
    {
        public IEnumerable<string> SchemaErrors { get; }

        public CFTFileXSDValidationException(IEnumerable<string> schemaErrors) : base("Ошибка проверки файла по схеме XSD.")
        {
            SchemaErrors = schemaErrors;
        }

        public CFTFileXSDValidationException(IEnumerable<string> schemaErrors, Exception innerException) : base("Ошибка проверки файла по схеме XSD.", innerException)
        {
            SchemaErrors = schemaErrors;
        }

        protected CFTFileXSDValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var error in SchemaErrors)
            {
                result.AppendLine(error);
            }
            result.AppendLine(base.ToString());
            return result.ToString();
        }
    }
}
