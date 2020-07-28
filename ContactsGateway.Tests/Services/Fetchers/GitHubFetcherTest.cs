using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContactsGateway.Exceptions;
using ContactsGateway.Models;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services;
using ContactsGateway.Services.Clients;
using ContactsGateway.Services.Fetchers;
using Moq;
using Xunit;

namespace ContactsGateway.Tests.Services.Fetchers
{
    public class GitHubFetcherTest
    {
        private readonly Mock<IGitHubClient> _client;
        private readonly Mock<IEntryFactory<GitHubContact>> _entryFactory;
        private readonly GitHubFetcher _fetcher;

        public GitHubFetcherTest()
        {
            _client = new Mock<IGitHubClient>();
            _entryFactory = new Mock<IEntryFactory<GitHubContact>>();
            _fetcher = new GitHubFetcher(
                _client.Object,
                _entryFactory.Object
            );
        }
        
        [Fact]
        public async void TestFetchAsync()
        {
            const ulong id = 123456789UL;
            const string name = "foo";
            const string screenName = "bar";

            var entry = new Mock<IEntry<GitHubContact>>();
            var contact = new GitHubContact();

            _client
                .Setup(c => c.GetAsync<GitHubContact>($"user/{id}"))
                .Returns(Task.FromResult(contact))
            ;

            _entryFactory
                .Setup(f => f.Create(contact))
                .Returns(entry.Object)
            ;

            Assert.Equal(
                entry.Object,
                await _fetcher.FetchAsync(id)
            );
        }

        [Fact]
        public async void TestFetchNotFoundAsync()
        {
            const ulong id = 123456789UL;
            var exception = new GitHubException(
                new HttpResponseMessage(HttpStatusCode.NotFound)
            );

            _client
                .Setup(c => c.GetAsync<GitHubContact>($"user/{id}"))
                .Throws(exception)
            ;

            await Assert.ThrowsAsync<ContactNotFoundException<GitHubContact>>(
                () => _fetcher.FetchAsync(id)
            );
        }

        [Fact]
        public async void TestFetchUnknownErrorAsync()
        {
            const ulong id = 123456789UL;
            var exception = new GitHubException(new HttpResponseMessage());

            _client
                .Setup(c => c.GetAsync<GitHubContact>($"user/{id}"))
                .Throws(exception)
            ;

            Assert.Equal(
                exception,
                await Assert.ThrowsAsync<GitHubException>(
                    () => _fetcher.FetchAsync(id)
                )
            );
        }
    }
}
