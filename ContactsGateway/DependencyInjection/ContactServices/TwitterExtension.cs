using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;
using ContactsGateway.Services.Fetchers;
using CoreTweet;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsGateway.DependencyInjection.ContactServices
{
    public static class TwitterExtension
    {
        public static ContactServiceBuilder<TwitterContact> AddTwitterServices(
            this IServiceCollection services,
            string consumerKey,
            string consumerSecret
        )
        {
            return services.AddContactService<TwitterContact, ITwitterClient, TwitterFetcher>(provider => new TwitterClient(
                OAuth2.GetToken(
                    consumerKey,
                    consumerSecret
                )
            ));
        }
    }
}
