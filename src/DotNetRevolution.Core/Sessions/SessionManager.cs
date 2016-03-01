using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Sessions
{
    public class SessionManager : ISessionManager
    {
        private readonly ICurrentSession _currentSession;
        private readonly List<ISession> _sessions;

        public IReadOnlyCollection<ISession> Sessions
        {
            get { return _sessions.AsReadOnly(); }
        }

        public ISession this[string id]
        {
            get
            {
                return _sessions.FirstOrDefault(x => x.Id == id);
            }
        }

        public virtual ICurrentSession Current
        {
            get
            {
                return _currentSession;
            }
        }

        protected SessionManager()
        {
            _sessions = new List<ISession>();
            SessionRemoved += (sender, session) => { };
        }

        public SessionManager(ICurrentSession currentSession)
            : this()
        {
            Contract.Requires(currentSession != null);

            _currentSession = currentSession;
            _sessions.Add(currentSession);
        }

        public event EventHandler<SessionEventArgs> SessionRemoved;

        public void Add(ISession session)
        {
            _sessions.Add(session);

            Contract.Assume(this[session.Id] != null);
        }

        public void Remove(ISession session)
        {
            if (_sessions.Remove(_sessions.FirstOrDefault(x => x.Id == session.Id)))
            {
                SessionRemoved(this, new SessionEventArgs(session));
            }

            Contract.Assume(this[session.Id] == null);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(SessionRemoved != null);
            Contract.Invariant(_sessions != null);
        }
    }
}
