using System;
using System.Net;
using System.Threading.Tasks;
using ContactsGateway.Exceptions;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;

namespace ContactsGateway.Services.Fetchers
{
    public class GitHubFetcher : IFetcher<GitHubContact>
    {
        private readonly IGitHubClient _client;
        
        public GitHubFetcher(IGitHubClient client)
        {
            _client = client;
        }
        
        public async Task<GitHubContact> FetchAsync(ulong id)
        {
            try
            {
                return await _client.GetAsync<GitHubContact>($"user/{id}");
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
