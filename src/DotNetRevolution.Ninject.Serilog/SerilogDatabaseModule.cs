using DotNetRevolution.Logging.Serilog.Sink;
using Ninject.Modules;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Ninject.Serilog
{
    public class SerilogDatabaseModule : NinjectModule
    {
        private readonly LogEventLevel _defaultMinimumLevel;
        private readonly string _connectionString;
        private readonly string _tableName;

        public SerilogDatabaseModule(string connectionString,
                                     string tableName,
                                     LogEventLevel defaultMinimumLevel)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(connectionString) == false);
            Contract.Requires(string.IsNullOrWhiteSpace(tableName) == false);

            _defaultMinimumLevel = defaultMinimumLevel;
            _connectionString = connectionString;
            _tableName = tableName;
        }        

        public override void Load()
        {
            var binding = Bind<SinkWrapper>();
            Contract.Assume(binding != null);

            var columnOptions = new ColumnOptions
                {
                    Store = new Collection<StandardColumn>
                            {
                                StandardColumn.Message,
                                StandardColumn.Level,
                                StandardColumn.TimeStamp,
                                StandardColumn.Exception,
                                StandardColumn.Properties
                            }
                };

            //var bindingMethod = binding.ToConstant(new ConcreteSinkWrapper(_defaultMinimumLevel, new DatabaseSink(_connectionString)));
            var bindingMethod = binding.ToConstant(new ConfigurationExtensionSinkWrapper(_defaultMinimumLevel, configuration => configuration.MSSqlServer(_connectionString, _tableName, _defaultMinimumLevel, columnOptions: columnOptions)));
            Contract.Assume(bindingMethod != null);
            
            bindingMethod.WhenParentNamed(LogFactoryName.Main.ToString());
        }
    }
}
