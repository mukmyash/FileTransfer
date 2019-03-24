using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace cft.Application.Exceptions
{
    public class CFTDirectoryException : CFTApplicationException
    {
        public CFTDirectoryException(string message) : base(message)
        {
        }

        public CFTDirectoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CFTDirectoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
