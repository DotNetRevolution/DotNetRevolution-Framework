using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.AggregateRoot
{
    public class SingleMethodAggregateRootProcessor : AggregateRootProcessor
    {        
        private readonly string _methodName;

        public SingleMethodAggregateRootProcessor(string methodName)
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
