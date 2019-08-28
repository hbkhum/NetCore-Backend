using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCore.Infrastructure.Entities;

namespace NetCore.Services.Interfaces
{
    public interface ITruckService
    {
        Task<bool> CreateTruck(Truck entity);
        Task<bool> DeleteTruck(Guid id);
        Task<IEnumerable<Truck>> GetAllTrucks(string whereValue, string orderBy);
        Task<bool> UpdateTruck(Truck entity);
        Task<Truck> GetTruckById(Guid id);
    }
}