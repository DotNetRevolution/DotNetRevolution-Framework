using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvent
{
    public class DebitDeclined
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public decimal Balance { get; }

        public string Reason { get; }

        public DebitDeclined(Guid accountId, decimal amount, decimal balance, string reason)
        {
            AccountId = accountId;
            Amount = amount;
            Balance = balance;
            Reason = reason;
        }
    }
}
