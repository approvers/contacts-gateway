using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Caching.Cache;
using ContactsGateway.Services.Caching.Storage;
using ContactsGateway.Services.Clients;
using ContactsGateway.Services.Fetchers;
using CoreTweet;
using Discord.Rest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ContactsGateway
{
    public class Startup
    {
        public Startup(IHostEnvironment environment)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<ITwitterClient>(provider => new TwitterClient(
                OAuth2.GetToken(
                    Configuration["Twitter:ConsumerKey"],
                    Configuration["Twitter:ConsumerSecret"]
                )
            ));

            services.AddScoped<IGitHubClient>(provider => new GitHubClient(
                new HttpClient(),
                Configuration["GitHub:Token"]
            ));

            services.AddScoped<IDiscordClient>(provider =>
            {
                var client = new DiscordRestClient();

                client
                    .LoginAsync(Discord.TokenType.Bot, Configuration["Discord:Token"])
                    .Wait();

                return new DiscordClient(client);
            });

            services.AddScoped<TwitterFetcher>();
            services.AddScoped<DiscordFetcher>();
            services.AddScoped<GitHubFetcher>();

            services.AddTransient<TimeoutCache<TwitterContact>.EntryFactory>();
            services.AddTransient<TimeoutCache<DiscordContact>.EntryFactory>();
            services.AddTransient<TimeoutCache<GitHubContact>.EntryFactory>();

            services.AddSingleton<ICacheStorage<TimeoutCache<TwitterContact>.Entry, TwitterContact>, InMemoryCacheStorage<TimeoutCache<TwitterContact>.Entry, TwitterContact>>();
            services.AddSingleton<ICacheStorage<TimeoutCache<GitHubContact>.Entry, GitHubContact>, InMemoryCacheStorage<TimeoutCache<GitHubContact>.Entry, GitHubContact>>();
            services.AddSingleton<ICacheStorage<TimeoutCache<DiscordContact>.Entry, DiscordContact>, InMemoryCacheStorage<TimeoutCache<DiscordContact>.Entry, DiscordContact>>();

            services.AddSingleton<ICache<TwitterContact>, TimeoutCache<TwitterContact>>(provider => new TimeoutCache<TwitterContact>(
                provider.GetService<ICacheStorage<TimeoutCache<TwitterContact>.Entry, TwitterContact>>(),
                provider.GetService<TimeoutCache<TwitterContact>.EntryFactory>(),
                TimeSpan.FromMilliseconds(int.Parse(Configuration["Cache:Timeout"])))
            );
            
            services.AddSingleton<ICache<GitHubContact>, TimeoutCache<GitHubContact>>(provider => new TimeoutCache<GitHubContact>(
                provider.GetService<ICacheStorage<TimeoutCache<GitHubContact>.Entry, GitHubContact>>(),
                provider.GetService<TimeoutCache<GitHubContact>.EntryFactory>(),
                TimeSpan.FromMilliseconds(int.Parse(Configuration["Cache:Timeout"])))
            );
            
            services.AddSingleton<ICache<DiscordContact>, TimeoutCache<DiscordContact>>(provider => new TimeoutCache<DiscordContact>(
                provider.GetService<ICacheStorage<TimeoutCache<DiscordContact>.Entry, DiscordContact>>(),
                provider.GetService<TimeoutCache<DiscordContact>.EntryFactory>(),
                TimeSpan.FromMilliseconds(int.Parse(Configuration["Cache:Timeout"])))
            );

            services.AddScoped<IFetcher<TwitterContact>, CachedFetcher<TwitterContact>>(provider => new CachedFetcher<TwitterContact>(
                provider.GetService<ICache<TwitterContact>>(),
                provider.GetService<TwitterFetcher>()
            ));
            
            services.AddScoped<IFetcher<GitHubContact>, CachedFetcher<GitHubContact>>(provider => new CachedFetcher<GitHubContact>(
                provider.GetService<ICache<GitHubContact>>(),
                provider.GetService<GitHubFetcher>()
            ));
            
            services.AddScoped<IFetcher<DiscordContact>, CachedFetcher<DiscordContact>>(provider => new CachedFetcher<DiscordContact>(
                provider.GetService<ICache<DiscordContact>>(),
                provider.GetService<DiscordFetcher>()
            ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
