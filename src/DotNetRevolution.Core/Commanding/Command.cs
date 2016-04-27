using DotNetRevolution.Core.Base;
using System;

namespace DotNetRevolution.Core.Commanding
{
    public abstract class Command : ICommand
    {
        private readonly Guid _id = SequentialGuid.Create();

        public Guid Id
        {
            get
            {
                return _id;
            }
        }
    }
}
