using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class EventProviderDescriptor
    {
        public int EventProviderDescriptorId { get; }
        public Guid EventProviderId { get; }
        public long TransactionId { get; }
        public string Descriptor { get; }

        public virtual EventProvider EventProvider { get; }
        public virtual Transaction Transaction { get; }

        [UsedImplicitly]
        private EventProviderDescriptor()
        {
        }

        internal EventProviderDescriptor(EventProvider eventProvider, Transaction transaction, string descriptor)
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(transaction != null);

            EventProvider = eventProvider;
            Transaction = transaction;
            Descriptor = string.IsNullOrWhiteSpace(descriptor) ? "__UNKNOWN__" : descriptor;
        }
    }
}