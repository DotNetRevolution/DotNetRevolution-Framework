using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents
{
    public class CreditApplied : DomainEvent
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }
        
        public CreditApplied(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
