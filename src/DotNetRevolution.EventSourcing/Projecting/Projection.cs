using System;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public abstract class Projection
    {
        public HashSet<Guid> HandledTransactions { get; }

        public Projection()
        {
            HandledTransactions = new HashSet<Guid>();
        }
    }
}
