using System.Net.Http;
using System.Threading.Tasks;
using Finances.Models;
using Microsoft.AspNetCore.Http;

namespace Finances.Web.Authentication {
    public interface ISessionAccessor {
        Task<Session> GetSession(HttpContext context);
    }
}