using System;
using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using CoreTweet;

namespace ContactsGateway.Services.Fetchers
{
    public class TwitterFetcher : IFetcher<TwitterContact>
    {
        private readonly OAuth2Token _token;
        
        public TwitterFetcher(OAuth2Token token)
        {
            _token = token;
        }
        
        public async Task<TwitterContact> FetchAsync(ulong id)
        {
            if (id > long.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Twitter ID must be in range of long."
                );
            }

            var userId = (long) id;
            var user = await _token.Users.ShowAsync(userId);
            
            return new TwitterContact(
                (ulong) user.Id.GetValueOrDefault(userId),
                user.Name,
                user.ScreenName,
                $"https://twitter.com/{user.ScreenName}"
            );
        }
    }
}
