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

        public async Task<Wallet> CreateWallet(int bookId, string walletName, string userName) {
            var user = await UserStore.GetUser(userName);
            if (user == null) {
                Logger.LogAppErrorAndThrow($"User '{userName}' not found", ApplicationError.UserNotFound);
            }

            var bookUser = await BookStore.GetUserBook(userName, bookId);
            if (bookUser == null) {
                Logger.LogAppErrorAndThrow($"Cannot create wallet: book #{bookId} not found or user has no access to this book", ApplicationError.BookNotFound);
            }

            if (bookUser.Role < BookUserRole.Member) {
                Logger.LogAppErrorAndThrow($"Cannot create unit: permission denied for user {userName}", ApplicationError.PermissionDenied);
            }
            
            var newWallet = new Wallet() {
                Book = bookUser.Book,
                Name = walletName
            };

            DataBase.Wallets.Add(newWallet);
            await DataBase.SaveChangesAsync();

            return newWallet;
        }
    }
}
