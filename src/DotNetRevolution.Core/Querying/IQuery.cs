﻿using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Querying
{
    public interface IQuery<TResult>
        where TResult : class
    {
        Guid Id { [Pure] get; }
    }
}
