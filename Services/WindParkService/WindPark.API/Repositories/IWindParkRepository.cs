using System.Collections.Generic;
using System.Threading.Tasks;
using WindPark.API.Entities;

namespace WindPark.API.Repositories
{
    public interface IWindParkRepository
    {
        Task<bool> AddWindPark(WindParkEntity item);
        Task<IEnumerable<WindParkEntity>> GetWindParks();
    }
}
