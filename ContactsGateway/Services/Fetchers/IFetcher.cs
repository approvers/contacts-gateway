using System.Threading.Tasks;

namespace ContactsGateway.Services.Fetchers
{
    public interface IFetcher<T>
    {
        Task<T> FetchAsync(ulong id);
    }
}
