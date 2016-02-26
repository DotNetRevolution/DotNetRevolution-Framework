using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions
{
    public class Session : ISession
    {
        public string Id { get; }

        protected IDictionary<string, object> InternalVariables { get; private set; }

        public IReadOnlyDictionary<string, object> Variables
        {
            get
            {
                return new ReadOnlyDictionary<string, object>(InternalVariables);
            }
        }

        public Session(string identity)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(identity));

            Id = identity;
            InternalVariables = new Dictionary<string, object>();
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(InternalVariables != null);
        }
    }
}
