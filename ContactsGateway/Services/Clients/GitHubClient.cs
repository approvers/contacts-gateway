using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ContactsGateway.Services.Clients
{
    public interface IGitHubClient
    {
        Task<T> GetAsync<T>(string url);
    }
    
    public class GitHubClient : IGitHubClient
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
            var response = await _httpClient.SendAsync(
                new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BASE_URL + url),
                    Headers =
                    {
                        { "User-Agent", "Contacts-Gateway" },
                        { "Authorization", $"Bearer {_token}" }
                    }
                }
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new GitHubException(response);
            }
            
            return JsonConvert.DeserializeObject<T>(
                await response.Content.ReadAsStringAsync()
            );
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
    
    public class GitHubException : Exception
    {
        public HttpResponseMessage Response { get; }

        public GitHubException(HttpResponseMessage response) : base("An error occured on GitHub API.")
        {
            Response = response;
        }
    }
}
