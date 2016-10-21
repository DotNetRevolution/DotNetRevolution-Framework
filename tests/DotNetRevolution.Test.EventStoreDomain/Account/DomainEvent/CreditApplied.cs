using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents
{
    public class CreditApplied : IDomainEvent
    {
        public Guid AccountId { get; private set; }

        public decimal Amount { get; private set; }

        public decimal NewBalance { get; private set; }

        public Guid DomainEventId { get; private set; }

        public CreditApplied(Guid domainEventId, Guid accountId, decimal amount, decimal newBalance)
        {
            DomainEventId = domainEventId;
            AccountId = accountId;
            Amount = amount;
            NewBalance = newBalance;
        }

        [UsedImplicitly]
        private CreditApplied()
        {
        }
    }
}
