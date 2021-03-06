using System.Linq;
using System.Threading.Tasks;
using Finances.Models;
using Finances.Services.Books;
using Finances.Services.Units;
using Finances.Services.Users;
using Finances.Exceptions;
using Xunit;
using Xunit.Abstractions;

using static Xunit.Assert;
using static Finances.Test.Utils.AssertErrors;

namespace Finances.Test.Services {
    [Collection("UnitsService")]
    public class Units : DbTestsBase {
        private readonly IBookStore _bookStore;
        private readonly IUnitStore _unitStore;

        public Units(ITestOutputHelper outputHelper) : base(outputHelper) {
            var userStore = new AppUserStore(Db);
            _bookStore = new BookStore(Db, userStore, CreateLogger<BookStore>());
            _unitStore = new UnitStore(Db, _bookStore, userStore, CreateLogger<UnitStore>());
        }

        [Theory(DisplayName = "Units service - creates an unit")]
        [InlineData("test1", 2)]
        [InlineData("test2", 0)]
        public async Task CreatesNewUnit(string code, int decimals) {
            var book = (await _bookStore.CreateBook("Admin")).Book;

            var result = await _unitStore.CreateUnit(code, decimals, code, book.Id, "Admin");

            Equal(code, result.Code);
            Equal(decimals, result.Decimals);
            True(Db.Units.Any(u => u.Code == code && u.Book.Id == book.Id));
        }

        [Fact(DisplayName = "Units service - returns units for book")]
        public async Task ReturnsUnitsForBook() {
            var book = (await _bookStore.CreateBook("Admin")).Book;

            var units1 = await _unitStore.GetUnits(book.Id);
            False(units1.Any(u => u.Book != null));

            var result = await _unitStore.CreateUnit("test", 2, "test", book.Id, "Admin");

            var units2 = await _unitStore.GetUnits(book.Id);
            Equal(1, units2.Count(u => u.BookId == book.Id));
            False(units2.Any(u => u.BookId.HasValue && u.BookId != book.Id));
            Equal(units1.Length + 1, units2.Length);
        }

        [Fact(DisplayName = "Units service - returns unit by id")]
        public async Task ReturnsUnitById() {
            var bookId = (await _bookStore.CreateBook("Admin")).BookId;

            var unit1 = await _unitStore.GetUnit(0, bookId);
            Null(unit1);

            var result = await _unitStore.CreateUnit("test", 2, "test", bookId, "Admin");

            var unit2 = await _unitStore.GetUnit(result.Id, bookId);
            NotNull(unit2);
            Equal(result.Id, unit2.Id);
            Equal("test", unit2.Text);
        }

        [Fact(DisplayName = "Units service - only an andmin and a member can create units")]
        public async Task OnlyAdminOrMemberCanCreateUnits() {
            var book = (await _bookStore.CreateBook("Admin")).Book;

            await _bookStore.AddUser("Guest", BookUserRole.Guest, book.Id, "Admin");
            await _bookStore.AddUser("Member", BookUserRole.Member, book.Id, "Admin");

            await ExpectAppError(ApplicationError.PermissionDenied, () => _unitStore.CreateUnit("test3", 5, "test3", book.Id, "Guest"));
            False(Db.Units.Any(u => u.Code == "test3"));

            var createByMemberResult = await _unitStore.CreateUnit("test4", 5, "test4", book.Id, "Member");
            var createByAdminResult = await _unitStore.CreateUnit("test5", 5, "test5", book.Id, "Admin");
        }

    }
}