using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Books {
    public class BookStore : IBookStore {
        private readonly ApplicationDbContext _db;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<BookStore> _logger;

        public BookStore(ApplicationDbContext db, IUserStore<ApplicationUser> userStore, ILogger<BookStore> logger) {
            _db = db;
            _userStore = userStore;
            _logger = logger;
        }

        public Task<BookUser[]> GetUserBooks(string userName) {
            if (string.IsNullOrWhiteSpace(userName)) {
                throw new ArgumentException("Cannot create book: user name cannot be empty or null", nameof(userName));
            }

            return _db.BooksUsers.Include(b => b.Book).Include(b => b.User)
                .Where(b => b.User.UserName == userName).ToArrayAsync();
        }

        public async Task<BookUser> CreateBook(string userName) {
            if (string.IsNullOrWhiteSpace(userName)) {
                throw new ArgumentException("Cannot create book: user name cannot be empty or null", nameof(userName));
            }

            var user = await _userStore.FindByNameAsync(userName.ToUpperInvariant(), CancellationToken.None);

            if (user == null) {
                throw new ArgumentException($"Cannot create book: user '{userName}' not found", nameof(userName));
            }

            var newBookUser = new BookUser() {
                Book = new Book(),
                User = user,
                Role = BookUserRole.Administrator
            };
            
            _db.BooksUsers.Add(newBookUser);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"Book #${newBookUser.Book.Id} has been created by user {userName}");

            return newBookUser;
        }
    }
}