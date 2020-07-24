using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;

namespace ContactsGateway.Services.Fetchers
{
    public class GitHubFetcher : IFetcher<GitHubContact>
    {
        private readonly GitHubClient _client;
        
        public GitHubFetcher(GitHubClient client)
        {
            _client = client;
        }
        
        public Task<GitHubContact> FetchAsync(ulong id)
        {
            return _client.GetAsync<GitHubContact>($"user/{id}");
        }
    }
}
