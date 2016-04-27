using System;
using System.Runtime.Serialization;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Exceptions
{
    [Serializable]
    public class DebitDeclinedException : Exception
    {
        public DebitDeclinedException()
        {
        }

        public DebitDeclinedException(string message) : base(message)
        {
        }

        public DebitDeclinedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DebitDeclinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
