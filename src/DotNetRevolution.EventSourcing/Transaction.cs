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

        public string User { [Pure] get; }
        
        public Transaction(string user, ICommand command, EventProvider eventProvider)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(user) == false);
            Contract.Requires(command != null);
            Contract.Requires(eventProvider != null);

            Identity = Identity.New();

            User = user;
            Command = command;
            EventProvider = eventProvider;
        }
    }
}
