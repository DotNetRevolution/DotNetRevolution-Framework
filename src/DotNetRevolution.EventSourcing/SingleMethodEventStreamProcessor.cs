using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class SingleMethodEventStreamProcessor : EventStreamProcessor
    {        
        private readonly string _methodName;

        public SingleMethodEventStreamProcessor(string methodName)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(methodName) == false);

            _methodName = methodName;
        }
        
        protected override string GetMethodName(string domainEventName)
        {
            return _methodName;
        }
    }
}
