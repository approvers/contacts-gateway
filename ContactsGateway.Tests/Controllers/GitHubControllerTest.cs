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
    public class GitHubControllerTest
    {
        private readonly Mock<IFetcher<GitHubContact>> _fetcher;
        private readonly GitHubController _controller;

        public GitHubControllerTest()
        {
            _fetcher = new Mock<IFetcher<GitHubContact>>();
            _controller = new GitHubController(
                _fetcher.Object
            );
        }

        [Fact]
        public async void TestGet()
        {
            const ulong id = 123456789UL;
            var entry = new Mock<IEntry<GitHubContact>>();

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
                .Throws(new ContactNotFoundException<GitHubContact>(exception.Object))
            ;

            Assert.ThrowsAsync<ContactNotFoundException<GitHubContact>>(async () =>
            {
                await _controller.Get(id);
            });
        }
    }
}
