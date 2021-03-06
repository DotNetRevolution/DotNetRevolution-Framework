﻿using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.DomainEvent
{
    public class DebitApplied : IDomainEvent
    {
        public Guid AccountId { get; private set; }

        public decimal Amount { get; private set; }

        public decimal NewBalance { get; private set; }

        public Guid DomainEventId { get; private set; }

        public DebitApplied(Guid domainEventId, Guid accountId, decimal amount, decimal newBalance)
        {
            DomainEventId = domainEventId;
            AccountId = accountId;
            Amount = amount;
            NewBalance = newBalance;
        }

        [UsedImplicitly]
        private DebitApplied()
        {
        }
    }
}
