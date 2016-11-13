using System.Threading.Tasks;
using Finances.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finances.Services.Wallets;
using Finances.Web;

namespace Finances.Controllers {
    [Authorize]
    [Route("api/wallet")]
    public class WalletsController : Controller {

        private readonly IWalletStore _walletStore;

        public WalletsController(IWalletStore walletStore) {
            _walletStore = walletStore;
        }

        public async Task<Response> Post([FromBody]GetWalletsRequestModel model) {
            var wallets = await _walletStore.GetWallets(User.Identity.Name, model.BookId);
            return new PayloadResponse(new { wallets });
        }

        [Route("create")]
        public async Task<Response> Post([FromBody]CreateWalletRequestModel model) {
            var result = await _walletStore.CreateWallet(model.BookId, model.WalletName, User.Identity.Name);

            return new Response();
        }

    }
}