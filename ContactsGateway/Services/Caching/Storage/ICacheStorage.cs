using System.Threading.Tasks;

namespace ContactsGateway.Services.Caching.Storage
{
    public interface ICacheStorage<T, U> where T : ICacheEntry<U> where U : ICacheable
    {
        Task<bool> HasAsync(ulong id);
        Task<T> RestoreAsync(ulong id);
        Task StoreAsync(T entry);
    }
}
