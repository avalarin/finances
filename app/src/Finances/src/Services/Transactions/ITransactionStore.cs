using System.Threading.Tasks;

namespace Finances.Services.Transactions {
    public interface ITransactionStore {

        Task CreateTransaction(TransactionPrototype transaction);

    }
}