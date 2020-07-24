using System.Threading.Tasks;

namespace ContactsGateway.Services.Fetchers
{
    public interface IFetcher<T>
    {
        public Task<T> FetchAsync(ulong id);
    }
}
