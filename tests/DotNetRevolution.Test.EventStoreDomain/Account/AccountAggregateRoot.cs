using DotNetRevolution.Core.Domain;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using DotNetRevolution.Core.Reflection;

namespace DotNetRevolution.Test.EventStoreDomain.Account
{
    public class AccountAggregateRoot : AggregateRoot<AccountState>
    {        
        private static IMethodInvoker _methodInvoker = new NamedMethodInvoker<AccountAggregateRoot>("Execute");

        private AccountAggregateRoot(Identity identity, AccountState state)
            : base(identity, state)
        {
            Contract.Requires(identity != null);
            Contract.Requires(state != null);
        }               
        
        private void Execute(Deposit command)
        {
            State.Apply(new CreditApplied(Identity, command.Amount, State.Balance + command.Amount));
        }

        private void Execute(Withdraw command)
        {
            State.Apply(new CreditApplied(Identity, command.Amount, State.Balance - command.Amount));
        }
        
        public static DomainEventCollection Create(Create command)
        {
            var identity = new Identity(command.AggregateRootId);

            var domainEvent = new Created(identity, command.BeginningBalance);

            var state = new AccountState(domainEvent);

            var aggregateRoot = new AccountAggregateRoot(identity, state);

            return new DomainEventCollection(aggregateRoot, domainEvent);
        }
        
        public override void Execute(ICommand command)
        {
            _methodInvoker.InvokeMethodFor(this, command);
        }
    }
}
