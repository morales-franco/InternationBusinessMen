using InternationalBusinessMen.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Core.IRepositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAll();
    }
}
