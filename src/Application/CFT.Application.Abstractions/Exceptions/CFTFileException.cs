using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CFT.Application.Abstractions.Exceptions
{
    public class CFTFileException : CFTApplicationException
    {
        public CFTFileException(string message) : base(message)
        {
        }

        public CFTFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CFTFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
