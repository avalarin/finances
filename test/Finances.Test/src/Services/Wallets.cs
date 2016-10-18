using System.Linq;
using Finances.Services.Books;
using Finances.Services.Users;
using Finances.Services.Wallets;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Finances.Test.Services {
    [Collection("WalletsService")]
    public class Wallets : DbTestsBase {
        private readonly WalletStore _walletStore;
        private readonly BookStore _bookStore;

        public Wallets(ITestOutputHelper outputHelper) : base(outputHelper)  {
            var userStore = new AppUserStore(Db);
            _bookStore = new BookStore(Db, userStore, CreateLogger<BookStore>());
            _walletStore = new WalletStore(Db, userStore, _bookStore, CreateLogger<WalletStore>());
        }

        [Fact(DisplayName = "Wallets service - creates a wallet")]
        public void CreateNewWallet() {
            Assert.Equal(0, Db.Books.Count());
            Assert.Equal(0, Db.Wallets.Count());

            var book = _bookStore.CreateBook("Admin").Result;
            var wallet = _walletStore.CreateWallet(book.BookUser.BookId, "Admin").Result.Wallet;

            Assert.Equal(1, Db.Wallets.Count());

            var wallet2 = Db.Wallets.Include(w => w.Book).First();

            Assert.Equal(wallet.Id, wallet2.Id);
            Assert.Equal(wallet.Book.Id, wallet2.Book.Id);
        }

    }
}