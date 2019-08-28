using NetCore.Services.Interfaces;

namespace NetCore.Services
{
    public interface IDataServices
    {
        CarService CarService { get; set; }
        TruckService TruckService { get; set; }
    }
    public class DataServices : IDataServices
    {
        public DataServices(ICarService carService, ITruckService truckService)
        {
            CarService = (CarService) carService;
            TruckService = (TruckService) truckService;
        }

        public CarService CarService { get; set; }
        public TruckService TruckService { get; set; }
    }
}