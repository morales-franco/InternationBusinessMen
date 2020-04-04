using InternationalBusinessMen.Api.Dtos;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Api.Services
{
    public interface IProductTransactionService
    {
        Task<ProducTransactionsDto> CreateModel(string productId);
    }
}
