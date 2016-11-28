using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvent
{
    public class Created : IDomainEvent
    {
        public Guid AccountId { get; private set; }

        public decimal Balance { get; private set; }
        
        public Guid DomainEventId { get; private set; }

        public Created(Guid domainEventId, Guid accountId, decimal balance)
        {
            DomainEventId = domainEventId;
            AccountId = accountId;
            Balance = balance;
        }

        [UsedImplicitly]
        private Created()
        {
        }
    }
}
