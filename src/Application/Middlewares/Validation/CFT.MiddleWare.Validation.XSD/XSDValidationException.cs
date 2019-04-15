using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD
{
    /// <summary>
    /// Ошибка валидации по XSD.
    /// </summary>
    public class XSDValidationException : Exception
    {
        public IEnumerable<string> SchemaErrors { get; }

        /// <summary>
        /// Создает экземпляр класса <see cref="XSDValidationException"/>
        /// </summary>
        /// <param name="schemaErrors">Список ошибок выявленных при проверке.</param>
        public XSDValidationException(IEnumerable<string> schemaErrors)
            : this(schemaErrors, null)
        {
            SchemaErrors = schemaErrors;
        }

        /// <summary>
        /// Создает экземпляр класса <see cref="XSDValidationException"/>
        /// </summary>
        /// <param name="schemaErrors">Список ошибок выявленных при проверке.</param>
        /// <param name="innerException">Внутреннее исключение.</param>
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
