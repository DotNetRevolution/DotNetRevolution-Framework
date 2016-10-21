using System;
using DotNetRevolution.Core.Commanding;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class Command1 : Command
    {
        public Command1(Guid commandId) 
            : base(commandId)
        {
            Contract.Requires(commandId != Guid.Empty);
        }
    }
}
