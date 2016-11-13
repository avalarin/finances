using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Finances.Models.Prototypes;
using Finances.Test.Utils;
using Finances.Services.Books;
using Finances.Services.Users;
using Finances.Services.Transactions;
using Finances.Services.Wallets;
using Xunit;
using Xunit.Abstractions;
using static Xunit.Assert;
using static Finances.Test.Utils.AssertErrors;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Finances.Test.Services {
    public class Transactions : DbTestsBase {
        private IAppUserStore UserStore { get; }
        private IBookStore BookStore { get; }
        private IWalletStore WalletStore { get; }
        private ITransactionStore TxnStore { get; }

        public Transactions(ITestOutputHelper outputHelper) : base(outputHelper) {
            UserStore = new AppUserStore(Db);
            BookStore = new BookStore(Db, UserStore, CreateLogger<BookStore>());
            WalletStore = new WalletStore(Db, UserStore, BookStore, CreateLogger<WalletStore>());
            TxnStore = new TransactionStore(Db, UserStore, BookStore, CreateLogger<TransactionStore>());
        }

        [Fact(DisplayName = "Transaction service - creates a simple transaction")]
        public async Task CreatesSimpleTransaction() {
             var book = (await BookStore.CreateBook("Admin")).Book;
             var wallet = (await WalletStore.CreateWallet(book.Id, "Wallet1", "Admin"));

             Equal(0, await Db.Tags.Where(t => t.BookId == book.Id).CountAsync());
             Equal(0, await Db.Transactions.Where(t => t.BookId == book.Id).CountAsync());
             Equal(0, (await TxnStore.GetTransactions(book.Id, "Admin")).Count());

             var newTxn = await TxnStore.CreateTransaction(new TransactionPrototype() {
                 BookId = book.Id, UserName = "Admin",
                 DateTime = DateTime.Now,
                 Tags = new [] { "test_tag", "test_tag2" },
                 Operations = new OperationPrototype[] {
                     new OperationPrototype() {
                         WalletId = wallet.Id,
                         Currency = Currencies.RUB,
                         Amount = 5.5M
                     }
                 }
             });

             Equal(1, Db.Transactions.Where(t => t.Book.Id == book.Id).Count());
             Equal(2, Db.Tags.Where(t => t.Book.Id == book.Id).Count());

             var eTxn = (await TxnStore.GetTransactions(book.Id, "Admin")).Single();
             
             Equal(eTxn.Id, newTxn.Id);
             Equal(book.Id, eTxn.BookId);
             Equal(book.Id, newTxn.BookId);
             Equal(newTxn.CreatedAt, eTxn.CreatedAt);
             Equal(newTxn.CreatedBy.Id, eTxn.CreatedBy.Id);

             Equal(1, eTxn.Operations.Count);
             Equal(1, newTxn.Operations.Count);
        }
    }
}