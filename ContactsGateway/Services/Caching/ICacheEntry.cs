using ContactsGateway.Models;

namespace ContactsGateway.Services.Caching
{
    public interface ICacheEntry<out T> : IEntry<T> where T : ICacheable
    {
    }
}
