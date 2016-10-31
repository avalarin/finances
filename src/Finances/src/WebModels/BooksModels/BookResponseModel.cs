using Finances.Models;

namespace Finances.WebModels.BooksModels {
    public class BookResponseModel : ResponseModel<CreateBookStatus> {

        public int BookId { get; set; }

        public BookResponseModel(CreateBookStatus status) : base(status) {
        }

        public BookResponseModel(Book book) : base(CreateBookStatus.Success) {
            BookId = book.Id;
        }
    }
}