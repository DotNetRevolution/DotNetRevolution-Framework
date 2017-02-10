using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventStream))]
    internal abstract class EventStreamContract : IEventStream
    {
        public IEventProvider EventProvider
        {
            get
            {
                Contract.Ensures(Contract.Result<IEventProvider>() != null);

                throw new NotImplementedException();
            }
        }

        public EventProviderVersion LatestVersion
        {
            get
            {
                Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

                throw new NotImplementedException();
            }
        }

        public void Append(EventStreamRevision revision)
        {
            Contract.Requires(revision != null);

            throw new NotImplementedException();
        }
        
        public abstract IEnumerator<EventStreamRevision> GetEnumerator();

        public EventProviderVersion GetNextVersion()
        {
            Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
