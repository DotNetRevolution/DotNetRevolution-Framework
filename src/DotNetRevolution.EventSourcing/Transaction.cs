using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class Transaction
    {
        private readonly Collection<EventProvider> _eventProviders;

        public ICommand Command { [Pure] get; }
        
        public EntityCollection<EventProvider> EventProviders
        {
            [Pure]
            get
            {
                return _eventProviders as EntityCollection<EventProvider>;
            }
        }

        public string User { [Pure] get; }
        
        public Transaction(string user, ICommand command, params EventProvider[] eventProviders)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(user) == false);
            Contract.Requires(command != null);
            Contract.Requires(eventProviders != null);

            User = user;
            Command = command;
            _eventProviders = new EntityCollection<EventProvider>(eventProviders);
        }

        public void AddEventProvider(EventProvider eventProvider)
        {
            Contract.Requires(eventProvider != null);
            
            _eventProviders.Add(eventProvider);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_eventProviders != null);
        }
    }
}
