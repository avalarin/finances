using Microsoft.AspNetCore.Mvc;

namespace Finances.Controllers {
    public class AppController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
