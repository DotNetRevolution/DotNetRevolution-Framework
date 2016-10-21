using DotNetRevolution.Core.Commanding;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{    
    public class Create : AggregateRootCommand<AccountAggregateRoot>
    {
        public decimal BeginningBalance { get; }
        
        public Create(Guid commandId, Guid aggregateRootIdentity, decimal beginningBalance) 
            : base(commandId, aggregateRootIdentity)
        {
            BeginningBalance = beginningBalance;
        }
    }
}
