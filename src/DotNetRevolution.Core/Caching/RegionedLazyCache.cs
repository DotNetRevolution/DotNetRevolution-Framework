using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Caching
{
    public class RegionedLazyCache : LazyCache
    {
        private readonly string _region;

        public RegionedLazyCache(string region)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(region) == false);

            _region = region;
        }

        protected override string AddRegionToKey(string key)
        {
            return $"{_region}::{key}";
        }
    }
}
