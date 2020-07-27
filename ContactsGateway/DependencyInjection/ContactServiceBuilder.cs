using Microsoft.Extensions.DependencyInjection;

namespace ContactsGateway.DependencyInjection
{
    public class ContactServiceBuilder<T>
    {
        internal IServiceCollection Services { get; }

        public ContactServiceBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Build()
        {
            return Services;
        }
    }
}
