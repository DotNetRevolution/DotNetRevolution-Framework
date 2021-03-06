﻿using DotNetRevolution.Core.Extension;
using DotNetRevolution.Core.Sessions;
using System.Diagnostics.Contracts;
using System.Web.SessionState;

namespace DotNetRevolution.Web.Sessions
{
    public class HttpCurrentSession : HttpSession, ICurrentSession
    {
        public HttpCurrentSession(HttpSessionState httpSession)
            : base(httpSession)
        {
            Contract.Requires(httpSession != null);
        }

        public void SetVariable(string key, object variable)
        {
            InternalHttpSession[key] = variable;
            Contract.Assume(Variables.PureContainsKey(key));
        }

        public void RemoveVariable(string key)
        {
            InternalHttpSession.Remove(key);
            Contract.Assume(Variables.PureContainsKey(key) == false);
        }
    }
}
