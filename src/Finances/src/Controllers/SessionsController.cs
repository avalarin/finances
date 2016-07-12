using System.Threading.Tasks;
using Finances.Models;
using Finances.Models.SessionsModels;
using Finances.Services.Sessions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Finances.Controllers {
    [Route("api/sessions")]
    public class SessionsController : Controller {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISessionStore _sessionStore;
        private readonly ILogger _logger;

        public SessionsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ISessionStore sessionStore,
            ILogger<SessionsController> logger) {

            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _sessionStore = sessionStore;
        }

        [HttpPost]
        public async Task<CreateSessionResponseModel> Post([FromBody] CreateSessionRequestModel model) {
            if (!ModelState.IsValid) {
                return new CreateSessionResponseModel(CreateSessionStatus.Failed);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) {
                return new CreateSessionResponseModel(CreateSessionStatus.InvalidNameOrPassword);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password)) {
                return new CreateSessionResponseModel(CreateSessionStatus.InvalidNameOrPassword);
            }

            var session = await _sessionStore.CreateSessionForUser(model.UserName, true);

            return new CreateSessionResponseModel(session);
        }

    }
}