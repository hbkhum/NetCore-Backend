using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.Infrastructure.Entities;

namespace NetCore.Services.Interfaces
{
    public interface ICarService
    {
        Task<bool> CreateCar(Car entity);
        Task<bool> DeleteCar(Guid id);
        Task<IEnumerable<Car>> GetAllCars(string whereValue, string orderBy);
        Task<bool> UpdateCar(Car entity);
        Task<Car> GetCarById(Guid id);
    }
}
