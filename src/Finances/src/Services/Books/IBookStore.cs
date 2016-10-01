using System.Threading.Tasks;
using Finances.Models;

namespace Finances.Services.Books {
    public interface IBookStore {

        Task<BookUser[]> GetUserBooks(string userName);

        Task<BookUser> GetUserBook(string userName, int bookId);

        Task<Book> GetBookById(int bookId);

        Task<BookUserResult> CreateBook(string userName);

        Task<BookUserResult> AddUser(string targetUserName, BookUserRole role, int bookId, string userName);

        Task<BookUserResult> ChangeRole(string targetUserName, BookUserRole role, int bookId, string userName);
    }
}