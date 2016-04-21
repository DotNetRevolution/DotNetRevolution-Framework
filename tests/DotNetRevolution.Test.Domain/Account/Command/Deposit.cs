using System;

namespace DotNetRevolution.Test.Domain.Account.Command
{
    public class Deposit
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
