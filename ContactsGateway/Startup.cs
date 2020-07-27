using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ContactsGateway.DependencyInjection;
using ContactsGateway.DependencyInjection.ContactServices;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services;
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
            var timeout = TimeSpan.FromMilliseconds(Configuration.GetValue<ulong>("Cache:Timeout"));
            
            services
                .AddTwitterServices(Configuration["Twitter:ConsumerKey"], Configuration["Twitter:ConsumerSecret"])
                    .WithTimeoutCache<TwitterContact, InMemoryCacheStorage<TimeoutCache<TwitterContact>.Entry, TwitterContact>>(timeout)
                    .Build()
                .AddGitHubServices(Configuration["GitHub:Token"])
                    .WithTimeoutCache<GitHubContact, InMemoryCacheStorage<TimeoutCache<GitHubContact>.Entry, GitHubContact>>(timeout)
                    .Build()
                .AddDiscordServices(Configuration["Discord:Token"])
                    .WithTimeoutCache<DiscordContact, InMemoryCacheStorage<TimeoutCache<DiscordContact>.Entry, DiscordContact>>(timeout)
                    .Build()
                .AddControllers()
            ;
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
