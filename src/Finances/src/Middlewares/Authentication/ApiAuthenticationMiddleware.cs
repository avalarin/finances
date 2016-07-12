using System.Text.Encodings.Web;
using Finances.Models;
using Finances.Services.Sessions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Finances.Middlewares.Authentication {
    public class ApiAuthenticationMiddleware : AuthenticationMiddleware<ApiAuthenticationOptions> {
        private readonly ISessionStore _sessionStore;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApiAuthenticationMiddleware(RequestDelegate next, 
                                           IOptions<ApiAuthenticationOptions> options, 
                                           ILoggerFactory loggerFactory,
                                           ISessionStore sessionStore,
                                           SignInManager<ApplicationUser> signInManager,
                                           UrlEncoder encoder) : base(next, options, loggerFactory, encoder) {
            _sessionStore = sessionStore;
            _signInManager = signInManager;

            if (string.IsNullOrWhiteSpace(Options.HeaderName)) {
                Options.HeaderName = "X-Auth";
            }

            if (string.IsNullOrWhiteSpace(Options.AuthenticationScheme)) {
                Options.AuthenticationScheme = "Headers";
            }
        }

        protected override AuthenticationHandler<ApiAuthenticationOptions> CreateHandler() {
            return new ApiAuthenticationHandler(_sessionStore, _signInManager);
        }
    }
}
