using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NetCore.Infrastructure.Entities;

namespace NetCore.Hub
{
    public interface ITruckHub
    {
        Task AddTruck(Truck truck);
        Task UpdateTruck(Truck truck);
        Task DeleteTruck(Truck truck);
    }
    public class TruckHub : Hub<ITruckHub>
    {
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId,"");
            await base.OnDisconnectedAsync(exception);
        }
        public async Task<string> AssociateTruck(string truckId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, truckId);
            return Context.ConnectionId;
        }

        public async Task<bool> DeleteTruck(string truckId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, truckId);
            return true;
        }
    }
}