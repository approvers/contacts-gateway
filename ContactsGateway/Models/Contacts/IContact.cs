namespace ContactsGateway.Models.Contacts
{
    public interface IContact
    {
        ulong Id { get; set; }
        string Name { get; set; }
        string Url { get; set; }
    }
}
