using DotNetRevolution.Core.Sessions.CodeContract;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Sessions
{
    [ContractClass(typeof(SessionManagerContract))]
    public interface ISessionManager
    {        
        IReadOnlyCollection<ISession> Sessions { [Pure] get; }
                
        ISession this[string id] { [Pure] get; }

        void Add(ISession session);
        void Remove(ISession session);

        ICurrentSession Current { [Pure] get; }
        
        event EventHandler<SessionEventArgs> SessionRemoved;
    }
}
