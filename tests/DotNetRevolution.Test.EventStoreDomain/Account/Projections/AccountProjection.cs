using DotNetRevolution.EventSourcing.Projecting;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvent;
using System;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Projections
{
    public class AccountProjection : Projection,
        IProject<Created>,
        IProject<DebitApplied>,
        IProject<CreditApplied>
    {
        private Account _state;

        public void Project(IProjectionContext<Created> context)
        {
            if (_state == null)
            {
                _state = new Account(context.Data.AccountId, context.Data.Balance);
            }
            else
            {
                throw new ApplicationException("Account already created");
            }
        }

        public void Project(IProjectionContext<CreditApplied> context)
        {
            if (_state == null)
            {
                throw new ApplicationException("Account has not been created");
            }

            _state.Balance = context.Data.NewBalance;
        }

        public void Project(IProjectionContext<DebitApplied> context)
        {
            if (_state == null)
            {
                throw new ApplicationException("Account has not been created");
            }

            _state.Balance = context.Data.NewBalance;
        }
    }
}
