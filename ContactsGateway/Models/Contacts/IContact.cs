namespace ContactsGateway.Models.Contacts
{
    public interface IContact
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
