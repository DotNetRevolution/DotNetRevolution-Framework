using DotNetRevolution.Logging.Serilog.Sink;
using Ninject.Modules;
using Ninject.Syntax;
using Serilog.Events;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Ninject.Serilog
{
    public class SerilogDatabaseModule : NinjectModule
    {
        private readonly LogEventLevel _defaultMinimumLevel;
        private readonly string _connectionString;

        public SerilogDatabaseModule(string connectionString,
                                     LogEventLevel defaultMinimumLevel)
        {            
            Contract.Requires(connectionString != null);
            Contract.Requires(connectionString.Length > 0);

            _defaultMinimumLevel = defaultMinimumLevel;
            _connectionString = connectionString;            
        }        

        public override void Load()
        {
            var binding = Bind<SinkWrapper>();

            Contract.Assume(binding != null);

            var bindingMethod = binding.ToMethod(context => new SinkWrapper(new DatabaseSink(_connectionString), _defaultMinimumLevel));

            Contract.Assume(bindingMethod != null);
            
            bindingMethod.WhenParentNamed(SerilogModule.MainLoggerFactoryName);
        }
    }
}