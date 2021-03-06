﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

#pragma warning disable CS1591
namespace HaruGaKita.Application.Exceptions
{
    public class UnauthenticatedException : Exception
    {
        public UnauthenticatedException()
        {
        }

        public UnauthenticatedException(string message) : base(message)
        {
        }

        public UnauthenticatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthenticatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
