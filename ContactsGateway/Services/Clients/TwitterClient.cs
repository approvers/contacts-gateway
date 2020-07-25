using System;
using System.Threading.Tasks;
using CoreTweet;

namespace ContactsGateway.Services.Clients
{
    public interface ITwitterClient
    {
        Task<User> GetUserAsync(ulong id);
    }
    
    public class TwitterClient : ITwitterClient
    {
        private readonly OAuth2Token _token;

        public TwitterClient(OAuth2Token token)
        {
            _token = token;
        }
        
        public async Task<User> GetUserAsync(ulong id)
        {
            if (id > long.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Twitter ID must be in range of long."
                );
            }
            
            return await _token.Users.ShowAsync((long) id);
        }
    }
}
