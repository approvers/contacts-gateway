using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;
using ContactsGateway.Services.Fetchers;
using Discord.Rest;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsGateway.DependencyInjection.ContactServices
{
    public static class DiscordExtension
    {
        public static ContactServiceBuilder<DiscordContact> AddDiscordServices(
            this IServiceCollection services,
            string token
        )
        {
            return services.AddContactService<DiscordContact, IDiscordClient, DiscordFetcher>(provider =>
            {
                var client = new DiscordRestClient();

                client
                    .LoginAsync(Discord.TokenType.Bot, token)
                    .Wait();

                return new DiscordClient(client);
            });
        }
    }
}
