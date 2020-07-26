using System.Threading.Tasks;
using ContactsGateway.Services.Caching;
using ContactsGateway.Services.Caching.Cache;

namespace ContactsGateway.Services.Fetchers
{
    public class CachedFetcher<T> : IFetcher<T> where T : ICacheable
    {
        private readonly ICache<T> _cache;
        private readonly IFetcher<T> _fetcher;
        
        public CachedFetcher(ICache<T> cache, IFetcher<T> fetcher)
        {
            _cache = cache;
            _fetcher = fetcher;
        }

        public Task<T> FetchAsync(ulong id)
        {
            return _cache.HitOrUpdateAsync(id, _fetcher.FetchAsync);
        }
    }
}
