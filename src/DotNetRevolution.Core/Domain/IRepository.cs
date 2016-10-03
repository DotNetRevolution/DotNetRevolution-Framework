﻿using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(RepositoryContract<>))]
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        TAggregateRoot GetByIdentity(Identity identity);

        void Commit(TAggregateRoot aggregateRoot);
    }
}