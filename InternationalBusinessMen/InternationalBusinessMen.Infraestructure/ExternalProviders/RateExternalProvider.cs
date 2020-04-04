using InternationalBusinessMen.Core.Entities;
using InternationalBusinessMen.Infraestructure.ExternalProviders;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Infraestructure.ExternalServices
{
    public class RateExternalProvider : IRateExternalProvider
    {
        private readonly HttpClient _httpClient;

        public RateExternalProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Rate>> GetAllRates()
        {
            using (var response = await _httpClient.GetAsync("rates.json"))
            {
                response.EnsureSuccessStatusCode();
                var responseAsString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<Rate>>(responseAsString);
            }
        }
    }
}
