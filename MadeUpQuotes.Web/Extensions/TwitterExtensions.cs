using MadeUpQuotes.Web.Models.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;

namespace MadeUpQuotes.Web.Extensions
{
    public static class TwitterExtensions
    {
        public static IServiceCollection AddTwitter(this IServiceCollection services, IConfiguration configuration)
        {
            var twitter = new Twitter();

            configuration.Bind("Twitter", twitter);

            services.AddScoped<ITwitterClient>(_ => new TwitterClient(
                twitter.ApiKey,
                twitter.ApiSecret,
                twitter.AccessToken,
                twitter.AccessTokenSecret
            ));

            return services;
        }
    }
}