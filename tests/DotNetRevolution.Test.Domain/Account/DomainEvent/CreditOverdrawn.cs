using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
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
            DomainEventId = SequentialGuid.Create();
            AccountId = accountId;
            Balance = balance;
        }

        [UsedImplicitly]
        private CreditOverdrawn()
        {
        }
    }
}
