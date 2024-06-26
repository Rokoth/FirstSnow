using FirstSnow.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstSnow.Contract.Interfaces
{
    public interface IPersonDataService
    {
        Task<Response<Person>> GetAsync(Guid id, Guid userId, CancellationToken token);
        Task<Response<EntityList<Person>>> GetAsync(PersonFilter filter, Guid userId, CancellationToken token);
        Task<Response<Person>> UpdateAsync(PersonUpdater model, Guid userId, CancellationToken token);

        Task<Response<Person>> AddAsync(PersonCreator model, Guid userId, CancellationToken token);
    }
}
