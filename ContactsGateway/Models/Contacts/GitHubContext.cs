﻿namespace ContactsGateway.Models.Contacts
{
    public class GitHubContact : ContactBase
    {
        public string Login { get; set; }

        public GitHubContact() : base()
        {
            
        }
        
        public GitHubContact(ulong id, string name, string login, string url) : base(id, name, url)
        {
            Login = login;
        }
    }
}
