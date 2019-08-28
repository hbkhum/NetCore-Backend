using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.Infrastructure.Entities;
using NetCore.Repositories;
using NetCore.Services.Interfaces;

namespace NetCore.Services
{
    public class TruckService : ITruckService

    {
        private readonly IDataRepositories _dataRepositories;

        public TruckService(IDataRepositories dataRepositories)
        {
            _dataRepositories = dataRepositories;
        }

        public async Task<bool> CreateTruck(Truck entity)
        {
            try
            {
                var result = false;
                //
                _dataRepositories.TruckRepository.Add(entity);
                _dataRepositories.TruckRepository.Save();
                if (entity.TruckId != Guid.NewGuid()) result = true;
                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteTruck(Guid id)
        {
            var entity = await _dataRepositories.TruckRepository.GetById(id);
            try
            {
                _dataRepositories.TruckRepository.Delete(entity);
                _dataRepositories.TruckRepository.Save();
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<Truck>> GetAllTrucks(string whereValue, string orderBy)
        {
            var result = await _dataRepositories.TruckRepository.GetAllAsync(whereValue, orderBy);
            return result.Select(c => new Truck
            {
                TruckId = c.TruckId,
                Make = c.Make,
                VIN = c.VIN,
                Color = c.Color,
                Model = c.Model
            });
        }

        public async Task<bool> UpdateTruck(Truck entity)
        {
            try
            {
                _dataRepositories.TruckRepository.Edit(entity);
                _dataRepositories.TruckRepository.Save();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return await Task.FromResult(true);
        }

        public async Task<Truck> GetTruckById(Guid id)
        {
            return await _dataRepositories.TruckRepository.GetById(id);
        }
    }
}