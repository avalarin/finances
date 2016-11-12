using System.Threading.Tasks;
using Finances.Models;

namespace Finances.Services.Products {
    public interface IProductStore {
         
         Task<Product> CreateProduct(string title, int unitId, int bookId, string userName);

         Task<Product[]> GetProducts(int book);

         Task<Product> GetProduct(int productId, int bookId);
    }
}