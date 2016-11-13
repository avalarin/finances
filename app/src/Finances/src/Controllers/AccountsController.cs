using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Models.Responses;
using Finances.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Controllers {
    [Route("api/accounts")]
    [Authorize]
    public class AccountsController : Controller {

        private readonly ApplicationDbContext _database;
        private readonly ILogger _logger;

        public AccountsController(ApplicationDbContext database, ILogger<AccountsController> logger) {
            _database = database;
            _logger = logger;
        } 

        [HttpGet]
        public async Task<Response> Get() {
            var users = (await _database.Users.ToArrayAsync())
                                              .Select(u => new UserResponseModel(u));
            return new PayloadResponse(new { users });
        } 
    }
}