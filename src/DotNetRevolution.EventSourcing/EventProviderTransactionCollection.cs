using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderTransactionCollection
    {
        public IEventProvider EventProvider { get; }

        public IReadOnlyCollection<EventProviderTransaction> Transactions { get; }

        public EventProviderTransactionCollection(IEventProvider eventProvider, IReadOnlyCollection<EventProviderTransaction> transactions)
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(transactions != null);

            EventProvider = eventProvider;
            Transactions = transactions;
        }
    }
}
