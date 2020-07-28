using System.Threading.Tasks;
using ContactsGateway.Exceptions;
using ContactsGateway.Models;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services;
using ContactsGateway.Services.Fetchers;
using Discord;
using Moq;
using Xunit;
using IDiscordClient = ContactsGateway.Services.Clients.IDiscordClient;

namespace ContactsGateway.Tests.Services.Fetchers
{
    public class DiscordFetcherTest
    {
        private readonly Mock<IDiscordClient> _client;
        private readonly Mock<IEntryFactory<DiscordContact>> _entryFactory;
        private readonly DiscordFetcher _fetcher;

        public DiscordFetcherTest()
        {
            _client = new Mock<IDiscordClient>();
            _entryFactory = new Mock<IEntryFactory<DiscordContact>>();
            _fetcher = new DiscordFetcher(
                _client.Object,
                _entryFactory.Object
            );
        }
        
        [Fact]
        public async void TestFetchAsync()
        {
            const ulong id = 123456789UL;
            const string username = "foo";
            const string discriminator = "bar";

            var entry = new Mock<IEntry<DiscordContact>>();
            var user = new Mock<IUser>();

            user.Setup(u => u.Id).Returns(id);
            user.Setup(u => u.Username).Returns(username);
            user.Setup(u => u.Discriminator).Returns(discriminator);

            _client
                .Setup(c => c.GetUserAsync(id))
                .Returns(Task.FromResult(user.Object))
            ;

            _entryFactory
                .Setup(f => f.Create(
                    It.Is<DiscordContact>(
                        contact => contact.Id == id
                            && contact.Name == username 
                            && contact.Tag == discriminator
                            && contact.Url == null
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

            _client
                .Setup(c => c.GetUserAsync(id))
                .Returns(Task.FromResult((IUser) null))
            ;

            await Assert.ThrowsAsync<ContactNotFoundException<DiscordContact>>(
                () => _fetcher.FetchAsync(id)
            );
        }
    }
}
