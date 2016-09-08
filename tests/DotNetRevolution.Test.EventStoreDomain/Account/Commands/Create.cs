using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{    
    public class Create : AggregateRootCommand
    {
        public decimal BeginningBalance { get; }
        
        public Create(Guid aggregateRootIdentity, decimal beginningBalance) 
            : base(aggregateRootIdentity)
        {
            BeginningBalance = beginningBalance;
        }
    }
}
