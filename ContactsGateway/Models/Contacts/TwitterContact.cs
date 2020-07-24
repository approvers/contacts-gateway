namespace ContactsGateway.Models.Contacts
{
    public class TwitterContact : ContactBase
    {
        public string ScreenName { get; }
        
        public TwitterContact(ulong id, string name, string screenName, string url) : base(id, name, url)
        {
            ScreenName = screenName;
        }
    }
}
