using System.Linq;
using Finances.Models;
using Finances.Services.Books;
using Finances.Test.Utils;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace Finances.Test.Services {
    public class Books : TestsBase {
        public Books(DatabaseFixture fixture) : base(fixture) {
        }

        [Fact]
        public void CreateNewBook() {
            using (var context = DbFixture.CreateContext()) {
                var userStore = new UserStore<ApplicationUser>(context);
                var bookStore = new BookStore(context, userStore, new TestLogger<BookStore>());

                Assert.Equal(1, context.Users.Count());
                Assert.Equal(0, context.Books.Count());
                Assert.Equal(0, context.BooksUsers.Count());
                Assert.Equal(0, bookStore.GetUserBooks("Admin").Result.Length);

                var book = bookStore.CreateBook("Admin").Result;

                Assert.Equal(1, context.BooksUsers.Count());
                Assert.Equal(1, context.Books.Count());

                var bookUser = context.BooksUsers.FirstOrDefault(b => b.Book.Id == book.Id);
                Assert.NotNull(bookUser);
                Assert.Equal(BookUserRole.Administrator, bookUser.Role);
                Assert.Equal("Admin", bookUser.User.UserName);

                var books = bookStore.GetUserBooks("Admin").Result;
                Assert.Equal(1, books.Count());
                Assert.True(books.Any(b => b.Id == bookUser.Id));
                Assert.True(books.Any(b => b.Book.Id == book.Id));
            }
        }
    }
}