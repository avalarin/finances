using System.Threading.Tasks;
using Finances.Models;

namespace Finances.Services.Sessions {
    public interface ISessionStore {

        Task<Session> GetSessionById(string id);

        Task<Session> CreateSessionForUser(string user, bool persistent);

    }
}