using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Projections
{
    public class Account
    {
        public Guid Id { get; }

        public decimal Balance { get; set; }

        public Account(Guid id, decimal balance)
        {
            Id = id;
            Balance = balance;
        }
    }
}
