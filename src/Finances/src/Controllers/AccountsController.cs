using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Controllers {
    [Authorize]
    [Route("api/accounts")]
    public class AccountsController : Controller {

        private readonly ApplicationDbContext _database;
        private readonly ILogger _logger;

        public AccountsController(ApplicationDbContext database, ILogger<AccountsController> logger) {
            _database = database;
            _logger = logger;
        }

        [HttpGet]
        public async Task<Models.AccountsModels.User[]> Get() {
            var users = await _database.Users.ToArrayAsync();

            return users.Select(Models.AccountsModels.User.FromApplicationUser).ToArray();
        } 
    }
}