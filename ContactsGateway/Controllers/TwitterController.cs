using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Fetchers;
using Microsoft.AspNetCore.Mvc;

namespace ContactsGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : Controller
    {
        private readonly TwitterFetcher _fetcher;
        
        public TwitterController(TwitterFetcher fetcher)
        {
            _fetcher = fetcher;
        }
        
        [HttpGet]
        [Route("{id}")]
        public Task<TwitterContact> Get(ulong id)
        {
            return _fetcher.FetchAsync(id);
        }
    }
}
