using System;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Caching.Cache;
using ContactsGateway.Services.Caching.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsGateway.DependencyInjection
{
    public static class TimeoutCacheExtension
    {
        public static ContactServiceBuilder<TContact> WithTimeoutCache<TContact, TCacheStorage>(
            this ContactServiceBuilder<TContact> builder,
            TimeSpan timeout
        )
            where TContact : class, IContact
            where TCacheStorage : class, ICacheStorage<TimeoutCache<TContact>.Entry, TContact>
        {
            return builder.WithCache<
                TContact,
                TCacheStorage,
                TimeoutCache<TContact>.Entry,
                TimeoutCache<TContact>.EntryFactory
            >(provider => new TimeoutCache<TContact>(
                provider.GetService<ICacheStorage<TimeoutCache<TContact>.Entry, TContact>>(),
                provider.GetService<TimeoutCache<TContact>.EntryFactory>(),
                timeout
            ));
        }
    }
}
