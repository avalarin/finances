using Finances.Web;
using Microsoft.AspNetCore.Mvc;

namespace Finances.Controllers {
    [Route("api/about")]
    public class AboutController : Controller {

        public Response Get() {
            var version = "1";
            return new PayloadResponse(new { version });
        }

    }
}
