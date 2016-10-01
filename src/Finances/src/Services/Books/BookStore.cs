using System;
using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Finances.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Books {
    public class BookStore : IBookStore {
        private readonly ApplicationDbContext _db;

        private readonly CreateBookOperation _createBookOperation;
        private readonly SetBookUserRoleOperation _setBookUserRoleOperation;

        public BookStore(ApplicationDbContext db, IAppUserStore userStore, ILogger<BookStore> logger) {
            _db = db;

            _createBookOperation = new CreateBookOperation(logger, db, userStore);
            _setBookUserRoleOperation = new SetBookUserRoleOperation(logger, db, userStore);
        }

        public Task<BookUser[]> GetUserBooks(string userName) {
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            return _db.BooksUsers.Include(b => b.Book).Include(b => b.User)
                .Where(b => b.User.UserName == userName).ToArrayAsync();
        }

        public Task<BookUser> GetUserBook(string userName, int bookId) {
            if (userName == null) throw new ArgumentNullException(nameof(userName));
            return _db.BooksUsers.Include(b => b.Book).Include(b => b.User)
                .SingleOrDefaultAsync(b => b.User.UserName == userName && b.Book.Id == bookId);
        }

        public Task<Book> GetBookById(int bookId) {
            return _db.Books.SingleOrDefaultAsync(b => b.Id == bookId);
        }

        public Task<BookUserResult> CreateBook(string userName) {
            return _createBookOperation.Execute(new CreateBookOperationOptions() {
                UserName = userName
            });
        }

        public Task<BookUserResult> AddUser(string targetUserName, BookUserRole role, int bookId, string userName) {
            return _setBookUserRoleOperation.Execute(new SetBookUserRoleOperation.SetBookUserRoleOptions() {
                AllowCreate = true,
                TargetUserName = targetUserName,
                TargetBookId = bookId,
                Role = role,
                UserName = userName
            });
        }

        public Task<BookUserResult> ChangeRole(string targetUserName, BookUserRole role, int bookId, string userName) {
            return _setBookUserRoleOperation.Execute(new SetBookUserRoleOperation.SetBookUserRoleOptions() {
                AllowCreate = false,
                TargetUserName = targetUserName,
                TargetBookId = bookId,
                Role = role,
                UserName = userName
            });
        }

    }
}