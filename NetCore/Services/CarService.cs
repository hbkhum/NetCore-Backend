using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.Infrastructure.Entities;
using NetCore.Repositories;
using NetCore.Services.Interfaces;

namespace NetCore.Services
{
    public class CarService:ICarService

    {
        private readonly IDataRepositories _dataRepositories;

        public CarService(IDataRepositories dataRepositories)
        {
            _dataRepositories = dataRepositories;
        }

        public async Task<bool> CreateCar(Car entity)
        {
            try
            {
                var result = false;
                //
                _dataRepositories.CarRepository.Add(entity);
                _dataRepositories.CarRepository.Save();
                if (entity.CarId != Guid.NewGuid()) result = true;
                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteCar(Guid id)
        {
            var entity = await _dataRepositories.CarRepository.GetById(id);
            try
            {
                _dataRepositories.CarRepository.Delete(entity);
                _dataRepositories.CarRepository.Save();
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<Car>> GetAllCars(string whereValue, string orderBy)
        {
            var result = await _dataRepositories.CarRepository.GetAllAsync(whereValue, orderBy);
            return result.Select(c => new Car
            {
                CarId = c.CarId,
                Make = c.Make,
                VIN = c.VIN,
                Color = c.Color,
                Model = c.Model
            });
        }

        public async Task<bool> UpdateCar(Car entity)
        {
            try
            {
                _dataRepositories.CarRepository.Edit(entity);
                _dataRepositories.CarRepository.Save();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return await Task.FromResult(true);
        }

        public  async Task<Car> GetCarById(Guid id)
        {
            return await _dataRepositories.CarRepository.GetById(id);
        }
    }
}
