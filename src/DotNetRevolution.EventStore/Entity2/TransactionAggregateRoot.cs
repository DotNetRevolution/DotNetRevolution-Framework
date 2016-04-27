using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventStore.Entity2
{
    public class EventProvider
    {
        private readonly Identity _identity;

        public Identity Identity
        {
            get
            {
                return _identity;
            }
        }

        public EventProvider(Identity identity)
        {
            _identity = identity;
        }
    }
}
