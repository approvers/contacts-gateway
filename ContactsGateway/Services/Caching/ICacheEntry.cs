using System;

namespace ContactsGateway.Services.Caching
{
    public interface ICacheEntry<T> where T : ICacheable
    {
        public T Item { get; }
    }
}
