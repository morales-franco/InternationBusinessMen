using InternationalBusinessMen.Core.Entities;
using InternationalBusinessMen.Core.IRepositories;
using InternationalBusinessMen.Core.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Core.Services
{
    public class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;

        public RateService(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public async Task<IEnumerable<Rate>> GetAll()
        {
            return await _rateRepository.GetAll();
        }
    }
}
