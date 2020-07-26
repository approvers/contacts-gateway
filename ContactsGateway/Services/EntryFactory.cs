using ContactsGateway.Models;

namespace ContactsGateway.Services
{
    public interface IEntryFactory<T>
    {
        IEntry<T> Create(T item);
    }
    
    public class EntryFactory<T> : IEntryFactory<T>
    {
        public IEntry<T> Create(T item)
        {
            return new Entry<T>(item);
        }
    }
}
