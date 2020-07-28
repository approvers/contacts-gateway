using ContactsGateway.Models;
using ContactsGateway.Services;
using Moq;
using Xunit;

namespace ContactsGateway.Tests.Services
{
    public class EntryFactoryTest
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public interface IDummy
        {
        }
        
        private readonly EntryFactory<IDummy> _factory;

        public EntryFactoryTest()
        {
            _factory = new EntryFactory<IDummy>();
        }

        [Fact]
        public void TestCreate()
        {
            var item = new Mock<IDummy>().Object;
            var entry = _factory.Create(item);

            Assert.IsType<Entry<IDummy>>(entry);
            Assert.IsAssignableFrom<IDummy>(entry.Item);
            Assert.Equal(item, entry.Item);
        }
    }
}
