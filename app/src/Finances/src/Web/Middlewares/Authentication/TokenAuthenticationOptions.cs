using Microsoft.AspNetCore.Builder;

namespace Finances.Web.Middlewares.Authentication {
    public class TokenAuthenticationOptions : AuthenticationOptions {

        public string HeaderName { get; set; }

    }
}