using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finances.Services.Transactions;
using Finances.Models.Prototypes;
using Finances.Web;

namespace Finances.Controllers {
    [Authorize]
    [Route("api/txn")]
    public class TransactionsController : Controller {
        private readonly ITransactionStore _txnStore;

        public TransactionsController(ITransactionStore txnStore) {
            _txnStore = txnStore;
        }

        public async Task<Response> Get(int bookId) {
            var txns = await _txnStore.GetTransactions(bookId, User.Identity.Name);
            return new PayloadResponse(new { txns });
        }

        [Route("create")]
        public async Task<Response> Post(TransactionPrototype model) {
            var result = await _txnStore.CreateTransaction(model);
            
            return new Response();
        }

    }
}