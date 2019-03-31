using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CFT.Application.Abstractions.Exceptions
{
    public class CFTApplicationException : Exception
    {
        public CFTApplicationException()
        {
        }

        public CFTApplicationException(string message) : base(message)
        {
        }

        public CFTApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CFTApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
