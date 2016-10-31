namespace Finances.WebModels.WalletsModels {
    public class WalletResponseModel : ResponseModel<CreateWalletStatus> {
        public WalletResponseModel(CreateWalletStatus status) : base(status) {

        }

        public WalletResponseModel() : base(CreateWalletStatus.Success) {
        }
    }
}