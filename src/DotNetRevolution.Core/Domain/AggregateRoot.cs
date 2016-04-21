namespace DotNetRevolution.Core.Domain
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        public abstract Identity Identity { get; }
                
        
        //public void ClearUncommittedEvents()
        //{
        //    _uncommittedEvents.Clear();
        //}

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = ".Net event model does not work for domain events.")]
        //protected virtual void RaiseDomainEvent(object domainEvent)
        //{
        //    Contract.Requires(domainEvent != null);

        //    _uncommittedEvents.Add(domainEvent);
        //}

        //[ContractInvariantMethod]
        //private void ObjectInvariant()
        //{
        //    Contract.Invariant(_uncommittedEvents != null);
        //}
    }
}
