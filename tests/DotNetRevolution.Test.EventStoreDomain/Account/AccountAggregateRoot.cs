using DotNetRevolution.Core.Domain;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvent;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using DotNetRevolution.Core.Reflection;
using System;
using System.Collections.Generic;

namespace DotNetRevolution.Test.EventStoreDomain.Account
{
    public class AccountAggregateRoot : AggregateRoot<AccountState>
    {        
        private static IMethodInvoker _methodInvoker = new NamedMethodInvoker<AccountAggregateRoot>("Execute");

        private AccountAggregateRoot(AggregateRootIdentity identity, AccountState state)
            : base(identity, state)
        {
            Contract.Requires(identity != null);
            Contract.Requires(state != null);
        }               
        
        private void Execute(Deposit command)
        {
            var newBalance = State.Balance + command.Amount;

            var domainEvents = new List<IDomainEvent>();
            domainEvents.Add(new CreditApplied(Guid.NewGuid(), Identity, command.Amount, newBalance));
            
            if (newBalance < 0)
            {
                domainEvents.Add(new Overdrawn(Guid.NewGuid(), Identity, newBalance));
            }

            State.Apply(domainEvents);
        }

        private void Execute(Withdraw command)
        {
            var newBalance = State.Balance - command.Amount;

            var domainEvents = new List<IDomainEvent>();
            domainEvents.Add(new DebitApplied(Guid.NewGuid(), Identity, command.Amount, newBalance));

            if (newBalance < 0)
            {
                domainEvents.Add(new Overdrawn(Guid.NewGuid(), Identity, newBalance));
            }

            State.Apply(domainEvents);
        }
        
        private void Execute(Withdraw2 command)
        {
            var newBalance = State.Balance - command.Amount;
            
            if (newBalance < 0)
            {
                State.Apply(new Overdrawn(Guid.NewGuid(), Identity, newBalance));
            }

            State.Apply(new DebitApplied(Guid.NewGuid(), Identity, command.Amount, newBalance));
        }

        public static DomainEventCollection Create(Create command)
        {
            var identity = new AggregateRootIdentity(command.AggregateRootId);

            var domainEvent = new Created(Guid.NewGuid(), identity, command.BeginningBalance);

            var state = new AccountState(domainEvent);

            var aggregateRoot = new AccountAggregateRoot(identity, state);

            return new DomainEventCollection(aggregateRoot, domainEvent);
        }
        
        public override void Execute(ICommand command)
        {
            _methodInvoker.InvokeMethodFor(this, command);
        }

        public override string ToString()
        {
            return $"{Identity.Value}: Balance: {State.Balance}";
        }
    }
}
