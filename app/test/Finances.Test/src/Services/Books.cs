using System.Linq;
using System.Threading.Tasks;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Users;
using Xunit;
using Xunit.Abstractions;

namespace Finances.Test.Services {
    public class Books : DbTestsBase {
        private readonly IBookStore _bookStore;

        public Books(ITestOutputHelper outputHelper) : base(outputHelper) {
            var userStore = new AppUserStore(Db);
            _bookStore = new BookStore(Db, userStore, CreateLogger<BookStore>());
        }

        [Fact(DisplayName = "Books service - creates a book")]
        public async Task CreatesNewBook() {
            Assert.Equal(0, Db.Books.Count());
            Assert.Equal(0, Db.BooksUsers.Count());
            Assert.Equal(0, (await _bookStore.GetUserBooks("Admin")).Length);

            var result = await _bookStore.CreateBook("Admin");

            Assert.True(result.Success);
            Assert.Equal(1, Db.BooksUsers.Count());
            Assert.Equal(1, Db.Books.Count());

            var bookUser = Db.BooksUsers.FirstOrDefault(b => b.Book.Id == result.BookUser.Book.Id);
            Assert.NotNull(bookUser);
            Assert.Equal(BookUserRole.Administrator, bookUser.Role);
            Assert.Equal("Admin", bookUser.User.UserName);

            var books = await _bookStore.GetUserBooks("Admin");
            Assert.Equal(1, books.Count());
            Assert.True(books.Any(b => b.Id == bookUser.Id));
            Assert.True(books.Any(b => b.Book.Id == result.BookUser.Book.Id));
        }

        [Theory(DisplayName = "Books service - adds users to books")]
        [InlineData("Member", BookUserRole.Member)]
        [InlineData("Guest", BookUserRole.Guest)]
        public async Task AddsUsersToBooks(string userName, BookUserRole role) {
            var createBookResult = await _bookStore.CreateBook("Admin");
            var book = createBookResult.BookUser.Book;

            var result = await _bookStore.AddUser(userName, role, book.Id, "Admin");
            Assert.True(result.Success);

            var books = await _bookStore.GetUserBooks(userName);
            Assert.Equal(1, books.Length);
            Assert.Equal(book.Id, books[0].Book.Id);
            Assert.Equal(role, books[0].Role);
        }

        [Theory(DisplayName = "Books service - changes the user's role")]
        [InlineData("Guest", BookUserRole.Member, BookUserRole.Guest)]
        [InlineData("Member", BookUserRole.Guest, BookUserRole.Member)]
        public async Task ChangesUsersRole(string userName, BookUserRole firstRole, BookUserRole secondRole) {
            var createBookResult = await _bookStore.CreateBook("Admin");
            var book = createBookResult.BookUser.Book;

            var result = await _bookStore.AddUser(userName, firstRole, book.Id, "Admin");
            Assert.True(result.Success);

            result = await _bookStore.ChangeRole(userName, secondRole, book.Id, "Admin");
            Assert.True(result.Success);

            var books = await _bookStore.GetUserBooks(userName);
            Assert.Equal(1, books.Length);
            Assert.Equal(book.Id, books[0].Book.Id);
            Assert.Equal(secondRole, books[0].Role);
        }

        [Theory(DisplayName = "Books service - only an admin can add users")]
        [InlineData("Guest", BookUserRole.Guest, "Member",  BookUserRole.Member)]
        [InlineData("Member", BookUserRole.Member, "Guest", BookUserRole.Guest)]
        public async Task OnlyAdminCanAddUser(string creator, BookUserRole creatorRole, string other, BookUserRole otherRole) {
            var createBookResult = await _bookStore.CreateBook("Admin");
            var book = createBookResult.BookUser.Book;

            await _bookStore.AddUser(creator, creatorRole, book.Id, "Admin");

            var result = await _bookStore.AddUser(other, otherRole, book.Id, creator);
            Assert.False(result.Success);
            Assert.Equal(BookUserErrorCode.PermissionDenied, result.ErrorCode);
        }

        [Theory(DisplayName = "Books service - only an admin can change user's role")]
        [InlineData("Guest", BookUserRole.Guest, "Member", BookUserRole.Member)]
        [InlineData("Member", BookUserRole.Member, "Guest", BookUserRole.Guest)]
        public async Task OnlyAdminCanChangeRole(string updator, BookUserRole updatorRole, string other, BookUserRole otherRole) {
            var createBookResult = await _bookStore.CreateBook("Admin");
            var book = createBookResult.BookUser.Book;

            await _bookStore.AddUser(updator, updatorRole, book.Id, "Admin");
            await _bookStore.AddUser(other, updatorRole, book.Id, "Admin");

            var result = await _bookStore.ChangeRole(other, otherRole, book.Id, updator);
            Assert.False(result.Success);
            Assert.Equal(BookUserErrorCode.PermissionDenied, result.ErrorCode);
        }

    }
}