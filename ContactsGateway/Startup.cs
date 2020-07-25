using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Clients;
using ContactsGateway.Services.Fetchers;
using CoreTweet;
using Discord;
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

            services.AddScoped(provider =>
            {
                var client = new DiscordRestClient();

                client
                    .LoginAsync(TokenType.Bot, Configuration["Discord:Token"])
                    .Wait();

                return client;
            });

            services.AddScoped<TwitterFetcher>();
            services.AddScoped<GitHubFetcher>();
            services.AddScoped<DiscordFetcher>();
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
