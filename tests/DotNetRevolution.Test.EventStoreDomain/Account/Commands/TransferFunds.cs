using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class TransferFunds : Command
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
