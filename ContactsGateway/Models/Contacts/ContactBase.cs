namespace ContactsGateway.Models.Contacts
{
    public abstract class ContactBase : IContact
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        protected ContactBase()
        {
        }

        protected ContactBase(ulong id, string name, string url)
        {
            Id = id;
            Name = name;
            Url = url;
        }
    }
}
