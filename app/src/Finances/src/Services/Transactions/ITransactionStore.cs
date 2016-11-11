using System.Threading.Tasks;
using Finances.Models;

namespace Finances.Services.Transactions {
    public interface ITransactionStore {

        Task<Transaction[]> GetTransactions(int bookId, string userName);

        Task CreateTransaction(TransactionPrototype transaction);

    }
}