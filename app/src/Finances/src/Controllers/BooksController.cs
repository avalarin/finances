using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finances.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finances.Services.Books;
using Finances.WebModels.BooksModels;

namespace Finances.Controllers {
    [Authorize]
    [Route("api/book")]
    public class BooksController : Controller {

        private readonly IBookStore _bookStore;

        public BooksController(IBookStore bookStore) {
            _bookStore = bookStore;
        }

        public async Task<IEnumerable<Book>> Get() {
            BookUser[] books = await _bookStore.GetUserBooks(User.Identity.Name);
            return books.Select(b => b.Book);
        }

        [Route("create")]
        public async Task<BookResponseModel> Post(CreateBookRequestModel model) {
            var result = await _bookStore.CreateBook(User.Identity.Name);

            if (!result.Success) {
                return new BookResponseModel(CreateBookStatus.CannotCreate);
            }
            
            return new BookResponseModel(result.BookUser.Book);
        }

    }
}