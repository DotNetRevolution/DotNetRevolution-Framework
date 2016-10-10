using DotNetRevolution.Core.Caching;
using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

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

        protected override TAggregateRoot GetAggregateRoot(Identity identity)
        {
            // retrieve from cache or get from base handler and store in cache
            var aggregateRoot = _cache.AddOrGetExisting(GetCacheKey(identity), new Lazy<TAggregateRoot>(() => base.GetAggregateRoot(identity)));
            Contract.Assume(aggregateRoot != null);

            return aggregateRoot;
        }

        public override void Handle(TCommand command)
        {
            Contract.Assume(command.AggregateRootId != Guid.Empty);

            // enter aggregate root synchronization
            var identity = _synchronizer.Enter(typeof(TAggregateRoot), command.AggregateRootId);

            try
            {
                // call base class to handle command
                base.Handle(command);
            }
            catch
            {
                // remove aggregate root from cache in case of error
                _cache.Remove(GetCacheKey(identity));

                throw;
            }
            finally
            {
                // leave aggregate root synchronization
                _synchronizer.Exit(identity);
            }
        }

        private static string GetCacheKey(Identity identity)
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
