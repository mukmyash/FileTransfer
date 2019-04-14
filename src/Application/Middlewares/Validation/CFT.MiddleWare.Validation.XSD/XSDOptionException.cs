using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.MiddleWare.Validation.XSD
{
    public class XSDOptionException : Exception
    {
        public string OptionName { get; }

        public XSDOptionException(string optionName) : this(optionName, null)
        {
        }

        public XSDOptionException(string optionName, Exception innerException)
            : base($"Ошибка в параметре: '{optionName}'", innerException)
        {
            OptionName = optionName;
        }
    }
}
