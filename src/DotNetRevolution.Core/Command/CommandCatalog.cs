using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Command
{
    public class CommandCatalog : ICommandCatalog
    {
        private readonly Dictionary<Type, ICommandEntry> _entries = new Dictionary<Type, ICommandEntry>();

        public IReadOnlyCollection<Type> CommandTypes
        {
            get { return _entries.Keys.ToList().AsReadOnly(); }
        }

        public CommandCatalog()
        {
        }

        public CommandCatalog(IReadOnlyCollection<ICommandEntry> entries)
        {            
            Contract.Requires(entries != null);
            Contract.Requires(Contract.ForAll(entries, o => o != null));

            foreach(var entry in entries)
            {
                Add(entry);
            }
        }

        public ICommandCatalog Add(ICommandEntry entry)
        {
            _entries.Add(entry.CommandType, entry);

            Contract.Assume(GetEntry(entry.CommandType) == entry);

            return this;
        }

        public ICommandEntry GetEntry(Type commandType)
        {
            var result = _entries[commandType];
            Contract.Assume(result != null);

            return result;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_entries != null);
        }
    }
}
