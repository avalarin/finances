 using System.Threading.Tasks;
using Finances.Data;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.ServiceOperations;
using Finances.Services.Units;
using Finances.Services.Users;
using Microsoft.Extensions.Logging;

namespace Finances.Services.Products {

    public class CreateProductOperation : OperationBase<CreateProductResult, CreateProductOperation.CreateProductOptions> {
        private IUnitStore UnitStore { get; }

        private IBookStore BookStore { get; }

        private IAppUserStore UserStore { get; }

        public CreateProductOperation(ILogger logger, ApplicationDbContext db, IUnitStore unitStore, IBookStore bookStore, IAppUserStore userStore) : base(logger, db) {
            UnitStore = unitStore;
            BookStore = bookStore;
            UserStore = userStore;
        }

        protected override async Task<CreateProductResult> ExecuteCore(CreateProductOptions options) {
            var user = await UserStore.GetUser(options.UserName);
            if (user == null) {
                Logger.LogError($"User '{options.UserName}' not found");
                return new CreateProductResult(CreateProductErrorCode.UserNotFound);
            }

            var bookUser = await BookStore.GetUserBook(options.UserName, options.BookId);
            if (bookUser == null) {
                Logger.LogError($"Cannot create unit: book #{options.BookId} not found or user has no access to this book");
                return new CreateProductResult(CreateProductErrorCode.BookNotFound);
            }

            if (bookUser.Role < BookUserRole.Member) {
                Logger.LogError($"Cannot create unit: permission denied for user {options.UserName}");
                return new CreateProductResult(CreateProductErrorCode.PermissionDenied);
            }

            var unit = await UnitStore.GetUnit(options.UnitId, options.BookId);

            var product = new Product() {
                Book = bookUser.Book,
                Name = options.Title,
                Unit = unit
            };

            DataBase.Products.Add(product);
            await DataBase.SaveChangesAsync();

            return new CreateProductResult(product);
        }

        public class CreateProductOptions {
            
            public string Title { get; set; }
            public int UnitId { get; set; }
            public int BookId { get; set; }
            public string UserName { get; set; }

        }

    }

}