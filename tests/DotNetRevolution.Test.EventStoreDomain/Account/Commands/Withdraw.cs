using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Withdraw : Command
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public Withdraw(Guid accountId, decimal amount)            
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
