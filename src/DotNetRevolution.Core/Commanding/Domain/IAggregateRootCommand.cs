﻿using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Core.Commanding.Domain
{
    public interface IAggregateRootCommand<TAggregateRoot> : ICommand
        where TAggregateRoot : IAggregateRoot
    {
        Guid AggregateRootId { get; }
    }
}
