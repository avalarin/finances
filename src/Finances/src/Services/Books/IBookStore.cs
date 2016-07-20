using System.Threading.Tasks;
using Finances.Models;

namespace Finances.Services.Books {
    public interface IBookStore {

        Task<BookUser[]> GetUserBooks(string username);

        Task<BookUser> CreateBook(string username);

    }
}