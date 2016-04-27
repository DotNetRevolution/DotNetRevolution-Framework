using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public interface ICommand
    {
        Guid Id { [Pure] get; }
    }
}
