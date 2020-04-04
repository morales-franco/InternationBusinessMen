using InternationalBusinessMen.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Core.IServices
{
    public interface IRateService
    {
        Task<IEnumerable<Rate>> GetAll();
    }
}
