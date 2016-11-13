using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Finances.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Finances.Web.Middlewares.Errors {
    public class HandleErrorsMiddleware {
        private readonly RequestDelegate _next;
        private readonly HandleErrorsMiddlewareOptions _options;
        private readonly ILogger _logger;

        public HandleErrorsMiddleware(RequestDelegate next, IOptions<HandleErrorsMiddlewareOptions> options,
            ILogger<HandleErrorsMiddlewareOptions> logger) {
            if (next == null) throw new ArgumentNullException(nameof(next));
            if (options == null) throw new ArgumentNullException(nameof(options));
            _options = options.Value;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next.Invoke(context);
            }
            catch (Exception e) {
                HandleException(context, e);
            }
        }

        private void HandleException(HttpContext context, Exception exception) {
            _logger.LogError(0, exception, "An unhandled exception has occurred while executing the request");

            if (context.Response.HasStarted) {
                _logger.LogWarning("The response has already started, the error page middleware will not be executed.");
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
            try {
                context.Response.Clear();
                context.Response.StatusCode = 500;

                DisplayException(context, exception);
            }
            catch (Exception displayException) {
                _logger.LogError(0, displayException,
                    "An exception was thrown attempting to display the error response.");
            }
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        private void DisplayException(HttpContext context, Exception exception) {
            using (var writer = new StreamWriter(context.Response.Body))
            using (var json = new JsonTextWriter(writer)) {
                json.WriteStartObject();
                
                var appException = exception as ApplicationException;
                if (appException != null) {
                    json.WritePropertyName("status");
                    json.WriteValue(appException.Error.Name);
                    json.WritePropertyName("message");
                    json.WriteValue(appException.Error.Message);
                } else {
                    json.WritePropertyName("status");
                    json.WriteValue("InternalServerError");
                }
                
                if (_options.EnableStackTrace) {
                    json.WritePropertyName("details");
                    json.WriteValue(exception.ToString());
                }
                json.WriteEndObject();
            }
        }
    }
}
