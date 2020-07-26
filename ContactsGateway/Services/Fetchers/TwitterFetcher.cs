using System.Net;
using System.Threading.Tasks;
using ContactsGateway.Exceptions;
using ContactsGateway.Models;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;
using CoreTweet;

namespace ContactsGateway.Services.Fetchers
{
    public class TwitterFetcher : IFetcher<TwitterContact>
    {
        private readonly ITwitterClient _client;
        private readonly IEntryFactory<TwitterContact> _entryFactory;
        
        public TwitterFetcher(ITwitterClient client, IEntryFactory<TwitterContact> entryFactory)
        {
            _client = client;
            _entryFactory = entryFactory;
        }
        
        public async Task<IEntry<TwitterContact>> FetchAsync(ulong id)
        {
            try
            {
                var user = await _client.GetUserAsync(id);

                return _entryFactory.Create(
                new TwitterContact(
                        ((ulong?) user.Id).GetValueOrDefault(id),
                        user.Name,
                        user.ScreenName,
                        $"https://twitter.com/{user.ScreenName}"
                    )
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
