using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD
{
    /// <summary>
    /// Ошибка опций.
    /// </summary>
    public class XSDOptionException : Exception
    {
        /// <summary>
        /// Наименрование опции в котором произошла ошибка.
        /// </summary>
        public string OptionName { get; }

        /// <summary>
        /// Создает экземпляр класса <see cref="XSDOptionException"/>
        /// </summary>
        /// <param name="optionName">Наименование опции.</param>
        public XSDOptionException(string optionName) : this(optionName, null)
        {
        }

        /// <summary>
        /// Создает экземпляр класса <see cref="XSDOptionException"/>
        /// </summary>
        /// <param name="optionName">Наименование опции.</param>
        /// <param name="innerException">Внутреннее исключение.</param>
        public XSDOptionException(string optionName, Exception innerException)
            : base($"Ошибка в параметре: '{optionName}'", innerException)
        {
            OptionName = optionName;
        }
    }
}
