using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvents
{
    public class CreditOverdrawn : DomainEvent
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
