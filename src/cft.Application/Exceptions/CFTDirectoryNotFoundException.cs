using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace cft.Application.Exceptions
{
    public class CFTDirectoryNotFoundException : CFTDirectoryException
    {
        public CFTDirectoryNotFoundException(string message) : base(message)
        {
        }

        public CFTDirectoryNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CFTDirectoryNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
