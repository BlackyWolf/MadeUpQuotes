using MadeUpQuotes.Web.Models.AppSettings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadeUpQuotes.Web.Extensions
{
    public static class Auth0extensions
    {
        public static IServiceCollection ConfigureAuth0(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect("Auth0", options =>
                {
                    var auth0 = new Auth0();

                    configuration.Bind("Auth0", auth0);

                    options.Authority = $"https://{auth0.Domain}";
                    options.CallbackPath = new PathString("/callback");
                    options.ClaimsIssuer = "Auth0";
                    options.ClientId = auth0.ClientId;
                    options.ClientSecret = auth0.ClientSecret;
                    options.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProviderForSignOut = context =>
                        {
                            var logoutUri = $"https://{auth0.Domain}/v2/logout?client_id={auth0.ClientId}";

                            var postLogoutUri = context.Properties.RedirectUri;

                            if (!string.IsNullOrWhiteSpace(postLogoutUri))
                            {
                                if (postLogoutUri.StartsWith("/"))
                                {
                                    HttpRequest request = context.Request;

                                    postLogoutUri = $"{request.Scheme}://{request.Host}{request.PathBase}{postLogoutUri}";
                                }

                                logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                            }

                            context.Response.Redirect(logoutUri);
                            context.HandleResponse();

                            return Task.CompletedTask;
                        }
                    };
                    options.ResponseType = "code";
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                });

            return services;
        }
    }
}
