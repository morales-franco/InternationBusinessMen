using System.Collections.Generic;

namespace InternationalBusinessMen.Api.Dtos
{
    public class ProducTransactionsDto
    {
        public IList<TransactionDto> Transactions { get; set; }
        public decimal Total { get; set; }
    }
}
