using Finances.Models;

namespace Finances.Services.Wallets {
    public class CreateWalletResult : OperationResult<CreateWalletErrorCode, Wallet> {
        public Wallet Wallet { get; }

        protected override Wallet Payload => Wallet;

        public CreateWalletResult(CreateWalletErrorCode? errorCode) : base(errorCode) {
        }

        public CreateWalletResult(Wallet wallet) {
            Wallet = wallet;
        }
    }
}