using DotNetRevolution.Core.Commanding;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        public AggregateRootIdentity Identity { get; }

        public IAggregateRootState State { get; }

        protected AggregateRoot(AggregateRootIdentity identity,
                                IAggregateRootState state)
        {
            Contract.Requires(identity != null);
            Contract.Requires(state != null);

            Identity = identity;
            State = state;
        }        

        public override string ToString()
        {
            return $"{GetType().FullName}:{Identity.Value}";
        }

        public abstract void Execute(ICommand command);
    }
}
