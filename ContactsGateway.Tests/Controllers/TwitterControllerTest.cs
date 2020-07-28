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
    public class TwitterControllerTest
    {
        private readonly Mock<IFetcher<TwitterContact>> _fetcher;
        private readonly TwitterController _controller;

        public TwitterControllerTest()
        {
            _fetcher = new Mock<IFetcher<TwitterContact>>();
            _controller = new TwitterController(
                _fetcher.Object
            );
        }

        [Fact]
        public async void TestGet()
        {
            const ulong id = 123456789UL;
            var entry = new Mock<IEntry<TwitterContact>>();

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
                .Throws(new ContactNotFoundException<TwitterContact>(exception.Object))
            ;

            Assert.ThrowsAsync<ContactNotFoundException<TwitterContact>>(async () =>
            {
                await _controller.Get(id);
            });
        }
    }
}
