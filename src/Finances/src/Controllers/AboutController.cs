using Microsoft.AspNetCore.Mvc;

namespace Finances.Controllers {
    [Route("api/about")]
    public class AboutController : Controller {

        public object Get() {
            return new {
                version = "1"
            };
        }

    }
}
