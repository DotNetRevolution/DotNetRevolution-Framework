﻿using DotNetRevolution.Core.Session.CodeContract;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Session
{
    [ContractClass(typeof(SessionManagerContract))]
    public interface ISessionManager
    {
        IReadOnlyCollection<ISession> Sessions { get; }

        void Add(ISession session);
        void Remove(ISession session);

        ISession GetCurrentSession();
        
        event EventHandler<ISession> SessionReleased;
    }
}