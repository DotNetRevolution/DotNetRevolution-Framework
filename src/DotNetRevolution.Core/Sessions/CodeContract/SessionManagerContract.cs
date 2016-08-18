using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions.CodeContract
{
    [ContractClassFor(typeof(ISessionManager))]
    internal abstract class SessionManagerContract : ISessionManager
    {
        public ISession this[string id]
        {
            get
            {
                Contract.Requires(id != null);

                throw new NotImplementedException();
            }
        }

        public ICurrentSession Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IReadOnlyCollection<ISession> Sessions
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<ISession>>() != null);

                throw new NotImplementedException();
            }
        }

        public event EventHandler<SessionEventArgs> SessionRemoved;

        public void Add(ISession session)
        {
            Contract.Requires(session != null);
            Contract.Ensures(this[session.Id] != null);
        }

        public void Remove(ISession session)
        {
            Contract.Requires(session != null);
            Contract.Ensures(this[session.Id] == null);
        }
    }
}
