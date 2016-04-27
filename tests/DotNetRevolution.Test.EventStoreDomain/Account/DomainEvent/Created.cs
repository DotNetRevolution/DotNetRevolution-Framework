using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents
{
    public class Created : DomainEvent
    {
        public Guid AccountId { get; }

        public decimal Balance { get; }

        public Created(Guid accountId, decimal balance)
        {
            AccountId = accountId;
            Balance = balance;
        }
    }
}
