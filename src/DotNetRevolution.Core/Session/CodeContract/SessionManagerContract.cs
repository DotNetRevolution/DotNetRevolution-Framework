using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Session.CodeContract
{
    [ContractClassFor(typeof(ISessionManager))]
    public abstract class SessionManagerContract : ISessionManager
    {
        public IReadOnlyCollection<ISession> Sessions
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<ISession>>() != null);

                throw new NotImplementedException();
            }
        }

        public event EventHandler<ISession> SessionReleased;

        public void Add(ISession session)
        {
            Contract.Requires(session != null);
        }

        public ISession GetCurrentSession()
        {
            Contract.Ensures(Contract.Result<ISession>() != null);

            throw new NotImplementedException();
        }

        public void Remove(ISession session)
        {
            Contract.Requires(session != null);
        }
    }
}
