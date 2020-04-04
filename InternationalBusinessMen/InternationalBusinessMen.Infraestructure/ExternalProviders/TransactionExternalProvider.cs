using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InternationalBusinessMen.Core.Entities;
using Newtonsoft.Json;

namespace InternationalBusinessMen.Infraestructure.ExternalProviders
{
    public class TransactionExternalProvider : ITransactionExternalProvider
    {
        private readonly HttpClient _httpClient;

        public TransactionExternalProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            using (var response = await _httpClient.GetAsync("transactions.json"))
            {
                response.EnsureSuccessStatusCode();
                var responseAsString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<Transaction>>(responseAsString);
            }
        }
    }
}
