using System.Threading.Tasks;
using Finances.Models;

namespace Finances.Services.Units {
    public interface IUnitStore {

        Task<CreateUnitResult> CreateUnit(string code, int decimals, string text, int book, string userName);

        Task<Unit[]> GetUnits(int book);

        Task<Unit> GetUnit(int unitId, int bookId);
        
    }
}