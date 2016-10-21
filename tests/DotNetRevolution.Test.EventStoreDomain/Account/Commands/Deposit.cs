using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Deposit : AggregateRootCommand<AccountAggregateRoot>
    {
        public decimal Amount { get; }

        public Deposit(Guid commandId, Guid accountId, decimal amount)
            : base(commandId, accountId)
        {
            Amount = amount;
        }
    }
}
