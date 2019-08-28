using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.Infrastructure.Entities;
using Microsoft.AspNetCore.SignalR;

namespace NetCore.Hub
{
    public interface ICarHub
    {
        Task AddCar(Car car);
        Task UpdateCar(Car car);
        Task DeleteCar(Car car);
    }
    public class CarHub : Hub<ICarHub>
    {
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId,"");
            await base.OnDisconnectedAsync(exception);
        }
        public async Task<string> AssociateCar(string carId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, carId);
            return Context.ConnectionId;
        }

        public async Task<bool> DeleteCar(string carId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, carId);
            return true;
        }
    }


}
