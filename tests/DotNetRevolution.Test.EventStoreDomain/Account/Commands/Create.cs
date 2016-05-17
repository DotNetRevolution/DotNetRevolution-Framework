using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class Create : Command
    {
        public decimal BeginningBalance { get; }

        public Create(decimal beginningBalance)
        {
            BeginningBalance = beginningBalance;
        }
    }
}
