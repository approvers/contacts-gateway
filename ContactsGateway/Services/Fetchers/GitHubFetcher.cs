using System.Net;
using System.Threading.Tasks;
using ContactsGateway.Exceptions;
using ContactsGateway.Models;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;

namespace ContactsGateway.Services.Fetchers
{
    public class GitHubFetcher : IFetcher<GitHubContact>
    {
        private readonly IGitHubClient _client;
        private readonly IEntryFactory<GitHubContact> _entryFactory;
        
        public GitHubFetcher(IGitHubClient client, IEntryFactory<GitHubContact> entryFactory)
        {
            _client = client;
            _entryFactory = entryFactory;
        }
        
        public async Task<IEntry<GitHubContact>> FetchAsync(ulong id)
        {
            try
            {
                return _entryFactory.Create(
                    await _client.GetAsync<GitHubContact>($"user/{id}")
                );
            }
            catch (GitHubException e)
            {
                if (e.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ContactNotFoundException<GitHubContact>(e);
                }
                
                throw;
            }
        }
    }
}
