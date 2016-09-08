using System.Diagnostics.Contracts;
namespace DotNetRevolution.Core.Domain
{
    public abstract class AggregateRoot<TAggregateRootState> : AggregateRoot, IAggregateRoot<TAggregateRootState>
        where TAggregateRootState : IAggregateRootState
    {
        public new TAggregateRootState State { get; }

        public AggregateRoot(Identity identity, TAggregateRootState state)
            : base(identity, state)
        {
            Contract.Requires(identity != null);
            Contract.Requires(state != null);

            State = state;
        }
    }
}