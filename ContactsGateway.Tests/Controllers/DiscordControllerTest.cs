using System;
using System.Threading.Tasks;
using ContactsGateway.Controllers;
using ContactsGateway.Exceptions;
using ContactsGateway.Models;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Fetchers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ContactsGateway.Tests.Controllers
{
    public class DiscordControllerTest
    {
        private readonly Mock<IFetcher<DiscordContact>> _fetcher;
        private readonly DiscordController _controller;

        public DiscordControllerTest()
        {
            _fetcher = new Mock<IFetcher<DiscordContact>>();
            _controller = new DiscordController(
                _fetcher.Object
            );
        }

        [Fact]
        public async void TestGet()
        {
            const ulong id = 123456789UL;
            var entry = new Mock<IEntry<DiscordContact>>();

            _fetcher
                .Setup(f => f.FetchAsync(id))
                .Returns(Task.FromResult(entry.Object))
            ;

            var result = await _controller.Get(id);
            
            Assert.IsType<JsonResult>(result);
            Assert.Equal(entry.Object, ((JsonResult) result).Value);
        }

        [Fact]
        public void TestGetNotFound()
        {
            const ulong id = 123456789UL;
            var exception = new Mock<Exception>();

            _fetcher
                .Setup(f => f.FetchAsync(id))
                .Throws(new ContactNotFoundException<DiscordContact>(exception.Object))
            ;

            Assert.ThrowsAsync<ContactNotFoundException<DiscordContact>>(async () =>
            {
                await _controller.Get(id);
            });
        }
    }
}
