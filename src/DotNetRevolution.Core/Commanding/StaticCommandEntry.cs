using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public class StaticCommandEntry : CommandEntry
    {
        private readonly ICommandHandler _handler;

        public ICommandHandler Handler
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommandHandler>() != null);

                return _handler;
            }
        }

        public StaticCommandEntry(Type commandType, ICommandHandler handler)
            : base(commandType, handler.GetType())
        {
            Contract.Requires(commandType != null);
            Contract.Requires(handler != null);

            _handler = handler;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handler != null);
        }
    }
}
