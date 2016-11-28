using DotNetRevolution.Core.Commanding.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Withdraw : AggregateRootCommand<AccountAggregateRoot>
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public Withdraw(Guid accountId, decimal amount)            
            : base(accountId)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
