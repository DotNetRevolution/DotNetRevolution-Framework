using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvents
{
    public class DebitOverdrawn : DomainEvent
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
