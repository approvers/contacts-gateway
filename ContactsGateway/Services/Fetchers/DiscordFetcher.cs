using System.Threading.Tasks;
using ContactsGateway.Exceptions;
using ContactsGateway.Models;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;

namespace ContactsGateway.Services.Fetchers
{
    public class DiscordFetcher : IFetcher<DiscordContact>
    {
        private readonly IDiscordClient _client;
        private readonly IEntryFactory<DiscordContact> _entryFactory;
        
        public DiscordFetcher(IDiscordClient client, IEntryFactory<DiscordContact> entryFactory)
        {
            _client = client;
            _entryFactory = entryFactory;
        }
        
        public async Task<IEntry<DiscordContact>> FetchAsync(ulong id)
        {
            var user = await _client.GetUserAsync(id);

            if (user is null)
            {
                throw new ContactNotFoundException<DiscordContact>(null);
            }

            return _entryFactory.Create(
                new DiscordContact(
                    user.Id,
                    user.Username,
                    user.Discriminator,
                    null
                )
            );
        }
    }
}
