using System.Threading.Tasks;
using Finances.Models.Prototypes;
using Finances.Models;

namespace Finances.Services.Transactions {
    public interface ITransactionStore {

        Task<Transaction[]> GetTransactions(int bookId, string userName);

        Task<Transaction> CreateTransaction(TransactionPrototype transaction);

    }
}