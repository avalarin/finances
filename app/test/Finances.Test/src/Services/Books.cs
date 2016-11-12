using System.Linq;
using System.Threading.Tasks;
using Finances.Exceptions;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Users;
using Xunit;
using Xunit.Abstractions;

using static Xunit.Assert;
using static Finances.Test.Utils.AssertErrors;

namespace Finances.Test.Services {
    public class Books : DbTestsBase {
        private readonly IBookStore _bookStore;

        public Books(ITestOutputHelper outputHelper) : base(outputHelper) {
            var userStore = new AppUserStore(Db);
            _bookStore = new BookStore(Db, userStore, CreateLogger<BookStore>());
        }

        [Fact(DisplayName = "Books service - creates a book")]
        public async Task CreatesNewBook() {
            Equal(0, Db.Books.Count());
            Equal(0, Db.BooksUsers.Count());
            Equal(0, (await _bookStore.GetUserBooks("Admin")).Length);

            var result = await _bookStore.CreateBook("Admin");

            Equal(1, Db.BooksUsers.Count());
            Equal(1, Db.Books.Count());

            var bookUser = Db.BooksUsers.FirstOrDefault(b => b.Book.Id == result.Book.Id);
            NotNull(bookUser);
            Equal(BookUserRole.Administrator, bookUser.Role);
            Equal("Admin", bookUser.User.UserName);

            var books = await _bookStore.GetUserBooks("Admin");
            Equal(1, books.Count());
            True(books.Any(b => b.Id == bookUser.Id));
            True(books.Any(b => b.Book.Id == result.Book.Id));
        }

        [Theory(DisplayName = "Books service - adds users to books")]
        [InlineData("Member", BookUserRole.Member)]
        [InlineData("Guest", BookUserRole.Guest)]
        public async Task AddsUsersToBooks(string userName, BookUserRole role) {
            var createBookResult = await _bookStore.CreateBook("Admin");
            var book = createBookResult.Book;

            var result = await _bookStore.AddUser(userName, role, book.Id, "Admin");

            var books = await _bookStore.GetUserBooks(userName);
            Equal(1, books.Length);
            Equal(book.Id, books[0].Book.Id);
            Equal(role, books[0].Role);
        }

        [Theory(DisplayName = "Books service - changes the user's role")]
        [InlineData("Guest", BookUserRole.Member, BookUserRole.Guest)]
        [InlineData("Member", BookUserRole.Guest, BookUserRole.Member)]
        public async Task ChangesUsersRole(string userName, BookUserRole firstRole, BookUserRole secondRole) {
            var createBookResult = await _bookStore.CreateBook("Admin");
            var book = createBookResult.Book;

            var result = await _bookStore.AddUser(userName, firstRole, book.Id, "Admin");

            result = await _bookStore.ChangeRole(userName, secondRole, book.Id, "Admin");

            var books = await _bookStore.GetUserBooks(userName);
            Equal(1, books.Length);
            Equal(book.Id, books[0].Book.Id);
            Equal(secondRole, books[0].Role);
        }

        [Theory(DisplayName = "Books service - only an admin can add users")]
        [InlineData("Guest", BookUserRole.Guest, "Member",  BookUserRole.Member)]
        [InlineData("Member", BookUserRole.Member, "Guest", BookUserRole.Guest)]
        public async Task OnlyAdminCanAddUser(string creator, BookUserRole creatorRole, string other, BookUserRole otherRole) {
            var createBookResult = await _bookStore.CreateBook("Admin");
            var book = createBookResult.Book;

            await _bookStore.AddUser(creator, creatorRole, book.Id, "Admin");

            await ExpectAppError(ApplicationError.PermissionDenied, 
                                 () => _bookStore.AddUser(other, otherRole, book.Id, creator));
        }

        [Theory(DisplayName = "Books service - only an admin can change user's role")]
        [InlineData("Guest", BookUserRole.Guest, "Member", BookUserRole.Member)]
        [InlineData("Member", BookUserRole.Member, "Guest", BookUserRole.Guest)]
        public async Task OnlyAdminCanChangeRole(string updator, BookUserRole updatorRole, string other, BookUserRole otherRole) {
            var createBookResult = await _bookStore.CreateBook("Admin");
            var book = createBookResult.Book;

            await _bookStore.AddUser(updator, updatorRole, book.Id, "Admin");
            await _bookStore.AddUser(other, updatorRole, book.Id, "Admin");

            await ExpectAppError(ApplicationError.PermissionDenied, 
                                 () => _bookStore.ChangeRole(other, otherRole, book.Id, updator));
        }

    }
}