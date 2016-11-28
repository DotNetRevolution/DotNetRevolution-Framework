using DotNetRevolution.Core.Commanding.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Withdraw2 : AggregateRootCommand<AccountAggregateRoot>
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public Withdraw2(Guid accountId, decimal amount)            
            : base(accountId)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
