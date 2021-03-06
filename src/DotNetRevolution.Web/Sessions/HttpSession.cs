﻿using System.Collections.Generic;
using DotNetRevolution.Core.Sessions;
using System.Diagnostics.Contracts;
using System.Web.SessionState;
using DotNetRevolution.Core.Extension;

namespace DotNetRevolution.Web.Sessions
{
    public class HttpSession : ISession
    {
        protected HttpSessionState InternalHttpSession { get; private set; }
        
        public HttpSession(HttpSessionState httpSession)
        {
            Contract.Requires(httpSession != null);

            InternalHttpSession = httpSession;
        }

        public string Id
        {
            get
            {
                var id = InternalHttpSession.SessionID;
                Contract.Assume(string.IsNullOrWhiteSpace(id) == false);

                return InternalHttpSession.SessionID;
            }
        }

        public IReadOnlyDictionary<string, object> Variables
        {
            get
            {
                Contract.Assume(InternalHttpSession.Keys != null);

                var dictionary = new Dictionary<string, object>();
                
                foreach (var key in InternalHttpSession.Keys)
                {
                    Contract.Assume(key != null);

                    var keyString = key.ToString();

                    dictionary[keyString] = InternalHttpSession[keyString];
                }

                var readonlyDictionary = dictionary.AsReadOnly();
                Contract.Assume(readonlyDictionary != null);

                return readonlyDictionary;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(InternalHttpSession != null);
        }
    }
}
