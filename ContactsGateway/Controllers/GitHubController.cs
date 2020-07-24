using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Fetchers;
using Microsoft.AspNetCore.Mvc;

namespace ContactsGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GitHubController : Controller
    {
        private readonly GitHubFetcher _fetcher;
        
        public GitHubController(GitHubFetcher fetcher)
        {
            _fetcher = fetcher;
        }
        
        [HttpGet]
        [Route("{id}")]
        public Task<GitHubContact> Get(ulong id)
        {
            return _fetcher.FetchAsync(id);
        }
    }
}
