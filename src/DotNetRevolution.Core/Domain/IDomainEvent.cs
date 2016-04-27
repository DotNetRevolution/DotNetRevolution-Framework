using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public interface IDomainEvent
    {
        Guid Id { [Pure] get; }
    }
}
