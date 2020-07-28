using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using ContactsGateway.Exceptions;
using ContactsGateway.Models;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services;
using ContactsGateway.Services.Clients;
using ContactsGateway.Services.Fetchers;
using CoreTweet;
using Moq;
using Xunit;

namespace ContactsGateway.Tests.Services.Fetchers
{
    public class TwitterFetcherTest
    {
        private readonly Mock<ITwitterClient> _client;
        private readonly Mock<IEntryFactory<TwitterContact>> _entryFactory;
        private readonly TwitterFetcher _fetcher;

        public TwitterFetcherTest()
        {
            _client = new Mock<ITwitterClient>();
            _entryFactory = new Mock<IEntryFactory<TwitterContact>>();
            _fetcher = new TwitterFetcher(
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

            var entry = new Mock<IEntry<TwitterContact>>();
            var user = new User
            {
                Id = (long) id,
                Name = name,
                ScreenName = screenName
            };

            _client
                .Setup(c => c.GetUserAsync(id))
                .Returns(Task.FromResult(user))
            ;

            _entryFactory
                .Setup(f => f.Create(
                    It.Is<TwitterContact>(
                        contact => contact.Id == id
                            && contact.Name == name 
                            && contact.ScreenName == screenName
                            && contact.Url == $"https://twitter.com/{screenName}"
                    )
                ))
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
            var exception = ConstructTwitterException(HttpStatusCode.NotFound);

            _client
                .Setup(c => c.GetUserAsync(id))
                .Throws(exception)
            ;

            await Assert.ThrowsAsync<ContactNotFoundException<TwitterContact>>(
                () => _fetcher.FetchAsync(id)
            );
        }

        [Fact]
        public async void TestFetchUnknownErrorAsync()
        {
            const ulong id = 123456789UL;
            var exception = ConstructTwitterException();

            _client
                .Setup(c => c.GetUserAsync(id))
                .Throws(exception)
            ;

            Assert.Equal(
                exception,
                await Assert.ThrowsAsync<TwitterException>(
                    () => _fetcher.FetchAsync(id)
                )
            );
        }

        private static TwitterException ConstructTwitterException(HttpStatusCode statusCode = default)
        {
            return (TwitterException) typeof(TwitterException)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .First()
                .Invoke(new object[]
                {
                    statusCode,
                    default,
                    new[] { new Error() },
                    default,
                    default,
                    default
                })
            ;
        }
    }
}
