using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class EventProviderDescriptor
    {
        public int EventProviderDescriptorId { get; private set; }
        public Guid EventProviderId { get; private set; }
        public long TransactionId { get; private set; }
        public string Descriptor { get; private set; }

        public virtual EventProvider EventProvider { get; private set; }
        public virtual Transaction Transaction { get; private set; }

        [UsedImplicitly]
        private EventProviderDescriptor()
        {
        }

        public EventProviderDescriptor(EventProvider eventProvider, Transaction transaction, string descriptor)
        {
            Contract.Requires(eventProvider != null, "eventProvider");
            Contract.Requires(transaction != null, "transaction");
            Contract.Requires(!string.IsNullOrWhiteSpace(descriptor), "Descriptor");

            EventProvider = eventProvider;
            Transaction = transaction;
            Descriptor = descriptor;
        }
    }
}