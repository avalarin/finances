using System.Threading.Tasks;
using Finances.Models;
using Finances.Services.Sessions;
using Finances.Web.Authentication;
using Finances.WebModels.SessionsModels;
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
        public async Task<SessionResponseModel> Get() {
            if (!User.Identity.IsAuthenticated) {
                return new SessionResponseModel(CreateSessionStatus.Unauthenticated);
            }
            var session = await _sessionAccessor.GetSession(HttpContext);
            return new SessionResponseModel(session);
        }

        [HttpPost]
        public async Task<SessionResponseModel> Post([FromBody] CreateSessionRequestModel model) {
            if (!ModelState.IsValid) {
                return new SessionResponseModel(CreateSessionStatus.Failed);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) {
                return new SessionResponseModel(CreateSessionStatus.InvalidNameOrPassword);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password)) {
                return new SessionResponseModel(CreateSessionStatus.InvalidNameOrPassword);
            }

            var session = await _sessionStore.CreateSessionForUser(model.UserName, true);

            return new SessionResponseModel(session);
        }

    }
}