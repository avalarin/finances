using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Finances.Controllers {
    [Route("api/about")]
    public class AboutController : Controller {

        public async Task<Object> Get() {
            return new {
                version = "1"
            };
        }

    }
}
