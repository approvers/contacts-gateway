using Newtonsoft.Json;

namespace ContactsGateway.Models.Contacts
{
    public abstract class ContactBase : IContact
    {
        [JsonProperty]
        public ulong Id { get; set; }
        
        [JsonProperty]
        public string Name { get; set; }
        
        [JsonProperty]
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
