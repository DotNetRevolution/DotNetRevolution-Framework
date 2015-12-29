using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Logging.CodeContract;

namespace DotNetRevolution.Core.Logging
{
    [ContractClass(typeof(LoggerFactoryContract))]
    public interface ILoggerFactory
    {
        [Pure]
        ILogger Create(Type type);
    }
}