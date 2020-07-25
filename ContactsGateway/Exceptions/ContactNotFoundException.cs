using System;
using ContactsGateway.Models.Contacts;

namespace ContactsGateway.Exceptions
{
    public class ContactNotFoundException<T> : Exception where T : IContact
    {
        public ContactNotFoundException(Exception innerException)
            : base("No contact found with the ID.", innerException)
        {
        }
    }
}
