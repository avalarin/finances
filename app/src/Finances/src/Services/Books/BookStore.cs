using System;
using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Exceptions;
using Finances.Models;
using Finances.Services.Users;
using Finances.Utils.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Books {
    public class BookStore : IBookStore {

        private ApplicationDbContext DataBase { get; }
        private IAppUserStore UserStore { get; }
        private ILogger<BookStore> Logger { get; }

        public BookStore(ApplicationDbContext db, IAppUserStore userStore, ILogger<BookStore> logger) {
            DataBase = db;
            UserStore = userStore;
            Logger = logger;
        }

        public Task<BookUser[]> GetUserBooks(string userName) {
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            return DataBase.BooksUsers.Include(b => b.Book).Include(b => b.User)
                .Where(b => b.User.UserName == userName).ToArrayAsync();
        }

        public Task<BookUser> GetUserBook(string userName, int bookId) {
            if (userName == null) throw new ArgumentNullException(nameof(userName));
            return DataBase.BooksUsers.Include(b => b.Book).Include(b => b.User)
                .SingleOrDefaultAsync(b => b.User.UserName == userName && b.Book.Id == bookId);
        }

        public Task<Book> GetBookById(int bookId) {
            return DataBase.Books.SingleOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<BookUser> CreateBook(string userName) {
            var user = await UserStore.GetUser(userName);
            if (user == null) {
                Logger.LogAppErrorAndThrow($"User '{userName}' not found", ApplicationError.UserNotFound);
            }

            var newBookUser = new BookUser() {
                Book = new Book(),
                User = user,
                Role = BookUserRole.Administrator
            };

            DataBase.BooksUsers.Add(newBookUser);
            await DataBase.SaveChangesAsync();

            Logger.LogInformation($"Book #{newBookUser.Book.Id} has been created by user {userName}");

            return newBookUser;
        }

        public Task<BookUser> AddUser(string targetUserName, BookUserRole role, int bookId, string userName) {
            return SetBookUserRoleOperation(true, targetUserName, role, bookId, userName);
        }

        public Task<BookUser> ChangeRole(string targetUserName, BookUserRole role, int bookId, string userName) {
            return SetBookUserRoleOperation(false, targetUserName, role, bookId, userName);
        }

        public async Task<BookUser> SetBookUserRoleOperation(bool allowCreate, string targetUserName, BookUserRole role, int bookId, string userName) {
            var user = await UserStore.GetUser(userName);
            if (user == null) {
                Logger.LogAppErrorAndThrow($"User '{userName}' not found", ApplicationError.UserNotFound);
            }

            var book = await DataBase.Books.SingleOrDefaultAsync(b => b.Id == bookId);
            if (book == null) {
                Logger.LogAppErrorAndThrow($"Book #{bookId} not found", ApplicationError.BookNotFound);
            }

            var bookUser = await DataBase.BooksUsers.FirstOrDefaultAsync(bu =>
                bu.BookId == bookId && bu.UserId == user.Id);
            if (bookUser == null) {
                Logger.LogAppErrorAndThrow($"User '{userName}' has no access to the book #{bookId}", ApplicationError.PermissionDenied);
            }

            if (bookUser.Role != BookUserRole.Administrator) {
                Logger.LogAppErrorAndThrow($"User '{userName}' has no admin access to the book #{targetUserName}", ApplicationError.PermissionDenied);
            }

            var targetUser = await UserStore.GetUser(targetUserName);
            if (targetUser == null) {
                Logger.LogAppErrorAndThrow($"User '{targetUserName}' not found", ApplicationError.UserNotFound);
            }

            var targetBookUser = await DataBase.BooksUsers.FirstOrDefaultAsync(bu =>
                bu.BookId == bookId && bu.UserId == targetUser.Id);

            if (allowCreate) {
                if (targetBookUser != null) {
                    Logger.LogAppErrorAndThrow(ApplicationError.AlreadyExists);
                }

                targetBookUser = new BookUser() {
                    Book = book,
                    User = targetUser,
                    Role = role
                };
                DataBase.BooksUsers.Add(targetBookUser);
            }
            else {
                if (targetBookUser == null) {
                    Logger.LogAppErrorAndThrow(ApplicationError.BookUserNotFound);
                }
                targetBookUser.Role = role;
            }

            await DataBase.SaveChangesAsync();

            return targetBookUser;
        }

    }
}