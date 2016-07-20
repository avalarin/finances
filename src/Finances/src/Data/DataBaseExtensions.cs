using System;
using System.Linq;
using System.Threading.Tasks;
using Finances.Models;
using Microsoft.EntityFrameworkCore;

namespace Finances.Data {
    public static class DataBaseExtensions {
        public static async Task<Session> GetActualById(this IQueryable<Session> set, string id) {
            Guid guid;
            if (!Guid.TryParse(id, out guid)) {
                return null;
            }
            return await GetActualById(set, guid);
        }

        public static async Task<Session> GetActualById(this IQueryable<Session> set, Guid id) {
            var session = await GetById(set, id);
            if (session == null) {
                return null;
            }
            if (session.ExpiresAt <= DateTime.Now || session.ClosedAt.HasValue) {
                return null;
            }
            return session;
        }

        public static Task<Session> GetById(this IQueryable<Session> set, Guid id) {
            return set.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
        }

        public static Task<Currency> GetGlobalByCode(this IQueryable<Currency> set, string code) {
            return set.FirstOrDefaultAsync(c => c.Book == null && c.Code == code);
        }

        public static Task<Unit> GetGlobalByCode(this IQueryable<Unit> set, string code) {
            return set.FirstOrDefaultAsync(c => c.Book == null && c.Code == code);
        }
    }
}