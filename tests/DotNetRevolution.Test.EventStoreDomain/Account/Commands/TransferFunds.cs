using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class TransferFunds : Command
    {
        public Guid FromAccountId { get; }

        public Guid ToAccountId { get; }

        public decimal Amount { get; }

        public TransferFunds(Guid commandId, Guid fromAccountId, Guid toAccountId, decimal amount)
            : base(commandId)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
        }
    }
}
