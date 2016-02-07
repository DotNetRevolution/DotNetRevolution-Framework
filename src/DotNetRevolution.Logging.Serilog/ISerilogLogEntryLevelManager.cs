using DotNetRevolution.Core.Logging;
using DotNetRevolution.Logging.Serilog.CodeContract;
using Serilog.Core;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Logging.Serilog
{
    [ContractClass(typeof(SerilogLogEntryLevelManagerContract))]
    public interface ISerilogLogEntryLevelManager : ILogEntryLevelManager
    {
        LoggingLevelSwitch LoggingLevelSwitch { [Pure] get; }
    }
}
