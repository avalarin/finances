using System.Threading.Tasks;
using Finances.Models;
using Finances.Services.Sessions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace Finances.Middlewares.Authentication {
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions> {

        private readonly ISessionStore _sessionStore;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TokenAuthenticationHandler(ISessionStore sessionStore, SignInManager<ApplicationUser> signInManager) {
            _sessionStore = sessionStore;
            _signInManager = signInManager;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync() {
            StringValues values;
            Request.Headers.TryGetValue(Options.HeaderName, out values);

            if (values.Count == 0) {
                return AuthenticateResult.Skip();
            }

            if (values.Count > 1) {
                return AuthenticateResult.Fail("Too many headers");
            }

            var headerValue = values[0];

            var session = await _sessionStore.GetSessionById(headerValue);

            if (session == null) {
                return AuthenticateResult.Skip();
            }

            var principal = await _signInManager.CreateUserPrincipalAsync(session.User);
            var authProperties = new AuthenticationProperties();
            
            var ticket = new AuthenticationTicket(principal, authProperties, Options.AuthenticationScheme);
            return AuthenticateResult.Success(ticket);
        }
    }
}