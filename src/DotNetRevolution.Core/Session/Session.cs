using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Session
{
    public class Session : ISession
    {
        public string Identity { get; private set; }

        public Dictionary<string, object> Variables { get; private set; }

        public Session(string identity)
        {
            Contract.Requires(identity != null);

            Identity = identity;
            Variables = new Dictionary<string, object>();
        }
    }
}