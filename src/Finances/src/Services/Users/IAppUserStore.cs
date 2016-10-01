using System.Threading.Tasks;
using Finances.Models;

namespace Finances.Services.Users {
    public interface IAppUserStore {
        Task<ApplicationUser> GetUser(string userName);
    }
}