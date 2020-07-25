using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;

namespace ContactsGateway.Services.Fetchers
{
    public class DiscordFetcher : IFetcher<DiscordContact>
    {
        private readonly IDiscordClient _client;
        
        public DiscordFetcher(IDiscordClient client)
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
