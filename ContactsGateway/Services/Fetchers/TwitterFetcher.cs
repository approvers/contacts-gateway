using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;

namespace ContactsGateway.Services.Fetchers
{
    public class TwitterFetcher : IFetcher<TwitterContact>
    {
        private readonly ITwitterClient _client;
        
        public TwitterFetcher(ITwitterClient client)
        {
            _client = client;
        }
        
        public async Task<TwitterContact> FetchAsync(ulong id)
        {
            var user = await _client.GetUserAsync(id);
            
            return new TwitterContact(
                ((ulong?) user.Id).GetValueOrDefault(id),
                user.Name,
                user.ScreenName,
                $"https://twitter.com/{user.ScreenName}"
            );
        }
    }
}
