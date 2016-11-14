﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    public class ProjectionManagerFactory : IProjectionManagerFactory
    {
        private readonly Dictionary<Type, IProjectionManager> _managers = new Dictionary<Type, IProjectionManager>();

        public IProjectionCatalog Catalog { get; }

        public ProjectionManagerFactory(IProjectionCatalog catalog)
        {
            Contract.Requires(catalog != null);

            Catalog = catalog;
        }

        public IProjectionManager GetManager(Type projectionType)
        {
            var entry = GetEntry(projectionType);

            // cast as static projection entry to check for preinitialized projection manager
            var staticEntry = entry as StaticProjectionEntry;

            // check if entry is static entry
            if (staticEntry == null)
            {
                // lock cache for concurrency
                lock (_managers)
                {
                    // find manager in cache
                    var manager = GetCachedManager(projectionType);

                    // if manager is not cached, create and cache
                    if (manager == null)
                    {
                        manager = CreateManager(entry.ProjectionManagerType);

                        CacheManager(projectionType, manager);
                    }

                    return manager;
                }
            }

            return staticEntry.Manager;
        }

        public IEnumerable<IProjectionManager> GetManagers()
        {
            var managers = new Collection<IProjectionManager>();

            // loop through all entries in catalog to retrieve projection managers
            foreach (var entry in Catalog.Entries)
            {
                Contract.Assume(entry != null);

                // cast as static projection entry to check for preinitialized projection manager
                var staticEntry = entry as StaticProjectionEntry;

                // check if entry is static entry
                if (staticEntry == null)
                {
                    // lock cache for concurrency
                    lock (_managers)
                    {
                        var projectionType = entry.ProjectionType;

                        // find manager in cache
                        var manager = GetCachedManager(projectionType);

                        // if manager is not cached, create and cache
                        if (manager == null)
                        {
                            manager = CreateManager(entry.ProjectionManagerType);

                            CacheManager(projectionType, manager);
                        }

                        // add manager to list
                        managers.Add(manager);
                    }
                }
                else
                {
                    // add static manager to list
                    managers.Add(staticEntry.Manager);
                }
            }

            // return managers
            return managers;
        }        

        [Pure]
        protected virtual IProjectionManager CreateManager(Type managerType)
        {
            Contract.Requires(managerType != null);
            Contract.Ensures(Contract.Result<IProjectionManager>() != null);

            return (IProjectionManager)Activator.CreateInstance(managerType);
        }

        private void CacheManager(Type projectionType, IProjectionManager manager)
        {
            Contract.Requires(projectionType != null);
            Contract.Requires(manager != null);

            _managers[projectionType] = manager;
        }

        [Pure]
        private IProjectionManager GetCachedManager(Type projectionType)
        {
            Contract.Requires(projectionType != null);

            IProjectionManager manager;

            _managers.TryGetValue(projectionType, out manager);

            return manager;
        }

        [Pure]
        private IProjectionEntry GetEntry(Type projectionType)
        {
            Contract.Requires(projectionType != null);
            Contract.Ensures(Contract.Result<IProjectionEntry>() != null);

            var entry = Catalog.GetEntry(projectionType);
            Contract.Assume(entry != null);

            return entry;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_managers != null);
        }
    }
}