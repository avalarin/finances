using System;
using System.Threading.Tasks;
using Finances.Models;
using Finances.Services.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace Finances.Web.Authentication {
    class SessionAccessor : ISessionAccessor {
        private readonly ISessionStore _sessionStore;

        public SessionAccessor(ISessionStore sessionStore) {
            _sessionStore = sessionStore;
        }

        public async Task<Session> GetSession(HttpContext context) {
            var authContext = new AuthenticateContext(Constants.TokenAuthenticationScheme);
            await context.Authentication.AuthenticateAsync(authContext);

            if (authContext.Properties == null || !authContext.Properties.ContainsKey(Constants.SessionIdItemKey)) {
                throw new InvalidOperationException("Invalid auth context");
            }

            var sessionId = authContext.Properties[Constants.SessionIdItemKey];

            return await _sessionStore.GetSessionById(sessionId);
        }
    }
}