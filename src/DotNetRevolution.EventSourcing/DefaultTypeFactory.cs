using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing
{
    public class DefaultTypeFactory : ITypeFactory
    {
        private static Collection<byte[]> AssemblyPublicKeyTokensToExclude = new Collection<byte[]>
        {
            // PublicKeyToken=b77a5c561934e089
            new byte[8] { 183, 122, 92, 86, 25, 52, 224, 137 },

            // PublicKeyToken=b03f5f7f11d50a3a
            new byte[8] { 176, 63, 95, 127, 17, 213, 10, 58 },

            // PublicKeyToken=31bf3856ad364e35
            new byte[8] { 49, 191, 56, 86, 173, 54, 78, 53 }
        };

        private readonly IHashProvider _hashProvider;
        private readonly Dictionary<byte[], Type> _types = new Dictionary<byte[], Type>();

        public DefaultTypeFactory(IHashProvider hashProvider)
        {
            Contract.Requires(hashProvider != null);

            _hashProvider = hashProvider;

            // go through each assembly in the current domain that is not in the exclude list
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => !AssemblyPublicKeyTokensToExclude.Any(y => y.SequenceEqual(x.GetName().GetPublicKeyToken()))))
            {
                Contract.Assume(assembly != null);

                // go through each type in assembly
                foreach (var type in assembly.GetTypes())
                {
                    // cache by hash code of types full name
                    _types.Add(GetHash(type), type);
                }
            }
        }
        
        public byte[] GetHash(Type type)
        {
            return _hashProvider.GetHash(type.FullName);
        }

        public Type GetType(byte[] hash)
        {
            if (_types.Count(x => x.Key.SequenceEqual(hash)) == 1)
            {
                var value = _types.First(x => x.Key.SequenceEqual(hash)).Value;
                Contract.Assume(value != null);

                return value;
            }
            
            throw new TypeNotFoundForHashException(hash);
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_hashProvider != null);
            Contract.Invariant(_types != null);
        }
    }
}
