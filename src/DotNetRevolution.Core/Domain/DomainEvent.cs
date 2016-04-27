using DotNetRevolution.Core.Base;
using System;

namespace DotNetRevolution.Core.Domain
{
    public abstract class DomainEvent : IDomainEvent
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
