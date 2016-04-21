using System;

namespace DotNetRevolution.Test.Domain.Account.Command
{
    public class Withdraw
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
