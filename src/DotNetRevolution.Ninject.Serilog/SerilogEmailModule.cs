using DotNetRevolution.Logging.Serilog;
using DotNetRevolution.Logging.Serilog.Sink;
using Ninject.Modules;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Ninject.Serilog
{
    public class SerilogEmailModule : NinjectModule
    {
        private readonly EmailConnectionInfo _emailConnectionInfo;
        private readonly LogEventLevel _defaultMinimumLevel;
        
        public SerilogEmailModule(EmailConnectionInfo emailConnectionInfo,
                                  LogEventLevel defaultMinimumLevel)
        {
            Contract.Requires(emailConnectionInfo != null);

            _emailConnectionInfo = emailConnectionInfo;
            _defaultMinimumLevel = defaultMinimumLevel;
        }

        public override void Load()
        {
            var binding = Bind<SinkWrapper>();
            Contract.Assume(binding != null);

            var bindingMethod = binding.ToConstant(new ConfigurationExtensionSinkWrapper(_defaultMinimumLevel, configuration => configuration.Email(_emailConnectionInfo, new MessageTemplateTextFormatter(Logger.Template, null), _defaultMinimumLevel)));
            Contract.Assume(bindingMethod != null);

            bindingMethod.WhenParentNamed(LogFactoryName.Main.ToString());            
        }
    }
}
