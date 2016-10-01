using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Finances.Services.ServiceOperations;
using Finances.Services.Users;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Books {
    public class CreateBookOperation : OperationBase<BookUserResult, CreateBookOperationOptions> {
        private IAppUserStore UserStore { get; }

        public CreateBookOperation(ILogger logger, ApplicationDbContext dataBase, IAppUserStore userStore) : base(logger, dataBase) {
            UserStore = userStore;
        }

        protected override async Task<BookUserResult> ExecuteCore(CreateBookOperationOptions options) {
            var user = await UserStore.GetUser(options.UserName);
            if (user == null) {
                Logger.LogError($"User '{options.UserName}' not found");
                return new BookUserResult(BookUserErrorCode.UserNotFound);
            }

            var newBookUser = new BookUser() {
                Book = new Book(),
                User = user,
                Role = BookUserRole.Administrator
            };

            DataBase.BooksUsers.Add(newBookUser);
            await DataBase.SaveChangesAsync();

            Logger.LogInformation($"Book #{newBookUser.Book.Id} has been created by user {options.UserName}");

            return new BookUserResult(newBookUser);
        }
    }

    public class CreateBookOperationOptions {
        public string UserName { get; set; }
    }
}