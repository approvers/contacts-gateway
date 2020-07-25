using System.Net;
using System.Threading.Tasks;
using ContactsGateway.Exceptions;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;
using CoreTweet;

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
            try
            {
                var user = await _client.GetUserAsync(id);

                return new TwitterContact(
                    ((ulong?) user.Id).GetValueOrDefault(id),
                    user.Name,
                    user.ScreenName,
                    $"https://twitter.com/{user.ScreenName}"
                );
            }
            catch (TwitterException e)
            {
                if (e.Status == HttpStatusCode.NotFound)
                {
                    throw new ContactNotFoundException<TwitterContact>(e);
                }

                throw;
            }
        }
    }
}
