using System;

namespace DotNetRevolution.Test.Domain.Account.Command
{
    public class TransferFunds
    {
        public Guid FromAccountId { get; }

        public Guid ToAccountId { get; }

        public decimal Amount { get; }

        public TransferFunds(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
        }
    }
}
