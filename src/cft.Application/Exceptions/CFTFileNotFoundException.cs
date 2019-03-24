using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace cft.Application.Exceptions
{
    public class CFTFileNotFoundException : CFTFileException
    {
        public CFTFileNotFoundException(string message) : base(message)
        {
        }

        public CFTFileNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CFTFileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
