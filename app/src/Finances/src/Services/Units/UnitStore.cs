using System;
using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Exceptions;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Users;
using Finances.Utils.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Units {
    public class UnitStore : IUnitStore {
        private ApplicationDbContext DataBase { get; }
        private IBookStore BookStore { get; }
        private IAppUserStore UserStore { get; }
        private ILogger<UnitStore> Logger { get; }

        public UnitStore(ApplicationDbContext db, IBookStore bookStore, IAppUserStore userStore, ILogger<UnitStore> logger) {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (bookStore == null) throw new ArgumentNullException(nameof(bookStore));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            DataBase = db;
            BookStore = bookStore;
            UserStore = userStore;
            Logger = logger;
        }

        public async Task<Unit> CreateUnit(string code, int decimals, string text, int bookId, string userName) {
            var user = await UserStore.GetUser(userName);
            if (user == null) {
                Logger.LogAppErrorAndThrow($"User '{userName}' not found", ApplicationError.UserNotFound);
            }

            var bookUser = await BookStore.GetUserBook(userName, bookId);
            if (bookUser == null) {
                Logger.LogAppErrorAndThrow($"Cannot create unit: book #{bookId} not found or user has no access to this book", ApplicationError.BookNotFound);
            }

            if (bookUser.Role < BookUserRole.Member) {
                Logger.LogAppErrorAndThrow($"Cannot create unit: permission denied for user {userName}", ApplicationError.PermissionDenied);
            }

            var unit = new Unit() {
                Book = bookUser.Book,
                Code = code,
                Decimals = decimals,
                Text = text
            };

            DataBase.Units.Add(unit);
            await DataBase.SaveChangesAsync();

            Logger.LogInformation($"Unit #${unit.Id} has been created by user {userName}");

            return unit;
        }

        public Task<Unit[]> GetUnits(int book) {
            return DataBase.Units.Where(u => !u.BookId.HasValue || u.BookId == book).ToArrayAsync();
        }

        public Task<Unit> GetUnit(int unitId, int bookId) {
            return DataBase.Units.FirstOrDefaultAsync(u => (!u.BookId.HasValue || u.BookId == bookId) && u.Id == unitId);
        }

    }
}