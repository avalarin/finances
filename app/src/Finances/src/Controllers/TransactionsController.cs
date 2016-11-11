using System.Threading.Tasks;
using Finances.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finances.Services.Transactions;
using Finances.WebModels.TransactionsModels;

namespace Finances.Controllers {
    [Authorize]
    [Route("api/txn")]
    public class TransactionsController : Controller {
        private readonly ITransactionStore _txnStore;

        public TransactionsController(ITransactionStore txnStore) {
            _txnStore = txnStore;
        }

        public Task<Transaction[]> Get(int bookId) {
            return _txnStore.GetTransactions(bookId, User.Identity.Name);
        }

        [Route("create")]
        public async Task<TransactionResponseModel<CreateTransactionStatus>> Post(TransactionPrototype model) {
            await _txnStore.CreateTransaction(model);

            // if (!result.Success) {
            //     return new TransactionResponseModel<CreateTransactionStatus>(CreateTransactionStatus.Failed);
            // }
            
            return new TransactionResponseModel<CreateTransactionStatus>(CreateTransactionStatus.Success);
        }

    }
}