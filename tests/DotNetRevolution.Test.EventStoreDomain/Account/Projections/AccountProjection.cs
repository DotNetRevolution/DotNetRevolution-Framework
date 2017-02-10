using DotNetRevolution.EventSourcing.Projecting;
using System.Diagnostics.Contracts;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvent;
using System;
using System.Linq;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Projections
{
    public class AccountProjection : Projection,
        IProject<Created>,
        IProject<DebitApplied>,
        IProject<CreditApplied>
    {
        public void Project(IProjectionContext<Created> context)
        {
            //State.Accounts.Add(new Account(context.Data.AccountId, context.Data.Balance));
        }

        public void Project(IProjectionContext<CreditApplied> context)
        {
            //var account = State.Accounts.FirstOrDefault(x => x.Id == context.Data.AccountId);

            //if (account == null)
            //{
            //    throw new ApplicationException("account not found");
            //}

            //account.Balance = context.Data.NewBalance;
        }

        public void Project(IProjectionContext<DebitApplied> context)
        {
            //var account = State.Accounts.FirstOrDefault(x => x.Id == context.Data.AccountId);

            //if (account == null)
            //{
            //    throw new ApplicationException("account not found");
            //}

            //account.Balance = context.Data.NewBalance;
        }
    }
}
