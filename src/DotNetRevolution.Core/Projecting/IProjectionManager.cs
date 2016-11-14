﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    [ContractClass(typeof(ProjectionManagerContract))]
    public interface IProjectionManager
    {
        void Project(IEnumerable<IDomainEvent> domainEvents);

        void Wait(Guid domainEventId);
        void Wait(Guid domainEventId, TimeSpan timeout);
        Task WaitAsync(Guid domainEventId);
        Task WaitAsync(Guid domainEventId, TimeSpan timeout);
    }
}