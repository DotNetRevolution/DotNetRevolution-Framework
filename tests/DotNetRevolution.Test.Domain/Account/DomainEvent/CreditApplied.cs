using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvents
{
    public class CreditApplied : DomainEvent
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public decimal Balance { get; }

        public CreditApplied(Guid accountId, decimal amount, decimal balance)
        {
            AccountId = accountId;
            Amount = amount;
            Balance = balance;
        }
    }
}
