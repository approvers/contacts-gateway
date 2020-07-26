namespace ContactsGateway.Services.Caching
{
    public interface ICacheEntryFactory<out T, in U> where T : ICacheEntry<U> where U : ICacheable
    {
        T Create(U item);
    }
}
