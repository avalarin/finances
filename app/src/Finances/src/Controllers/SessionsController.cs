using System.Threading.Tasks;
using Finances.Exceptions;
using Finances.Models;
using Finances.Models.Responses;
using Finances.Models.Requests;
using Finances.Services.Sessions;
using Finances.Web;
using Finances.Web.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Finances.Controllers {
    [Route("api/sessions")]
    public class SessionsController : Controller {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISessionStore _sessionStore;
        private readonly ISessionAccessor _sessionAccessor;

        public SessionsController(UserManager<ApplicationUser> userManager,
            ISessionStore sessionStore,
            ISessionAccessor sessionAccessor) {

            _userManager = userManager;
            _sessionAccessor = sessionAccessor;
            _sessionStore = sessionStore;
        }

        [Route("current")]
        public async Task<Response> Get() {
            if (!User.Identity.IsAuthenticated) {
                return new Response(ApplicationError.Unauthenticated);
            }
            var dbSession = await _sessionAccessor.GetSession(HttpContext);
            var session = new SessionResponseModel(dbSession);
            return new PayloadResponse(new { session });
        }

        [HttpPost]
        public async Task<Response> Post([FromBody] CreateSessionRequestModel model) {
            if (!ModelState.IsValid) {
                return new Response(ApplicationError.Failed);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) {
                return new Response(ApplicationError.InvalidNameOrPassword);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password)) {
                return new Response(ApplicationError.InvalidNameOrPassword);
            }

            var dbSession = await _sessionStore.CreateSessionForUser(model.UserName, true);
            var session = new SessionResponseModel(dbSession);
            return new PayloadResponse(new { session });
        }

    }
}