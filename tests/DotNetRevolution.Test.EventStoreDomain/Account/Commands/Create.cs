﻿using DotNetRevolution.Core.Commanding.Domain;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{    
    public class Create : AggregateRootCommand<AccountAggregateRoot>
    {
        public decimal BeginningBalance { get; }
        
        public Create(Guid aggregateRootIdentity, decimal beginningBalance) 
            : base(aggregateRootIdentity)
        {
            BeginningBalance = beginningBalance;
        }
    }
}
