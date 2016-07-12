using Microsoft.AspNetCore.Builder;

namespace Finances.Middlewares.Authentication {
    public class ApiAuthenticationOptions : AuthenticationOptions {

        public string HeaderName { get; set; }

    }
}