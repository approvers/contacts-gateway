using System;
using System.Threading.Tasks;

namespace ContactsGateway.Services.Caching.Cache
{
    public interface ICache<T> where T : ICacheable
    {
        Task<T> HitOrUpdateAsync(ulong id, Func<ulong, Task<T>> updater);
    }
}
