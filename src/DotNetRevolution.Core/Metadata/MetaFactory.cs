using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Metadata
{
    public class MetaFactory : IMetaFactory
    {
        private readonly Meta _meta;

        public MetaFactory(Meta meta)
        {
            Contract.Requires(meta != null);

            _meta = meta;
        }

        public Meta GetMeta()
        {
            return _meta;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_meta != null);
        }
    }
}
