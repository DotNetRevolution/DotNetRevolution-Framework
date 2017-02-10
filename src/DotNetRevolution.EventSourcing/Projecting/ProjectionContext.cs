using System.Collections.Generic;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Metadata;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionContext : IProjectionContext
    {
        public TransactionIdentity TransactionIdentity { get; }

        public ICommand Command { get; }

        public IEventProvider EventProvider { get; }

        public IReadOnlyCollection<Meta> Metadata { get; }

        public EventProviderVersion Version { get; }

        public object Data { get; }

        public ProjectionContext(IEventProvider eventProvider, TransactionIdentity transactionIdentity, ICommand command, IReadOnlyCollection<Meta> metadata, EventProviderVersion version, object data)
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(transactionIdentity != null);
            Contract.Requires(command != null);
            Contract.Requires(metadata != null);
            Contract.Requires(version != null);
            Contract.Requires(data != null);

            EventProvider = eventProvider;
            TransactionIdentity = transactionIdentity;
            Command = command;
            Metadata = metadata;
            Version = version;
            Data = data;
        }

        protected ProjectionContext(IProjectionContext context)
            : this(context.EventProvider, context.TransactionIdentity, context.Command, context.Metadata, context.Version, context.Data)
        {
            Contract.Requires(context != null);
        }
    }
}
