using Finances.Models;

namespace Finances.Services.Books {
    public class BookUserResult : OperationResult<BookUserErrorCode, BookUser> {

        public BookUser BookUser { get; }

        protected override BookUser Payload => BookUser;

        public BookUserResult(BookUserErrorCode? errorCode) : base(errorCode) {
        }

        public BookUserResult(BookUser bookUser) {
            BookUser = bookUser;
        }
    }
}