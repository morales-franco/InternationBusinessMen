using Newtonsoft.Json;

namespace InternationalBusinessMen.Core.Entities
{
    public class Transaction: BaseEntity
    {
        [JsonProperty]
        public string Sku { get; private set; }

        [JsonProperty]
        public decimal Amount { get; private set; }

        [JsonProperty]
        public string Currency { get; private set; }

        public Transaction(string sku, decimal amount, string currency)
        {
            Sku = sku;
            Amount = amount;
            Currency = currency;
        }

        public void SetAmount(decimal amount)
        {
            Amount = amount;
        }

        public void SetCurrency(string currency)
        {
            Currency = currency;
        }
    }
}
