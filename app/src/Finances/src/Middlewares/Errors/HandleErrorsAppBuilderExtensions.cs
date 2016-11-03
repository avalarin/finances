using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Finances.Middlewares.Errors {
    public static class HandleErrorsAppBuilderExtensions {
        public static IApplicationBuilder UseHandleErrorsMiddleware(this IApplicationBuilder app) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<HandleErrorsMiddleware>();
        }

        public static IApplicationBuilder UseHandleErrorsMiddleware(this IApplicationBuilder app, HandleErrorsMiddlewareOptions options) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<HandleErrorsMiddleware>(Options.Create(options));
        }
    }
}
