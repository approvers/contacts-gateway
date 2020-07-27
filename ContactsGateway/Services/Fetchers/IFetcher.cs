using System.Threading.Tasks;
using ContactsGateway.Models;

namespace ContactsGateway.Services.Fetchers
{
    public interface IFetcher<T>
    {
        Task<IEntry<T>> FetchAsync(ulong id);
    }
}
