using System.Threading.Tasks;
using Finances.Models;

namespace Finances.Services.Wallets {
    public interface IWalletStore {
        Task<Wallet[]> GetWallets(string userName, int bookId);

        Task<CreateWalletResult> CreateWallet(int bookId, string walletName, string userName);
    }
}