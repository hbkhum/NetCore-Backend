using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.Infrastructure.Context;
using NetCore.Infrastructure.Core;
using NetCore.Infrastructure.Entities;
using NetCore.Infrastructure.Interfaces;

namespace NetCore.Repositories
{
    public class CarRepository : GenericRepositoryEntities<Car>, ICarRepository
    {
        private readonly VehicleContext _context;

        public CarRepository(VehicleContext entities)
            : base(entities)
        {
            _context = entities;
        }

        public async Task<Car> GetById(Guid id)
        {
            var task = Task.Run(() => Dbset.FirstOrDefault(c => c.CarId == id));
            return await task;

        }
    }
}
