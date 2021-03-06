﻿namespace ContactsGateway.Models.Contacts
{
    public class DiscordContact : ContactBase
    {
        public string Tag { get; }

        public DiscordContact(ulong id, string name, string tag, string url) : base(id, name, url)
        {
            Tag = tag;
        }
    }
}
