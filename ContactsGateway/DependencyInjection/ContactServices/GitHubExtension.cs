using System.Net.Http;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;
using ContactsGateway.Services.Fetchers;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsGateway.DependencyInjection.ContactServices
{
    public static class GitHubExtension
    {
        public static ContactServiceBuilder<GitHubContact> AddGitHubServices(
            this IServiceCollection services,
            string token
        )
        {
            return services.AddContactService<GitHubContact, IGitHubClient, GitHubFetcher>(provider => new GitHubClient(
                new HttpClient(),
                token
            ));
        }
    }
}
