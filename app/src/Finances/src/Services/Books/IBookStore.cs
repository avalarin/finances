using System.Threading.Tasks;
using Finances.Models;

namespace Finances.Services.Books {
    public interface IBookStore {

        Task<BookUser[]> GetUserBooks(string userName);

        Task<BookUser> GetUserBook(string userName, int bookId);

        Task<Book> GetBookById(int bookId);

        Task<BookUser> CreateBook(string userName);

        Task<BookUser> AddUser(string targetUserName, BookUserRole role, int bookId, string userName);

        Task<BookUser> ChangeRole(string targetUserName, BookUserRole role, int bookId, string userName);
    }
}