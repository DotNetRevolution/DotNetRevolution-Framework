using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvents
{
    public class DebitApplied : IDomainEvent
    {
        public Guid AccountId { get; private set; }

        public decimal Amount { get; private set; }

        public decimal Balance { get; private set; }

        public Guid DomainEventId { get; private set; }

        public DebitApplied(Guid accountId, decimal amount, decimal balance)
        {
            DomainEventId = SequentialGuid.Create();
            AccountId = accountId;
            Amount = amount;
            Balance = balance;
        }

        [UsedImplicitly]
        private DebitApplied()
        {
        }
    }
}
