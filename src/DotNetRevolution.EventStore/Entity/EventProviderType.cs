using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class EventProviderType
    {
        public int EventProviderTypeId { get; }

        public string FullName { get; }

        [UsedImplicitly]
        private EventProviderType()
        {
        }

        public EventProviderType(string fullName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(fullName));

            FullName = fullName;
        }
    }
}