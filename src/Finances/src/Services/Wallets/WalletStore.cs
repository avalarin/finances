using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Users;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Wallets {
    public class WalletStore {
        private ApplicationDbContext DataBase { get; }
        private AppUserStore UserStore { get; }
        private BookStore BookStore { get; }
        private ILogger<WalletStore> Logger { get; }

        public WalletStore(ApplicationDbContext dataBase, AppUserStore userStore, BookStore bookStore, ILogger<WalletStore> logger) {
            DataBase = dataBase;
            UserStore = userStore;
            BookStore = bookStore;
            Logger = logger;
        }

        public async Task<CreateWalletResult> CreateWallet(int bookId, string userName) {
            var user = await UserStore.GetUser(userName);
            if (user == null) {
                Logger.LogError($"User '{userName}' not found");
                return new CreateWalletResult(CreateWalletErrorCode.UserNotFound);
            }

            var bookUser = await BookStore.GetUserBook(userName, bookId);
            if (bookUser == null) {
                Logger.LogError($"Cannot create wallet: book #{bookId} not found or user has no access to this book");
                return new CreateWalletResult(CreateWalletErrorCode.BookNotFound);
            }

            if (bookUser.Role < BookUserRole.Member) {
                Logger.LogError($"Cannot create unit: permission denied for user {userName}");
                return new CreateWalletResult(CreateWalletErrorCode.PermissionDenied);
            }

            
            var newWallet = new Wallet() {
                Book = bookUser.Book
            };

            DataBase.Wallets.Add(newWallet);
            await DataBase.SaveChangesAsync();

            return new CreateWalletResult(newWallet);
        }
    }
}
