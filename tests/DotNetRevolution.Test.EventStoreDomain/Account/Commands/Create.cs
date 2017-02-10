using DotNetRevolution.Core.Commanding.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{    
    public class Create : IAggregateRootCommand<AccountAggregateRoot>
    {
        public Guid CommandId { get; private set; }

        public Guid AggregateRootId { get; private set; }

        public decimal BeginningBalance { get; private set; }

        public Create(Guid aggregateRootId, decimal beginningBalance)             
        {
            Contract.Requires(aggregateRootId != Guid.Empty);

            CommandId = Guid.NewGuid();
            AggregateRootId = aggregateRootId;
            BeginningBalance = beginningBalance;
        }
    }
}
