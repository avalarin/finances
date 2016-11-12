using System.Collections.Generic;
using System.Threading.Tasks;
using Finances.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finances.Services.Wallets;
using Finances.WebModels.WalletsModels;

namespace Finances.Controllers {
    [Authorize]
    [Route("api/wallet")]
    public class WalletsController : Controller {

        private readonly IWalletStore _walletStore;

        public WalletsController(IWalletStore walletStore) {
            _walletStore = walletStore;
        }

        public async Task<IEnumerable<Wallet>> Post([FromBody]GetWalletsRequestModel model) {
            return await _walletStore.GetWallets(User.Identity.Name, model.BookId);
        }

        [Route("create")]
        public async Task<WalletResponseModel> Post([FromBody]CreateWalletRequestModel model) {
            var result = await _walletStore.CreateWallet(model.BookId, model.WalletName, User.Identity.Name);

            // if (!result.Success) {
            //     return new WalletResponseModel(CreateWalletStatus.CannotCreate);
            // }

            return new WalletResponseModel();
        }

    }
}