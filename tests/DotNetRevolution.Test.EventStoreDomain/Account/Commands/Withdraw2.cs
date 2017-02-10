using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Commanding.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Withdraw2 : IAggregateRootCommand<AccountAggregateRoot>
    {
        public Guid CommandId { get; private set; }

        public Guid AggregateRootId
        {
            get
            {
                return AccountId;
            }
        }

        public Guid AccountId { get; private set; }

        public decimal Amount { get; private set; }

        public Withdraw2(Guid accountId, decimal amount)
        {
            CommandId = Guid.NewGuid();
            AccountId = accountId;
            Amount = amount;
        }
    }
}
