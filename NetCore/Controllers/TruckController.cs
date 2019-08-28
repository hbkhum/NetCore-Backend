using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using NetCore.Hub;
using NetCore.Infrastructure.Context;
using NetCore.Infrastructure.Entities;
using NetCore.Services;
namespace NetCore.Controllers
{
    namespace NetCore.Controllers
    {
        [Produces("application/json")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/Truck")]
        [EnableCors("CorsPolicy")]
        public class TruckController : Controller
        {
            private readonly IDataServices _dataServices;
            private readonly IHubContext<TruckHub, ITruckHub> _hub;

            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IConfiguration _configuration;

            public TruckController(IDataServices dataServices, IHubContext<TruckHub, ITruckHub> hub, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
            {
                _dataServices = dataServices;
                _hub = hub;
                _userManager = userManager;
                _signInManager = signInManager;
                _configuration = configuration;
            }

            [HttpGet]
            [Route("{pageSize:int?}/{pageNumber:int?}/{orderby:alpha?}/{wherevalue:alpha?}/{type:alpha?}")]
            public async Task<IActionResult> Get(int pageSize = 5000, int pageNumber = 1, string whereValue = "", string orderBy = "", string type = "json")
            {
                whereValue = whereValue ?? "";
                var result = await _dataServices.TruckService.GetAllTrucks(whereValue, orderBy);
                var data = result as IList<Truck> ?? result.ToList();
                var totalCount = data.Count();
                var totalPages = Math.Ceiling((double)totalCount / pageSize);
                if (type == "json")
                {
                    var results = new
                    {
                        TotalCount = totalCount,
                        totalPages = Math.Ceiling((double)totalCount / pageSize),
                        result = data
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList()
                    };
                    return Ok(results);
                }
                return null;
            }
            // GET api/Truck/5
            [HttpGet]
            [Route("{id:Guid}")]
            public async Task<IActionResult> Get(Guid id, string type = "json")
            {
                var result = await _dataServices.TruckService.GetTruckById(id);
                if (type == "json")
                {
                    return Ok(result);
                }
                return null;
            }
            // POST
            [HttpPost]
            public async Task<Guid?> Post([FromBody] Truck truck)
            {
                var result = await _dataServices.TruckService.CreateTruck(truck);
                if (!result) return null;
                //SignalR Methods Add Element
                await _hub.Clients.All.AddTruck(truck);
                var task = Task.Run(() => truck.TruckId);
                return await task;
            }
            // UPDATE api/Truck/5
            [HttpPut("{id:Guid}")]
            public async Task<bool> Put(Guid id, [FromBody] Truck truck)
            {
                var result = await _dataServices.TruckService.UpdateTruck(truck);
                if (!result) return await Task.Run(() => false);
                //SignalR Methods Update
                await _hub.Clients.All.UpdateTruck(truck);
                var task = Task.Run(() => true);
                return await task;

            }
            // DELETE api/Truck/5
            [HttpDelete("{id:Guid}")]
            public async Task<bool> Delete(Guid id)
            {
                var category = await _dataServices.TruckService.GetTruckById(id);
                var result = await _dataServices.TruckService.DeleteTruck(id);
                if (!result) return await Task.Run(() => false);
                //SignalR Methods Delete
                await _hub.Clients.All.DeleteTruck(category);
                var task = Task.Run(() => true);
                return await task;

            }
        }
    }

}