using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Logging.CodeContract
{
    [ContractClassFor(typeof(ILoggerFactory))]
    internal abstract class LoggerFactoryContract : ILoggerFactory
    {
        public ILogger GetLogger(Type type)
        {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<ILogger>() != null);

            throw new NotImplementedException();
        }
    }
}
