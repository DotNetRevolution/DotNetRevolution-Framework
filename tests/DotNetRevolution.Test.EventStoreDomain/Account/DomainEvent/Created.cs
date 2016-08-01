using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents
{
    public class Created : IDomainEvent
    {
        public Guid AccountId { get; private set; }

        public decimal Balance { get; private set; }
        
        public Guid DomainEventId { get; private set; }

        public Created(Guid accountId, decimal balance)
        {
            DomainEventId = GuidGenerator.Default.Create();
            AccountId = accountId;
            Balance = balance;
        }

        [UsedImplicitly]
        private Created()
        {
        }
    }
}
