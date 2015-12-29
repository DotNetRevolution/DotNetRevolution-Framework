using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Logging.CodeContract
{
    [ContractClassFor(typeof(ILoggerFactory))]
    public abstract class LoggerFactoryContract : ILoggerFactory
    {
        public ILogger Create(Type type)
        {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<ILogger>() != null);

            throw new NotImplementedException();
        }
    }
}