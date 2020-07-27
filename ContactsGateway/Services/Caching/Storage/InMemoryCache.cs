using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactsGateway.Services.Caching.Storage
{
    public class InMemoryCacheStorage<T, U> : ICacheStorage<T, U> where T : ICacheEntry<U> where U : ICacheable
    {
        private readonly IDictionary<ulong, T> _items;

        public InMemoryCacheStorage()
        {
            _items = new Dictionary<ulong, T>();
        }
        
        public Task<T> RestoreAsync(ulong id)
        {
            return Task.FromResult(_items[id]);
        }

        public Task<bool> HasAsync(ulong id)
        {
            return Task.FromResult(_items.ContainsKey(id));
        }

        public Task StoreAsync(T entry)
        {
            _items.Add(entry.Item.Id, entry);

            return Task.CompletedTask;
        }
    }
}
