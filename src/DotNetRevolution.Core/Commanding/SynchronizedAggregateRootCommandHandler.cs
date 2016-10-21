using DotNetRevolution.Core.Caching;
using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding
{
    public class SynchronizedAggregateRootCommandHandler<TAggregateRoot, TCommand> : AggregateRootCommandHandler<TAggregateRoot, TCommand>
         where TAggregateRoot : class, IAggregateRoot
         where TCommand : IAggregateRootCommand<TAggregateRoot>
    {
        private readonly IAggregateRootSynchronizer _synchronizer;
        private readonly ICache _cache;

        public SynchronizedAggregateRootCommandHandler(IRepository<TAggregateRoot> repository,
                                                       IAggregateRootSynchronizer synchronizer,
                                                       ICache cache)
            : base(repository)
        {
            Contract.Requires(repository != null);
            Contract.Requires(synchronizer != null);
            Contract.Requires(cache != null);
            
            _synchronizer = synchronizer;
            _cache = cache;
        }

        protected override TAggregateRoot GetAggregateRoot(AggregateRootIdentity identity)
        {
            // retrieve from cache or get from base handler and store in cache
            var aggregateRoot = _cache.AddOrGetExisting(GetCacheKey(identity), new Lazy<TAggregateRoot>(() => base.GetAggregateRoot(identity)));
            Contract.Assume(aggregateRoot != null);

            return aggregateRoot;
        }

        protected override async Task<TAggregateRoot> GetAggregateRootAsync(AggregateRootIdentity identity)
        {
            // retrieve from cache or get from base handler and store in cache
            var task = _cache.AddOrGetExisting(GetCacheKey(identity), new Lazy<Task<TAggregateRoot>>(() => base.GetAggregateRootAsync(identity)));
            Contract.Assume(task != null);

            return await task;
        }

        public override ICommandHandlingResult Handle(TCommand command)
        {
            Contract.Assume(command.AggregateRootId != Guid.Empty);

            // enter aggregate root synchronization
            using (var context = _synchronizer.Enter(typeof(TAggregateRoot), command.AggregateRootId))
            {
                try
                {                    
                    // call base class to handle command
                    return Handle(command, context.Identity);
                }
                catch
                {
                    // remove aggregate root from cache in case of error
                    _cache.Remove(GetCacheKey(context.Identity));

                    throw;
                }
            }
        }

        public override async Task<ICommandHandlingResult> HandleAsync(TCommand command)
        {
            Contract.Assume(command.AggregateRootId != Guid.Empty);

            // enter aggregate root synchronization
            using (var context = await _synchronizer.EnterAsync(typeof(TAggregateRoot), command.AggregateRootId))
            {
                try
                {
                    // call base class to handle command
                    return await HandleAsync(command, context.Identity);
                }
                catch
                {
                    // remove aggregate root from cache in case of error
                    _cache.Remove(GetCacheKey(context.Identity));

                    throw;
                }
            }
        }

        private static string GetCacheKey(AggregateRootIdentity identity)
        {
            Contract.Requires(identity != null);
            Contract.Ensures(string.IsNullOrWhiteSpace(Contract.Result<string>()) == false);

            var cacheKey = $"{typeof(TAggregateRoot).FullName}::{identity.Value}";
            Contract.Assume(string.IsNullOrWhiteSpace(cacheKey) == false);

            return cacheKey;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_synchronizer != null);
            Contract.Invariant(_cache != null);
        }
    }
}
