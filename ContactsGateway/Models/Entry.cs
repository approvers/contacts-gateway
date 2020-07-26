namespace ContactsGateway.Models
{
    public interface IEntry<out T>
    {
        T Item { get; }
    }

    public class Entry<T> : IEntry<T>
    {
        public T Item { get; }

        public Entry(T item)
        {
            Item = item;
        }
    }
}
