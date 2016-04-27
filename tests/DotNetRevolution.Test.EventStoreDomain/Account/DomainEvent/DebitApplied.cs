using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents
{
    public class DebitApplied : DomainEvent
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public DebitApplied(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
