using System.Threading.Tasks;
using Finances.Models;
using Finances.Services.Sessions;
using Finances.Web.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace Finances.Web.Middlewares.Authentication {
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions> {
        private readonly ISessionStore _sessionStore;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private Task<AuthenticateResult> _readHeaderTask;

        public TokenAuthenticationHandler(ISessionStore sessionStore, SignInManager<ApplicationUser> signInManager) {
            _sessionStore = sessionStore;
            _signInManager = signInManager;
        }


        private Task<AuthenticateResult> EnsureTicket() {
            if (_readHeaderTask == null) {
                _readHeaderTask = ReadTicket();
            }
            return _readHeaderTask;
        }

        private async Task<AuthenticateResult> ReadTicket() {
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
            authProperties.Items[Constants.SessionIdItemKey] = headerValue;

            var ticket = new AuthenticationTicket(principal, authProperties, Options.AuthenticationScheme);
            return AuthenticateResult.Success(ticket);
        }


        protected async override Task<AuthenticateResult> HandleAuthenticateAsync() {
            var ticket = await EnsureTicket();

            return ticket;
        }
    }
}