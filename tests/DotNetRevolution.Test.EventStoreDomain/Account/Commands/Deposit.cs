using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Deposit : AggregateRootCommand<AccountAggregateRoot>
    {
        public decimal Amount { get; }

        public Deposit(Guid accountId, decimal amount)
            : base(accountId)
        {
            Amount = amount;
        }
    }
}
