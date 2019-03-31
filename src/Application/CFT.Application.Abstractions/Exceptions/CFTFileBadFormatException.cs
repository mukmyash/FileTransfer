using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CFT.Application.Abstractions.Exceptions
{
    public class CFTFileBadFormatException : CFTFileException
    {
        public CFTFileBadFormatException(string message) : base(message)
        {
        }

        public CFTFileBadFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CFTFileBadFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
