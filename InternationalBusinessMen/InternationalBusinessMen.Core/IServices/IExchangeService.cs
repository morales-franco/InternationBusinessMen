using InternationalBusinessMen.Core.Entities;
using System.Collections.Generic;

namespace InternationalBusinessMen.Core.IServices
{
    public interface IExchangeService
    {
        void ConvertTransactionsToCurrency(string currencyTarget, IEnumerable<Transaction> transactions, IEnumerable<Rate> rates);
    }
}
