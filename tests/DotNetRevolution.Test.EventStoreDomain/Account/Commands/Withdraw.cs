using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Withdraw : Command
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public Withdraw(Guid commandId, Guid accountId, decimal amount)
            : base(commandId)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
