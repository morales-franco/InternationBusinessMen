using InternationalBusinessMen.Core.Entities;
using InternationalBusinessMen.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternationalBusinessMen.Core.Services
{
    public class ExchangeService : IExchangeService
    {
        private Dictionary<string, List<Rate>> _currencyTree;
        public ExchangeService()
        {
        }

        public void ConvertTransactionsToCurrency(string currencyTarget, IEnumerable<Transaction> transactions, IEnumerable<Rate> rates)
        {
            foreach (var transaction in transactions)
            {
                var newAmount = transaction.Amount * GetExchangeRate(transaction.Currency, currencyTarget, rates);

                transaction.SetAmount(RoundToEven(newAmount));
                transaction.SetCurrency(currencyTarget);

            }
        }

        private decimal RoundToEven(decimal newAmount, int decimals = 2)
        {
            return Math.Round(newAmount, decimals, MidpointRounding.ToEven);
        }

        private decimal GetExchangeRate(string currencyFrom, string currencyTarget, IEnumerable<Rate> rates)
        {
            if(currencyFrom == currencyTarget)
            {
                return 1;
            }

            var existCurrencyFromInRates = rates.Any(r => r.From.ToLower() == currencyFrom.ToLower());
            if (!existCurrencyFromInRates)
            {
                throw new Exception($"Currency exchange { currencyFrom } not found");
            }

            var perfectExchangeRateMatch = rates.FirstOrDefault(r => r.From.ToLower() == currencyFrom.ToLower() &&
                                                                     r.To.ToLower() == currencyTarget.ToLower());

            if (perfectExchangeRateMatch != null)
            {
                return perfectExchangeRateMatch.RateValue;
            }

            TryCreateExchangeTree(rates);

            var ratesLink = new List<Rate>();
            FindLinkToDoCurrencyExchangeFromTree(currencyFrom, currencyTarget, ref ratesLink);

            if (!ratesLink.Any())
            {
                throw new Exception($"Can not find a way to do the Currency exchange from { currencyFrom } to Currency Target { currencyTarget }");
            }

            decimal exchangeRate = 1;
            ratesLink.ForEach(r => exchangeRate *= r.RateValue);
            return exchangeRate;
        }

        private void FindLinkToDoCurrencyExchangeFromTree(string currencyFrom, string currencyTarget, ref List<Rate> ratesLink)
        {
            var currencyBranch = _currencyTree[currencyFrom];

            foreach (var currency in currencyBranch)
            {
                if (ratesLink.Any(r => r.From == currency.To))
                {
                    continue;
                }

                ratesLink.Add(currency);

                FindLinkToDoCurrencyExchangeFromTree(currency.To, currencyTarget, ref ratesLink);

                if (ratesLink.Any(r => r.To == currencyTarget))
                {
                    break;
                }
                else
                {
                    ratesLink.Remove(currency);
                }
            }
        }

        private void TryCreateExchangeTree(IEnumerable<Rate> rates)
        {
            if (_currencyTree != null)
            {
                return;
            }

            InitExchangeTree(rates);
        }

        private void InitExchangeTree(IEnumerable<Rate> rates)
        {
            _currencyTree = new Dictionary<string, List<Rate>>();

            foreach (var rate in rates)
            {
                List<Rate> children = rates.Where(r => r.From.ToLower() == rate.From.ToLower()).ToList();

                if (_currencyTree.ContainsKey(rate.From))
                {
                    var currentChildren = _currencyTree[rate.From];
                    currentChildren.AddRange(children);
                    _currencyTree[rate.From] = children;
                }
                else
                {
                    _currencyTree.Add(rate.From, children);
                }
            }
        }
    }
}
