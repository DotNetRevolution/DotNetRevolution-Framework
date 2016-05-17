using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamProcessor : IEventStreamProcessor
    {
        public TEventProvider Process<TEventProvider>(EventStream eventStream) 
            where TEventProvider : class
        {
            Contract.Requires(eventStream != null);

            throw new NotImplementedException();
        }
    }
}
