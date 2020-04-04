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
    public class RateRepository : IRateRepository
    {
        private readonly IRateExternalProvider _rateExternalProvider;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RateRepository> _logger;

        public RateRepository(IRateExternalProvider rateExternalProvider,
            ApplicationDbContext context,
            ILogger<RateRepository> logger)
        {
            _rateExternalProvider = rateExternalProvider;
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Rate>> GetAll()
        {
            IEnumerable<Rate> rates = new List<Rate>();
            try
            {
                rates = await _rateExternalProvider.GetAllRates();
                await RefreshAlternativeStorage(rates);
                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed retrieve Rates from WebService ", ex);

            }

            _logger.LogInformation("Retrieving rates from alternative storage");
            rates = await GetAllRatesFromAlternativeStorage();
            return rates;
        }

        private async Task<IEnumerable<Rate>> GetAllRatesFromAlternativeStorage()
        {
            return await _context.Rates.ToListAsync();
        }

        private async Task RefreshAlternativeStorage(IEnumerable<Rate> ratesFromWebService)
        {
            await DeleteAll();
            await Insert(ratesFromWebService);
        }

        private async Task DeleteAll()
        {
            var allRates = await GetAllRatesFromAlternativeStorage();
            _context.Rates.RemoveRange(allRates);
            await _context.SaveChangesAsync();
        }

        private async Task Insert(IEnumerable<Rate> ratesFromWebService)
        {
            await _context.Rates.AddRangeAsync(ratesFromWebService);
            await _context.SaveChangesAsync();
        }
    }
}
