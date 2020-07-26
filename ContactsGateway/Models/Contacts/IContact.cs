using ContactsGateway.Services.Caching;

namespace ContactsGateway.Models.Contacts
{
    public interface IContact : ICacheable
    {
        string Name { get; set; }
        string Url { get; set; }
    }
}
