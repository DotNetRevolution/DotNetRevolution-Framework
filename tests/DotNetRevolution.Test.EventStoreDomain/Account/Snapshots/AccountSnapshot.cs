using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Snapshots
{
    public class AccountSnapshot
    {
        public Guid Id { [Pure] get; }

        public decimal Balance { [Pure] get; }

        public AccountSnapshot(Guid id, decimal balance)
        {
            Id = id;
            Balance = balance;
        }
    }
}
