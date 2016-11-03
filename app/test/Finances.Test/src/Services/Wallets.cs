using System.Linq;
using Finances.Models;
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

        [Fact]
        public void GetWallets() {
            var book = _bookStore.CreateBook("Admin").Result;

            var wallet1 = _walletStore.CreateWallet(book.BookUser.BookId, "Wallet1", "Admin").Result.Wallet;
            var wallet2 = _walletStore.CreateWallet(book.BookUser.BookId, "Wallet2", "Admin").Result.Wallet;

            Assert.Equal(1, Db.Books.Count());
            Assert.Equal(2, Db.Wallets.Count());
            Assert.Equal(2, _walletStore.GetWallets("Admin", book.BookUser.BookId).Result.Length);
            Assert.Equal(0, _walletStore.GetWallets("Member", book.BookUser.BookId).Result.Length);

            _bookStore.AddUser("Member", BookUserRole.Member, book.BookUser.BookId, "Admin").Wait();

            Assert.Equal(2, _walletStore.GetWallets("Admin", book.BookUser.BookId).Result.Length);
            Assert.Equal(2, _walletStore.GetWallets("Member", book.BookUser.BookId).Result.Length);
        }

        [Fact(DisplayName = "Wallets service - creates a wallet")]
        public void CreateNewWallet() {
            Assert.Equal(0, Db.Books.Count());
            Assert.Equal(0, Db.Wallets.Count());

            var book = _bookStore.CreateBook("Admin").Result;
            var wallet = _walletStore.CreateWallet(book.BookUser.BookId, "Test", "Admin").Result.Wallet;

            Assert.Equal(1, Db.Wallets.Count());

            var wallet2 = Db.Wallets.Include(w => w.Book).First();

            Assert.Equal(wallet.Id, wallet2.Id);
            Assert.Equal(wallet.Book.Id, wallet2.Book.Id);
            Assert.Equal("Test", wallet2.Name);
            Assert.Equal(wallet.Name, wallet2.Name);
        }

    }
}