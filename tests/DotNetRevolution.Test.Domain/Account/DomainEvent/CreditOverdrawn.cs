using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvent
{
    public class CreditOverdrawn
    {
        public Guid AccountId { get; }

        public decimal Balance { get; }

        public CreditOverdrawn(Guid accountId, decimal balance)
        {
            AccountId = accountId;
            Balance = balance;
        }
    }
}
