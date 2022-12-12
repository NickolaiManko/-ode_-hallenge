using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindPark.API.Entities;

namespace WindPark.API.Repositories
{
    public class WindParkRepository : IWindParkRepository
    {
        protected readonly DataContext _dbContext;

        public WindParkRepository(DataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> AddWindPark(WindParkEntity item)
        {
            _dbContext.WindParks.Add(item);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<WindParkEntity>> GetWindParks()
        {
            return await _dbContext.WindParks.AsNoTracking()
                                             .Include(o => o.Turbines)
                                             .ToListAsync();
        }
    }
}
