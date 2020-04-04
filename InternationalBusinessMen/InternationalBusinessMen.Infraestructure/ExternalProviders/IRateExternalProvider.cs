using InternationalBusinessMen.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Infraestructure.ExternalProviders
{
    public interface IRateExternalProvider
    {
        Task<IEnumerable<Rate>> GetAllRates();
    }
}
