using System;
using System.Threading.Tasks;
using ContactsGateway.Services.Caching.Storage;

namespace ContactsGateway.Services.Caching.Cache
{
    public class TimeoutCache<T> : ICache<T> where T : ICacheable
    {
        public class Entry : ICacheEntry<T>
        {
            public T Item { get; }
            public DateTimeOffset CachedAt { get; }

            public Entry(T item, DateTimeOffset cachedAt)
            {
                Item = item;
                CachedAt = cachedAt;
            }
        }

        public class EntryFactory : ICacheEntryFactory<Entry, T>
        {
            public Entry Create(T item)
            {
                return new Entry(
                    item,
                    DateTimeOffset.Now
                );
            }
        }
        
        private readonly ICacheStorage<Entry, T> _storage;
        private readonly EntryFactory _entryFactory;
        private readonly TimeSpan _timeout;
        
        public TimeoutCache(ICacheStorage<Entry, T> storage, EntryFactory entryFactory, TimeSpan timeout)
        {
            _storage = storage;
            _entryFactory = entryFactory;
            _timeout = timeout;
        }

        public async Task<T> HitOrUpdateAsync(ulong id, Func<ulong, Task<T>> updater)
        {
            if (await _storage.HasAsync(id))
            {
                var entry = await _storage.RestoreAsync(id);
                if (entry.CachedAt + _timeout > DateTimeOffset.Now)
                {
                    return entry.Item;
                }
            }

            var item = await updater(id);
            await _storage.StoreAsync(
                _entryFactory.Create(item)
            );

            return item;
        }
    }
}
