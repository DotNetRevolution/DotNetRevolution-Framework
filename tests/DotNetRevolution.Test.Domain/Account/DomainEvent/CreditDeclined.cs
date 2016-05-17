using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvents
{
    public class CreditDeclined : IDomainEvent
    {
        public Guid AccountId { get; private set; }

        public decimal Amount { get; private set; }

        public decimal Balance { get; private set; }

        public string Reason { get; private set; }

        public Guid DomainEventId { get; private set; }

        public CreditDeclined(Guid accountId, decimal amount, decimal balance, string reason)
        {
            DomainEventId = SequentialGuid.Create();
            AccountId = accountId;
            Amount = amount;
            Balance = balance;
            Reason = reason;
        }

        [UsedImplicitly]
        private CreditDeclined()
        {
        }
    }
}
