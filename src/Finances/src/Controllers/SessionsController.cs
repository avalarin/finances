using System.Threading.Tasks;
using Finances.Models;
using Finances.Models.SessionsModels;
using Finances.Services.Sessions;
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
        public async Task<SessionResponseModel> Get() {
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