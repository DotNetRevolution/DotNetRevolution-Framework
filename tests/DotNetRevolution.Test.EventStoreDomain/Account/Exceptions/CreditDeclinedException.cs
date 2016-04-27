using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Exceptions
{
    [Serializable]
    public class CreditDeclinedException : Exception
    {
        public CreditDeclinedException()
        {
        }

        public CreditDeclinedException(string message) : base(message)
        {
        }

        public CreditDeclinedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CreditDeclinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
