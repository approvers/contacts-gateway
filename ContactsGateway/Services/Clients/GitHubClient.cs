using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ContactsGateway.Services.Clients
{
    public class GitHubClient
    {
        private const string BASE_URL = "https://api.github.com/";
        
        private readonly HttpClient _httpClient;
        private readonly string _token;
        
        public GitHubClient(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(BASE_URL + url),
                Headers =
                {
                    { "User-Agent", "Contacts-Gateway" },
                    { "Authorization", $"Bearer {_token}" }
                }
            };
            
            return JsonConvert.DeserializeObject<T>(
                await (await _httpClient.SendAsync(message)).Content.ReadAsStringAsync()
            );
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
