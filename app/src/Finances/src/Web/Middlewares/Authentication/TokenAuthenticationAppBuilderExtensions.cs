using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Finances.Web.Middlewares.Authentication {
    public static class TokenAuthenticationAppBuilderExtensions {
        public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder app) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<TokenAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder app, TokenAuthenticationOptions options) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<TokenAuthenticationMiddleware>(Options.Create(options));
        }
    }
}