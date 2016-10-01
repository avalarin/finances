using System.Threading.Tasks;
using Finances.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finances.Services.Books;

namespace Finances.Controllers {
    [Authorize]
    [Route("api/books")]
    public class BooksController : Controller {

        private BookStore _bookStore;

        public Task<Book[]> Get() {
            return null;
        }

    }
}