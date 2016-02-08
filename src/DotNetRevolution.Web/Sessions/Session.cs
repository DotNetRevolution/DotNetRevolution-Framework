using System.Collections.Generic;
using DotNetRevolution.Core.Sessions;
using System.Diagnostics.Contracts;
using System.Web.SessionState;

namespace DotNetRevolution.MVC.Sessions
{
    public class HttpSession : ISession
    {
        private readonly HttpSessionState _httpSession;

        public HttpSession(HttpSessionState httpSession)
        {
            Contract.Requires(httpSession != null);

            _httpSession = httpSession;
        }

        public string Identity
        {
            get
            {                
                return _httpSession.SessionID;
            }
        }

        public Dictionary<string, object> Variables
        {
            get
            {
                Contract.Assume(_httpSession.Keys != null);

                var dictionary = new Dictionary<string, object>();                
                
                foreach (var key in _httpSession.Keys)
                {
                    Contract.Assume(key != null);

                    var keyString = key.ToString();

                    dictionary[keyString] = _httpSession[keyString];
                }

                return dictionary;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_httpSession != null);
        }
    }
}
