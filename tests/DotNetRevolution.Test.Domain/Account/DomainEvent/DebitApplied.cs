﻿using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Test.Domain.Account.DomainEvents
{
    public class DebitApplied : DomainEvent
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public decimal Balance { get; }

        public DebitApplied(Guid accountId, decimal amount, decimal balance)
        {
            AccountId = accountId;
            Amount = amount;
            Balance = balance;
        }
    }
}
