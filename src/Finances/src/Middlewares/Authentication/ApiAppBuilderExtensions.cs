using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Finances.Middlewares.Authentication {
    public static class ApiAppBuilderExtensions {
        public static IApplicationBuilder UseApiAuthentication(this IApplicationBuilder app) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ApiAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseApiAuthentication(this IApplicationBuilder app, ApiAuthenticationOptions options) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<ApiAuthenticationMiddleware>(Options.Create(options));
        }
    }
}