﻿using System;
using System.Runtime.Serialization;

namespace Dev.Framework.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base()
        { }

        public NotFoundException(string message)
            : base(message)

        { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    } 
}
