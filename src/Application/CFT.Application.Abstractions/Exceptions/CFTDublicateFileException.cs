using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CFT.Application.Abstractions.Exceptions
{
    public class CFTDublicateFileException : CFTApplicationException
    {
        public CFTDublicateFileException(string message) : base(message)
        {
        }

        public CFTDublicateFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CFTDublicateFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
