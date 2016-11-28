using DotNetRevolution.Core.Persistence;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerContextFactory _contextFactory;
        private readonly ICommandHandlerFactory _handlerFactory;

        public CommandDispatcher(ICommandHandlerFactory handlerFactory,
                                 ICommandHandlerContextFactory contextFactory)
        {
            Contract.Requires(handlerFactory != null);
            Contract.Requires(contextFactory != null);

            _handlerFactory = handlerFactory;
            _contextFactory = contextFactory;
        }
  
        public void Dispatch(ICommand command)
        {            
            var handler = GetHandler(command);
            var context = GetContext(command);

            try
            {
                handler.Handle(context);
            }
            catch (AggregateRootConcurrencyException)
            {
                // keep trying command until no more ConcurrencyException occur
                Dispatch(command);            
            }
            catch (Exception e)
            {
                // re-throw exception as a command handling exception
                throw new CommandHandlingException(command, e, "Exception occurred in command handler, check inner exception for details.");
            }
        }

        public async Task DispatchAsync(ICommand command)
        {
            var handler = GetHandler(command);
            var context = GetContext(command);

            try
            {
                await handler.HandleAsync(context);
            }
            catch (AggregateRootConcurrencyException)
            {
                // keep trying command until no more ConcurrencyException occur
                await DispatchAsync(command);
            }
            catch (Exception e)
            {
                // re-throw exception as a command handling exception
                throw new CommandHandlingException(command, e, "Exception occurred in command handler, check inner exception for details.");
            }
        }
        
        private ICommandHandlerContext GetContext(ICommand command)
        {
            Contract.Requires(command != null);

            return _contextFactory.GetContext(command);
        }

        private ICommandHandler GetHandler(ICommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

            try
            {
                // get handler from factory
                return _handlerFactory.GetHandler(command.GetType());
            }
            catch (Exception e)
            {
                // rethrow exception has a command handling exception
                throw new CommandHandlingException(command, e, "Could not get command handler.");
            }
        }        

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handlerFactory != null);
            Contract.Invariant(_contextFactory != null);
        }
    }
}
