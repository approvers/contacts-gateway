using System;
using System.Linq;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services;
using ContactsGateway.Services.Caching;
using ContactsGateway.Services.Caching.Cache;
using ContactsGateway.Services.Caching.Storage;
using ContactsGateway.Services.Fetchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ContactsGateway.DependencyInjection
{
    public static class ContactServiceExtension
    {
        public static ContactServiceBuilder<TContact> AddContactService<
            TContact,
            TClient,
            TFetcher
        >(
            this IServiceCollection services,
            Func<IServiceProvider, TClient> clientFactory
        ) 
            where TContact : IContact
            where TClient : class
            where TFetcher : class, IFetcher<TContact>
        {
            var builder = new ContactServiceBuilder<TContact>(services);

            builder.Services
                .AddScoped(clientFactory)
                .AddScoped<IFetcher<TContact>, TFetcher>()
                .AddScoped<IEntryFactory<TContact>, EntryFactory<TContact>>()
            ;

            return builder;
        }

        public static ContactServiceBuilder<TContact> WithCache<
            TContact,
            TCacheStorage, 
            TEntry,
            TEntryFactory
        >(
            this ContactServiceBuilder<TContact> builder,
            Func<IServiceProvider, ICache<TContact>> cacheFactory
        )
            where TContact : IContact
            where TCacheStorage : class, ICacheStorage<TEntry, TContact>
            where TEntry : ICacheEntry<TContact>
            where TEntryFactory : class, ICacheEntryFactory<TEntry, TContact>
        {
            var provider = builder.Services.BuildServiceProvider();
            
            builder.Services
                .AddTransient<TEntryFactory>()
                .AddSingleton<ICacheStorage<TEntry, TContact>, TCacheStorage>()
                .AddScoped(cacheFactory)
                .RemoveAll<IFetcher<TContact>>()
                .AddScoped<IFetcher<TContact>, CachedFetcher<TContact>>(p => new CachedFetcher<TContact>(
                    p.GetService<ICache<TContact>>(),
                    provider.GetService<IFetcher<TContact>>()
                ))
            ;

            return builder;
        }
    }
}
