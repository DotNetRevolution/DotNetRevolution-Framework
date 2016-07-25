﻿using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace DotNetRevolution.EventSourcing
{
    public class TypeNotFoundForHashException : Exception
    {
        public byte[] Hash { get; }

        public TypeNotFoundForHashException(byte[] hash)
        {
            Contract.Requires(hash != null);

            Hash = hash;   
        }

        private TypeNotFoundForHashException()
        {
        }

        private TypeNotFoundForHashException(string message) : base(message)
        {
        }

        private TypeNotFoundForHashException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeNotFoundForHashException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
    }
}
