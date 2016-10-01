using System;
using System.Linq;
using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Units;
using Finances.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Products {
    public class ProductStore : IProductStore {
         
        private readonly ApplicationDbContext _db;
        private readonly CreateProductOperation _createProductOperation;

        public ProductStore(ApplicationDbContext db, IUnitStore unitStore, IBookStore bookStore, IAppUserStore userStore, ILogger<ProductStore> logger) {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _db = db;
            _createProductOperation = new CreateProductOperation(logger, _db, unitStore, bookStore, userStore);
        }

         public Task<CreateProductResult> CreateProduct(string title, int unitId, int bookId, string userName) {
             return _createProductOperation.Execute(new CreateProductOperation.CreateProductOptions() {
                 Title = title,
                 UnitId = unitId,
                 BookId = bookId,
                 UserName = userName
             });
         }

         public Task<Product[]> GetProducts(int book) {
             return _db.Products.Where(p => p.Book.Id == book).ToArrayAsync();
         }

         public Task<Product> GetProduct(int productId, int bookId) {
             return _db.Products.FirstOrDefaultAsync(p => p.Book.Id == bookId && p.Id == bookId);
         }
    }
}