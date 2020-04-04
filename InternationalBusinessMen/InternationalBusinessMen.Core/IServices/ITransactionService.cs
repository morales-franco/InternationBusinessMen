using InternationalBusinessMen.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Core.IServices
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAll();
        Task<IEnumerable<Transaction>> GetTransactionsByProduct(string productId);
    }
}
