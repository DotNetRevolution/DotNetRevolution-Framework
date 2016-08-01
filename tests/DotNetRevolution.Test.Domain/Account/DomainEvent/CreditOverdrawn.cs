using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvents
{
    public class CreditOverdrawn : IDomainEvent
    {
        public Guid AccountId { get; private set; }

        public decimal Balance { get; private set; }

        public Guid DomainEventId { get; private set; }

        public CreditOverdrawn(Guid accountId, decimal balance)
        {
            DomainEventId = GuidGenerator.Default.Create();
            AccountId = accountId;
            Balance = balance;
        }

        [UsedImplicitly]
        private CreditOverdrawn()
        {
        }
    }
}
