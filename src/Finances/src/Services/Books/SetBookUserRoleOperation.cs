using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Finances.Services.ServiceOperations;
using Finances.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Books {
    public class SetBookUserRoleOperation : OperationBase<BookUserResult, SetBookUserRoleOperation.SetBookUserRoleOptions> {
        private IAppUserStore UserStore { get; }

        public SetBookUserRoleOperation(ILogger logger, ApplicationDbContext dataBase, IAppUserStore userStore) : base(logger, dataBase) {
            UserStore = userStore;
        }

        protected override async Task<BookUserResult> ExecuteCore(SetBookUserRoleOptions options) {
            var user = await UserStore.GetUser(options.UserName);
            if (user == null) {
                Logger.LogError($"User '{options.UserName}' not found");
                return new BookUserResult(BookUserErrorCode.UserNotFound);
            }

            var book = await DataBase.Books.SingleOrDefaultAsync(b => b.Id == options.TargetBookId);
            if (book == null) {
                Logger.LogError($"Book #{options.TargetBookId} not found");
                return new BookUserResult(BookUserErrorCode.BookNotFound);
            }

            var bookUser = await DataBase.BooksUsers.FirstOrDefaultAsync(bu =>
                bu.BookId == options.TargetBookId && bu.UserId == user.Id);
            if (bookUser == null) {
                Logger.LogError($"User '{options.UserName}' has no access to the book #{options.TargetBookId}");
                return new BookUserResult(BookUserErrorCode.PermissionDenied);
            }

            if (bookUser.Role != BookUserRole.Administrator) {
                Logger.LogError($"User '{options.UserName}' has no admin access to the book #{options.TargetBookId}");
                return new BookUserResult(BookUserErrorCode.PermissionDenied);
            }

            var targetUser = await UserStore.GetUser(options.TargetUserName);
            if (targetUser == null) {
                Logger.LogError($"User '{options.TargetUserName}' not found");
                return new BookUserResult(BookUserErrorCode.TargetUserNotFound);
            }

            var targetBookUser = await DataBase.BooksUsers.FirstOrDefaultAsync(bu =>
                bu.BookId == options.TargetBookId && bu.UserId == targetUser.Id);

            if (options.AllowCreate) {
                if (targetBookUser != null) {
                    return new BookUserResult(BookUserErrorCode.AlreadyExists);
                }

                targetBookUser = new BookUser() {
                    Book = book,
                    User = targetUser,
                    Role = options.Role
                };
                DataBase.BooksUsers.Add(targetBookUser);
            }
            else {
                if (targetBookUser == null) {
                    return new BookUserResult(BookUserErrorCode.BookUserNotFound);
                }
                targetBookUser.Role = options.Role;
            }

            await DataBase.SaveChangesAsync();

            return new BookUserResult(targetBookUser);
        }

        public class SetBookUserRoleOptions {
            public string UserName { get; set; }

            public string TargetUserName { get; set; }

            public int TargetBookId { get; set; }

            public bool AllowCreate { get; set; }

            public BookUserRole Role { get; set; }
        }
    }
}