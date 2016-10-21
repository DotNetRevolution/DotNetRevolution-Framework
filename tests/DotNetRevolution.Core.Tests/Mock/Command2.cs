using System;
using DotNetRevolution.Core.Commanding;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class Command2 : Command
    {
        public Command2(Guid commandId) 
            : base(commandId)
        {
            Contract.Requires(commandId != Guid.Empty);
        }
    }
}
