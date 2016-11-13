using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finances.Services.Books;
using Finances.Web;

namespace Finances.Controllers {
    [Authorize]
    [Route("api/book")]
    public class BooksController : Controller {

        private readonly IBookStore _bookStore;

        public BooksController(IBookStore bookStore) {
            _bookStore = bookStore;
        }

        public async Task<Response> Get() {
            var books = (await _bookStore.GetUserBooks(User.Identity.Name)).Select(b => b.Book);
            return new PayloadResponse(new { books });
        }

        [Route("create")]
        public async Task<Response> Post() {
            var book = (await _bookStore.CreateBook(User.Identity.Name)).Book;
            return new PayloadResponse(new { book });
        }

    }
}