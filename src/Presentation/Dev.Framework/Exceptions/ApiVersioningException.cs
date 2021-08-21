using System;
using System.Runtime.Serialization;

namespace Dev.Framework.Exceptions
{
    public class ApiVersioningException : Exception
    {
        public ApiVersioningException()
            : base()
        { }

        public ApiVersioningException(string message)
            : base(message)

        { }

        public ApiVersioningException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected ApiVersioningException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
