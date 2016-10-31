using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Wallets {
    public class WalletStore : IWalletStore {
        private ApplicationDbContext DataBase { get; }
        private IAppUserStore UserStore { get; }
        private IBookStore BookStore { get; }
        private ILogger<WalletStore> Logger { get; }

        public WalletStore(ApplicationDbContext dataBase, IAppUserStore userStore, IBookStore bookStore, ILogger<WalletStore> logger) {
            DataBase = dataBase;
            UserStore = userStore;
            BookStore = bookStore;
            Logger = logger;
        }

        public Task<Wallet[]> GetWallets(string userName, int bookId) {
            var wallets = from wallet
                            in DataBase.Wallets
                            join userBook in DataBase.BooksUsers on wallet.BookId equals userBook.BookId
                            where wallet.BookId == bookId && userBook.User.UserName == userName
                          select wallet;

            return wallets.ToArrayAsync();
        }

        public async Task<CreateWalletResult> CreateWallet(int bookId, string walletName, string userName) {
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
                Book = bookUser.Book,
                Name = walletName
            };

            DataBase.Wallets.Add(newWallet);
            await DataBase.SaveChangesAsync();

            return new CreateWalletResult(newWallet);
        }
    }
}
