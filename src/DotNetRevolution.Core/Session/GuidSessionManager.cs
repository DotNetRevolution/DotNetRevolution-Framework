using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Session
{
    public class GuidSessionManager : ISessionManager
    {
        private readonly ISession _currentSession;

        public IReadOnlyCollection<ISession> Sessions
        {
            get { return new ReadOnlyCollection<ISession>(new List<ISession> { _currentSession }); }
        }

        public GuidSessionManager()
        {
            _currentSession = new Session(Guid.NewGuid().ToString());

            SessionReleased += (sender, session) => { };
        }

        public ISession GetCurrentSession()
        {
            return _currentSession;
        }

        public event EventHandler<ISession> SessionReleased;

        public void Add(ISession session)
        {
            throw new NotSupportedException("GuidSessionManager does not support multiple sessions");
        }

        public void Remove(ISession session)
        {
            Contract.Assume(SessionReleased != null);

            SessionReleased(this, session);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_currentSession != null);
        }
    }
}