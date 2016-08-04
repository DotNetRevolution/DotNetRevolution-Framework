﻿using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using DotNetRevolution.EventSourcing.Snapshotting;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventProviderContract))]
    public interface IEventProvider
    {
        Identity GlobalIdentity { [Pure] get; }
        EventProviderDescriptor Descriptor { [Pure] get; }
        EventStream DomainEvents { [Pure] get; }
        EventProviderType EventProviderType { [Pure] get; }
        Identity Identity { [Pure] get; }
        EventProviderVersion Version { [Pure] get; }

        Snapshot GetSnapshot();
    }
}