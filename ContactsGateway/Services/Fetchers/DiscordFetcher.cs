using System;
using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using Discord.Rest;

namespace ContactsGateway.Services.Fetchers
{
    public class DiscordFetcher : IFetcher<DiscordContact>
    {
        private readonly DiscordRestClient _client;
        
        public DiscordFetcher(DiscordRestClient client)
        {
            _client = client;
        }
        
        public async Task<DiscordContact> FetchAsync(ulong id)
        {
            var user = await _client.GetUserAsync(id);

            return new DiscordContact(
                user.Id,
                user.Username,
                user.Discriminator,
                null
            );
        }
    }
}
