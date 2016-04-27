using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvents
{
    public class CreditDeclined : DomainEvent
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public decimal Balance { get; }

        public string Reason { get; }

        public CreditDeclined(Guid accountId, decimal amount, decimal balance, string reason)
        {
            AccountId = accountId;
            Amount = amount;
            Balance = balance;
            Reason = reason;
        }
    }
}
