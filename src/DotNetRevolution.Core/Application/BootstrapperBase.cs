using DotNetRevolution.Core.Authentication;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Logging;
using DotNetRevolution.Core.Messaging;
using DotNetRevolution.Core.Querying;
using DotNetRevolution.Core.Sessions;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Application
{
    public abstract class BootstrapperBase
    {
        private ILogger _logger;

        public ICommandCatalog CommandCatalog { [Pure] get; private set; }

        public ICommandDispatcher CommandDispatcher { [Pure] get; private set; }

        public IQueryCatalog QueryCatalog { [Pure] get; private set; }

        public IQueryDispatcher QueryDispatcher { [Pure] get; private set; }

        public IDomainEventCatalog DomainEventCatalog { [Pure] get; private set; }

        public IDomainEventDispatcher DomainEventDispatcher { [Pure] get; private set; }

        public IMessageCatalog MessageCatalog { [Pure] get; private set; }

        public IMessageDispatcher MessageDispatcher { [Pure] get; private set; }

        protected ISessionManager SessionManager { [Pure] get; private set; }

        protected IIdentityManager IdentityManager { [Pure] get; private set; }

        public void Run()
        {
            var logger = GetLogger();

            try
            {
                logger.Log(LogEntryLevel.Information, "Logger created");

                GetDependencyContainer();

                SetupCatalogs();

                SetupDispatchers();

                IdentityManager = CreateIdentityManager();

                if (IdentityManager == null)
                {
                    throw new NullAuthenticationManagerException();
                }

                SessionManager = CreateSessionManager();

                if (SessionManager == null)
                {
                    throw new NullSessionManagerException();
                }

                ConfigureServices();

                ConfigureModules();
            }
            catch (Exception exception)
            {
                logger.Log(LogEntryLevel.Error, exception);

                throw;
            }
        }

        protected virtual IIdentityManager CreateIdentityManager()
        {
            return new IdentityManager();
        }

        protected virtual ISessionManager CreateSessionManager()
        {
            return new SessionManager();
        }

        [Pure]
        protected virtual ILogger GetLogger()
        {
            Contract.Ensures(Contract.Result<ILogger>() != null);

            return _logger = _logger ?? new ColoredConsoleLogger();
        }

        protected abstract IDependencyContainer GetDependencyContainer();

        protected abstract void ConfigureModules();
        protected abstract void ConfigureServices();

        [Pure]
        protected virtual ICommandCatalog CreateCommandCatalog()
        {
            return CommandCatalog = new CommandCatalog();
        }

        [Pure]
        protected virtual IQueryCatalog CreateQueryCatalog()
        {
            return QueryCatalog = new QueryCatalog();
        }

        [Pure]
        protected virtual IDomainEventCatalog CreateDomainEventCatalog()
        {
            return DomainEventCatalog = new DomainEventCatalog();
        }

        [Pure]
        protected virtual ICommandDispatcher CreateCommandDispatcher(ICommandCatalog catalog)
        {
            return new CommandDispatcher(catalog);
        }

        [Pure]
        protected virtual IMessageCatalog CreateMessageCatalog()
        {
            return MessageCatalog = new MessageCatalog();
        }

        [Pure]
        protected virtual IQueryDispatcher CreateQueryDispatcher(IQueryCatalog catalog)
        {
            return new QueryDispatcher(catalog);
        }

        [Pure]
        protected virtual IDomainEventDispatcher CreateDomainEventDispatcher(IDomainEventCatalog catalog)
        {
            return new DomainEventDispatcher(catalog);
        }

        [Pure]
        protected virtual IMessageDispatcher CreateMessageDispatcher(IMessageCatalog catalog)
        {
            return new MessageDispatcher(catalog);
        }

        private void SetupCatalogs()
        {
            SetupCatalog(CreateCommandCatalog);
            SetupCatalog(CreateDomainEventCatalog);
            SetupCatalog(CreateQueryCatalog);
            SetupCatalog(CreateMessageCatalog);
        }

        private void SetupDispatchers()
        {
            CommandDispatcher = SetupDispatcher(CreateCommandDispatcher, CommandCatalog);
            DomainEventDispatcher = SetupDispatcher(CreateDomainEventDispatcher, DomainEventCatalog);
            QueryDispatcher = SetupDispatcher(CreateQueryDispatcher, QueryCatalog);
            MessageDispatcher = SetupDispatcher(CreateMessageDispatcher, MessageCatalog);
        }

        private TService SetupDispatcher<TService, TCatalog>(Func<TCatalog, TService> createFunction, TCatalog catalog) where TService : class
        {
            Contract.Requires(createFunction != null);
            Contract.Ensures(Contract.Result<TService>() != null);

            var serviceType = typeof(TService);

            var logger = GetLogger();
            var container = GetDependencyContainer();

            logger.Log(LogEntryLevel.Information, string.Format("Creating: {0}", serviceType));

            var service = createFunction(catalog);

            if (service == null)
            {
                throw new InvalidOperationException(string.Format("Failed to create: {0}", serviceType));
            }

            logger.Log(LogEntryLevel.Information, string.Format("Registering: {0}", serviceType));

            container.RegisterSingleton(service);

            logger.Log(LogEntryLevel.Information, string.Format("Setup complete: {0}", serviceType));

            return service;
        }

        private void SetupCatalog<TService>(Func<TService> createFunction) where TService : class
        {
            Contract.Requires(createFunction != null);

            var serviceType = typeof(TService);

            var logger = GetLogger();
            var container = GetDependencyContainer();

            logger.Log(LogEntryLevel.Information, string.Format("Creating: {0}", serviceType));

            var service = createFunction();

            if (service == null)
            {
                throw new InvalidOperationException(string.Format("Failed to create: {0}", serviceType));
            }

            logger.Log(LogEntryLevel.Information, string.Format("Registering: {0}", serviceType));

            container.RegisterSingleton(service);

            logger.Log(LogEntryLevel.Information, string.Format("Setup complete: {0}", serviceType));
        }
    }
}
