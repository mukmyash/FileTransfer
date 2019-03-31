using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CFT.Application.Abstractions.Exceptions
{
    public class CFTConfigurationException : CFTApplicationException
    {
        public CFTConfigurationException(string message) : base(message)
        {
        }

        public CFTConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CFTConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
