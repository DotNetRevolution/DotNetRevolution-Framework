using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions
{
    public class SessionEventArgs : EventArgs
    {
        public ISession Session { get; }

        public SessionEventArgs(ISession session)
        {
            Contract.Requires(session != null);

            Session = session;
        }
    }
}
