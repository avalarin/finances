using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Users;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Finances.Services.Transactions {
    public interface ITransactionStore {

        Task CreateTransaction(TransactionPrototype transaction);

    }

    public class TransactionStore : ITransactionStore {
        private ApplicationDbContext DataBase { get; }
        private IAppUserStore UserStore { get; }
        private IBookStore BookStore { get; }
        private ILogger<TransactionStore> Logger { get; }

        public TransactionStore(ApplicationDbContext dataBase, IAppUserStore userStore, IBookStore bookStore,
            ILogger<TransactionStore> logger) {
            DataBase = dataBase;
            UserStore = userStore;
            BookStore = bookStore;
            Logger = logger;
        }

        public async Task CreateTransaction(TransactionPrototype prototype) {
            using (var dbTransaction = await DataBase.Database.BeginTransactionAsync()) {
                try {
                    var user = await UserStore.GetUser(prototype.UserName);
                    if (user == null) {
                        Logger.LogError($"User '{prototype.UserName}' not found");
                        throw new InvalidOperationException(); // TODO
                    }

                    var bookUser = await BookStore.GetUserBook(prototype.UserName, prototype.BookId);
                    if (bookUser == null) {
                        Logger.LogError($"Cannot create transaction: book #{prototype.BookId} not found or user has no access to this book");
                        throw new InvalidOperationException(); // TODO
                    }

                    if (bookUser.Role < BookUserRole.Member) {
                        Logger.LogError($"Cannot create unit: permission denied for user {prototype.UserName}");
                        throw new InvalidOperationException(); // TODO
                    }

                    var transaction = new Transaction() {
                        CreatedAt = DateTime.Now,
                        CreatedBy = user,
                        Book = bookUser.Book
                    };

                    await PushTags(transaction, prototype);
                    PushOperations(transaction, prototype);

                    await DataBase.SaveChangesAsync();
                    dbTransaction.Commit();
                }
                catch (Exception) {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        private void PushOperations(Transaction transaction, TransactionPrototype prototype) {
            var ops = prototype.Operations.Select(op => CreateOperation(transaction, prototype, op));
            transaction.Operations = ops.ToList();
        }

        private Operation CreateOperation(Transaction txn, TransactionPrototype txnPrototype, OperationPrototype opPrototype) {
            var op = new Operation() {
                Transaction = txn,
                Amount = opPrototype.Amount,
                Currency = opPrototype.Currency,

                Count = opPrototype.Count,
                Price = opPrototype.Price
            };

            if (opPrototype.ProductId.HasValue) {
                op.ProductId = opPrototype.ProductId.Value;
            }
            else if (opPrototype.Product != null) {
                throw new NotSupportedException();
            }

            return op;
        }

        private async Task PushTags(Transaction transaction, TransactionPrototype prototype) {
            var eTags = await DataBase.Tags.Where(t => t.BookId == prototype.BookId)
                .Join(prototype.Tags, dbTag => dbTag.Text, tagText => tagText, (dbTag, text) => dbTag)
                .ToArrayAsync();
            var tagsToCreate = prototype.Tags.Except(eTags.Select(t => t.Text), StringComparer.OrdinalIgnoreCase)
                .Select(text => new Tag() {BookId = prototype.BookId, Text = text});
            var transactionTags =
                new LinkedList<TransactionTag>(
                    eTags.Concat(tagsToCreate).Select(t => new TransactionTag() {Transaction = transaction, Tag = t}));
            transaction.Tags = transactionTags;
        }
    }
}