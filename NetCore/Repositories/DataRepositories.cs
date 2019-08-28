using NetCore.Infrastructure.Interfaces;

namespace NetCore.Repositories
{
    public interface IDataRepositories
    {
        CarRepository CarRepository { get; set; }
        TruckRepository TruckRepository { get; set; }
    }
    public class DataRepositories : IDataRepositories
    {
        public CarRepository CarRepository { get; set; }
        public TruckRepository TruckRepository { get; set; }

        public DataRepositories(ICarRepository carRepository, ITruckRepository truckRepository)
        {
            CarRepository = (CarRepository) carRepository;
            TruckRepository = (TruckRepository) truckRepository;
        }
    }
}