using System.Linq;
using Finances.Services.Books;
using Finances.Services.Products;
using Finances.Services.Units;
using Finances.Services.Users;
using Xunit;
using Xunit.Abstractions;

using static Xunit.Assert;
using static Finances.Test.Utils.AssertErrors;

namespace Finances.Test.Services {
    public class Products : DbTestsBase {
        private readonly BookStore _bookStore;
        private readonly UnitStore _unitStore;
        private readonly ProductStore _productStore;

        public Products(ITestOutputHelper outputHelper) : base(outputHelper) {
            var userStore = new AppUserStore(Db);
            _bookStore = new BookStore(Db, userStore, CreateLogger<BookStore>());
            _unitStore = new UnitStore(Db, _bookStore, userStore, CreateLogger<UnitStore>());
            _productStore = new ProductStore(Db, _unitStore, _bookStore, userStore, CreateLogger<ProductStore>());
        }

        [Theory(DisplayName = "Products service - creates a product")]
        [InlineData("Test1")]
        public void CreateNewProduct(string title) {
            var book = _bookStore.CreateBook("Admin").Result;
            var unit = _unitStore.GetUnits(book.BookId).Result.First();
            var result = _productStore.CreateProduct(title, unit.Id, book.BookId, "Admin").Result;
        }

    }
}