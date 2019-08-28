using System;
using System.Threading.Tasks;
using NetCore.Infrastructure.Core;
using NetCore.Infrastructure.Entities;

namespace NetCore.Infrastructure.Interfaces
{
    public interface ITruckRepository : IGenericRepositoryEntities<Truck>
    {
        /// <summary>
        /// Get the Car Id
        /// </summary>
        /// <param name="id">Is the Id from the database</param>
        /// <returns>Return a object type Business</returns>
        Task<Truck> GetById(Guid id);

    }
}