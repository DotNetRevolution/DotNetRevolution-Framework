using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class Transaction
    {
        public Identity Identity { [Pure] get; }

        public ICommand Command { [Pure] get; }
        
        public EventProvider EventProvider { [Pure] get; }
        
        public Transaction(ICommand command, EventProvider eventProvider)
        {
            Contract.Requires(command != null);
            Contract.Requires(eventProvider != null);

            Identity = Identity.New();

            Command = command;
            EventProvider = eventProvider;
        }
    }
}
