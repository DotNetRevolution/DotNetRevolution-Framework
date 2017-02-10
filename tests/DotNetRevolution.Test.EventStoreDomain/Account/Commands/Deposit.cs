using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Commanding.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Deposit : IAggregateRootCommand<AccountAggregateRoot>
    {
        public Guid CommandId { get; private set; }

        public Guid AggregateRootId { get; private set; }

        public decimal Amount { get; private set; }

        public Deposit(Guid accountId, decimal amount)
        {
            Contract.Requires(accountId != Guid.Empty);

            CommandId = Guid.NewGuid();
            AggregateRootId = accountId;
            Amount = amount;
        }
    }
}
