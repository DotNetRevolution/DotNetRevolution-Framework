using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;

namespace DotNetRevolution.EventStore.Entity
{
    public class EventProviderType
    {
        public int EventProviderTypeId { get; private set; }

        public string FullName { get; private set; }

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