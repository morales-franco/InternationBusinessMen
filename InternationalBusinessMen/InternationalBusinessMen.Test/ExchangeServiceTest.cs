using InternationalBusinessMen.Core.Constants;
using InternationalBusinessMen.Core.Entities;
using InternationalBusinessMen.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternationalBusinessMen.Test
{
    [TestClass]
    public class ExchangeServiceTest
    {
        [TestMethod]
        public void ConvertTransactionsToCurrency_Simple_Test()
        {
            //Arrange
            var exchangeService = new ExchangeService();
            var allTransactions = TestRepository.GetTransactionsByProduct("T2006").ToList();

            exchangeService.ConvertTransactionsToCurrency(CurrencyCodes.EUR,
                                                          allTransactions,
                                                          TestRepository.GetAllRates());

            //Act
            var total = allTransactions.Sum(x => x.Amount);

            // Assert
            Assert.AreEqual(14.99M, total);
        }

        [TestMethod]
        public void ConvertTransactionsToCurrency_RecursiveWay_Test()
        {
            //Arrange

            IEnumerable<Transaction> transactions = new List<Transaction>()
            {
                    new Transaction("T1",10, "USD")
            };

            //USD --> PESOS ==> The routine has to iterate recursively to get the currency exchange
            IEnumerable<Rate> rates = TestRepository.GetComplexRatesDataSource();

            var exchangeService = new ExchangeService();


            exchangeService.ConvertTransactionsToCurrency(CurrencyCodes.PESOS,
                                                          transactions,
                                                          rates);

            //Act
            var total = transactions.Sum(x => x.Amount);

            // Assert
            Assert.AreEqual(180M, total);
        }

        

    }
}
