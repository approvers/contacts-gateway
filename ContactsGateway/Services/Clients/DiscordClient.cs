using System.Threading.Tasks;
using Discord;

namespace ContactsGateway.Services.Clients
{
    public interface IDiscordClient
    {
        Task<IUser> GetUserAsync(ulong id);
    }
    
    public class DiscordClient : IDiscordClient
    {
        private readonly Discord.IDiscordClient _client;

        public DiscordClient(Discord.IDiscordClient client)
        {
            _client = client;
        }
        
        public Task<IUser> GetUserAsync(ulong id)
        {
            return _client.GetUserAsync(id);
        }
    }
}
