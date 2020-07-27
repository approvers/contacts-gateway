using System;
using System.Threading.Tasks;
using ContactsGateway.Models;

namespace ContactsGateway.Services.Caching.Cache
{
    public interface ICache<T> where T : ICacheable
    {
        Task<ICacheEntry<T>> HitOrUpdateAsync(ulong id, Func<ulong, Task<IEntry<T>>> updater);
    }
}
