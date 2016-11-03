using Microsoft.AspNetCore.Builder;

namespace Finances.Middlewares.Authentication {
    public class TokenAuthenticationOptions : AuthenticationOptions {

        public string HeaderName { get; set; }

    }
}