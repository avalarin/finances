using System;
using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Exceptions;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Units;
using Finances.Services.Users;
using Finances.Utils.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Products {
    public class ProductStore : IProductStore {
        private ApplicationDbContext DataBase { get; }
        private IUnitStore UnitStore { get; }
        private IBookStore BookStore { get; }
        private IAppUserStore UserStore { get; }
        private ILogger<ProductStore> Logger { get; }

        public ProductStore(ApplicationDbContext db, IUnitStore unitStore, IBookStore bookStore, IAppUserStore userStore, ILogger<ProductStore> logger) {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            DataBase = db;
            UnitStore = unitStore;
            BookStore = bookStore;
            UserStore = userStore;
            Logger = logger;
        }

        public async Task<Product> CreateProduct(string title, int unitId, int bookId, string userName) {
            var user = await UserStore.GetUser(userName);
            if (user == null) {
                Logger.LogAppErrorAndThrow($"User '{userName}' not found", ApplicationError.UserNotFound);
            }

            var bookUser = await BookStore.GetUserBook(userName, bookId);
            if (bookUser == null) {
                Logger.LogAppErrorAndThrow($"Cannot create unit: book #{bookId} not found or user has no access to this book", ApplicationError.BookNotFound);
            }

            if (bookUser.Role < BookUserRole.Member) {
                Logger.LogAppErrorAndThrow($"Cannot create unit: permission denied for user {userName}", ApplicationError.PermissionDenied);
            }

            var unit = await UnitStore.GetUnit(unitId, bookId);

            var product = new Product() {
                Book = bookUser.Book,
                Name = title,
                Unit = unit
            };

            DataBase.Products.Add(product);
            await DataBase.SaveChangesAsync();

            return product;
        }

        public Task<Product[]> GetProducts(int book) {
            return DataBase.Products.Where(p => p.Book.Id == book).ToArrayAsync();
        }

        public Task<Product> GetProduct(int productId, int bookId) {
            return DataBase.Products.FirstOrDefaultAsync(p => p.Book.Id == bookId && p.Id == bookId);
        }
    }
}