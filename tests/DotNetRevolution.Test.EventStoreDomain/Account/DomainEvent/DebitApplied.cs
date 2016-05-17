using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents
{
    public class DebitApplied : IDomainEvent
    {
        public Guid AccountId { get; private set; }

        public decimal Amount { get; private set; }

        public Guid DomainEventId { get; private set; }

        public DebitApplied(Guid accountId, decimal amount)
        {
            DomainEventId = SequentialGuid.Create();
            AccountId = accountId;
            Amount = amount;
        }

        [UsedImplicitly]
        private DebitApplied()
        {
        }
    }
}
