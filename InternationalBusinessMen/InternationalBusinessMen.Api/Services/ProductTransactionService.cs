using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InternationalBusinessMen.Api.Dtos;
using InternationalBusinessMen.Core.Constants;
using InternationalBusinessMen.Core.IServices;

namespace InternationalBusinessMen.Api.Services
{
    public class ProductTransactionService : IProductTransactionService
    {
        private readonly IRateService _rateService;
        private readonly ITransactionService _transactionService;
        private readonly IExchangeService _exchangeService;
        private readonly IMapper _mapper;

        public ProductTransactionService(ITransactionService transactionService,
            IRateService rateService,
            IExchangeService exchangeService,
            IMapper mapper)
        {
            _rateService = rateService;
            _transactionService = transactionService;
            _mapper = mapper;
            _exchangeService = exchangeService;
        }

        public async Task<ProducTransactionsDto> CreateModel(string productId)
        {
            ProducTransactionsDto model = new ProducTransactionsDto();

            var transactions = await _transactionService.GetTransactionsByProduct(productId);
            var rates = await _rateService.GetAll();

            _exchangeService.ConvertTransactionsToCurrency(CurrencyCodes.EUR, transactions, rates);

            model.Transactions = _mapper.Map<IList<TransactionDto>>(transactions);
            model.Total = model.Transactions.Select(t => t.Amount).Sum();

            return model;
        }
    }
}
