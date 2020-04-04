using InternationalBusinessMen.Core.Entities;
using InternationalBusinessMen.Core.IRepositories;
using InternationalBusinessMen.Infraestructure.ExternalProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Infraestructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ITransactionExternalProvider _transactionExternalProvider;
        private readonly ILogger<RateRepository> _logger;
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ITransactionExternalProvider transactionExternalProvider,
            ApplicationDbContext context,
            ILogger<RateRepository> logger)
        {
            _context = context;
            _transactionExternalProvider = transactionExternalProvider;
            _logger = logger;
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            IEnumerable<Transaction> transactions = new List<Transaction>();
            try
            {
                transactions = await _transactionExternalProvider.GetAllTransactions();
                await RefreshAlternativeStorage(transactions);
                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieve Transactions from WebService ", ex);
            }

            _logger.LogInformation("Retrieving transactions from alternative storage");
            transactions = await GetAllTransactionsFromAlternativeStorage();
            return transactions;
        }

        private async Task<IEnumerable<Transaction>> GetAllTransactionsFromAlternativeStorage()
        {
            return await _context.Transactions.ToListAsync();
        }


        private async Task RefreshAlternativeStorage(IEnumerable<Transaction> ratesFromWebService)
        {
            await DeleteAll();
            await Insert(ratesFromWebService);
        }

        private async Task DeleteAll()
        {
            var allTransactions = await GetAllTransactionsFromAlternativeStorage();
            _context.Transactions.RemoveRange(allTransactions);
            await _context.SaveChangesAsync();
        }

        private async Task Insert(IEnumerable<Transaction> transactionsFromWebService)
        {
            await _context.Transactions.AddRangeAsync(transactionsFromWebService);
            await _context.SaveChangesAsync();
        }
    }
}
