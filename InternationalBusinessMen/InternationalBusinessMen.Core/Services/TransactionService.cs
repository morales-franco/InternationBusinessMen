using InternationalBusinessMen.Core.Entities;
using InternationalBusinessMen.Core.IRepositories;
using InternationalBusinessMen.Core.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return await _transactionRepository.GetAll();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByProduct(string productId)
        {
            var transactions = await GetAll();
            return transactions.Where(t => t.Sku.ToLower() == productId.ToLower());
        }
    }
}
