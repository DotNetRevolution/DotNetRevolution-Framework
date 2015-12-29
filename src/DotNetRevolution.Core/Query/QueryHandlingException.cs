using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace DotNetRevolution.Core.Query
{
    [Serializable]
    public class QueryHandlingException : Exception
    {
        public object Query { get; private set; }

        public QueryHandlingException()
        {
        }

        public QueryHandlingException(string message)
            : base(message)
        {
        }

        public QueryHandlingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public QueryHandlingException(object query)
            : this(query, null, "Query was not handled correctly.")
        {
            Contract.Requires(query != null);
        }

        public QueryHandlingException(object query, string message)
            : this(query, null, message)
        {
            Contract.Requires(query != null);
        }

        public QueryHandlingException(object query, Exception innerException, string message)
            : base(message, innerException)
        {
            Contract.Requires(query != null);

            Query = query;
        }

        protected QueryHandlingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);

            Query = info.GetValue("Query", typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Query", Query);

            base.GetObjectData(info, context);
        }
    }
}