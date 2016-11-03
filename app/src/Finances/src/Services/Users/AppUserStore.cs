using System.Threading;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Finances.Services.Users {
    public class AppUserStore : IAppUserStore {

        private IUserStore<ApplicationUser> InternalStore { get; }

        public AppUserStore(ApplicationDbContext db) {
            InternalStore = new UserStore<ApplicationUser>(db);
        }

        public Task<ApplicationUser> GetUser(string userName) {
            return InternalStore.FindByNameAsync(userName.ToUpperInvariant(), CancellationToken.None);
        }

    }
}
