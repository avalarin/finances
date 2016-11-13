using System.Text.Encodings.Web;
using Finances.Models;
using Finances.Services.Sessions;
using Finances.Web.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Finances.Web.Middlewares.Authentication {
    public class TokenAuthenticationMiddleware : AuthenticationMiddleware<TokenAuthenticationOptions> {
        private readonly ISessionStore _sessionStore;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TokenAuthenticationMiddleware(RequestDelegate next, 
                                           IOptions<TokenAuthenticationOptions> options, 
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
                Options.AuthenticationScheme = Constants.TokenAuthenticationScheme;
            }
        }

        protected override AuthenticationHandler<TokenAuthenticationOptions> CreateHandler() {
            return new TokenAuthenticationHandler(_sessionStore, _signInManager);
        }
    }
}
