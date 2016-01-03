using DotNetRevolution.Logging.Serilog.Sink;
using Ninject.Modules;
using Serilog.Events;
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

            var bindingMethod = binding.ToMethod(context => new SinkWrapper(new EmailSink(_emailConnectionInfo), _defaultMinimumLevel));

            Contract.Assume(bindingMethod != null);

            bindingMethod.WhenParentNamed(SerilogModule.MainLoggerFactoryName);            
        }
    }
}