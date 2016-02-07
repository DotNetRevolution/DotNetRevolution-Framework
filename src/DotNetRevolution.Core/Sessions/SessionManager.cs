using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Sessions
{
    public class SessionManager : ISessionManager
    {
        private readonly ISession _currentSession;
        private readonly List<ISession> _sessions;

        public IReadOnlyCollection<ISession> Sessions
        {
            get { return new ReadOnlyCollection<ISession>(_sessions); }
        }

        public ISession this[string identity]
        {
            get
            {
                return _sessions.FirstOrDefault(x => x.Identity == identity);
            }
        }

        public virtual ISession Current
        {
            get
            {
                return _currentSession;
            }
        }

        public SessionManager()
        {
            _sessions = new List<ISession>();
            SessionReleased += (sender, session) => { };
        }

        public SessionManager(ISession currentSession)
            : this()
        {
            Contract.Requires(currentSession != null);

            _currentSession = currentSession;
            _sessions.Add(currentSession);
        }

        public event EventHandler<SessionEventArgs> SessionReleased;

        public void Add(ISession session)
        {
            _sessions.Add(session);

            Contract.Assume(this[session.Identity] != null);
        }

        public void Remove(ISession session)
        {
            if (_sessions.Remove(_sessions.FirstOrDefault(x => x.Identity == session.Identity)))
            {
                SessionReleased(this, new SessionEventArgs(session));
            }

            Contract.Assume(this[session.Identity] == null);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(SessionReleased != null);
            Contract.Invariant(_sessions != null);
        }
    }
}
