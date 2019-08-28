using System;
using System.Linq;
using System.Threading.Tasks;
using NetCore.Infrastructure.Context;
using NetCore.Infrastructure.Core;
using NetCore.Infrastructure.Entities;
using NetCore.Infrastructure.Interfaces;

namespace NetCore.Repositories
{



    public class TruckRepository : GenericRepositoryEntities<Truck>, ITruckRepository
    {
        private readonly VehicleContext _context;

        public TruckRepository(VehicleContext entities)
            : base(entities)
        {
            _context = entities;
        }

        public async Task<Truck> GetById(Guid id)
        {
            var task = Task.Run(() => Dbset.FirstOrDefault(c => c.TruckId == id));
            return await task;

        }
    }
}