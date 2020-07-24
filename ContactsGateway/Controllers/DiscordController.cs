using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Fetchers;
using Microsoft.AspNetCore.Mvc;

namespace ContactsGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscordController : Controller
    {
        private readonly DiscordFetcher _fetcher;
        
        public DiscordController(DiscordFetcher fetcher)
        {
            _fetcher = fetcher;
        }
        
        [HttpGet]
        [Route("{id}")]
        public Task<DiscordContact> Get(ulong id)
        {
            return _fetcher.FetchAsync(id);
        }
    }
}
