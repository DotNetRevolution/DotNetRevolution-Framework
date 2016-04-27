using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.Domain.Account.Commands
{
    public class Deposit : Command
    {
        public Guid AccountId { get; }

        public decimal Amount { get; }

        public Deposit(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
