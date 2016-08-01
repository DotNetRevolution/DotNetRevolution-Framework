using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents
{
    public class DebitApplied : IDomainEvent
    {
        public Guid AccountId { get; private set; }

        public decimal Amount { get; private set; }

        public decimal NewBalance { get; private set; }

        public Guid DomainEventId { get; private set; }

        public DebitApplied(Guid accountId, decimal amount, decimal newBalance)
        {
            DomainEventId = GuidGenerator.Default.Create();
            AccountId = accountId;
            Amount = amount;
            NewBalance = newBalance;
        }

        [UsedImplicitly]
        private DebitApplied()
        {
        }
    }
}
