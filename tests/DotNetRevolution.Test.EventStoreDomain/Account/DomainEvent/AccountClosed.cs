using System;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvent
{
    public class AccountClosed : IDomainEvent
    {
        public Guid AccountId { get; }

        public Guid DomainEventId { get; }

        public AccountClosed(Guid accountId)
        {
            DomainEventId = GuidGenerator.Default.Create();
            AccountId = accountId;
        }
    }
}
