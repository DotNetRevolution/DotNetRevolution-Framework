using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvent
{
    public class DebitOverdrawn
    {
        public Guid AccountId { get; }

        public decimal Balance { get; }

        public DebitOverdrawn(Guid accountId, decimal balance)
        {
            AccountId = accountId;
            Balance = balance;
        }
    }
}
