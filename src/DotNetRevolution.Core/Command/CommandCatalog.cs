using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Command
{
    public class CommandCatalog : ICommandCatalog
    {
        private readonly Dictionary<Type, ICommandEntry> _entries;

        public IReadOnlyCollection<Type> CommandTypes
        {
            get { return _entries.Keys.ToList().AsReadOnly(); }
        }
        
        public CommandCatalog()
        {
            _entries = new Dictionary<Type, ICommandEntry>();
        }

        public void Add(ICommandEntry entry)
        {
            _entries.Add(entry.CommandType, entry);

            Contract.Assume(GetEntry(entry.CommandType) != null);
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
