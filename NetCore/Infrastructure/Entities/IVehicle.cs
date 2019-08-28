using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Entities
{
    /// <summary>
    /// This is a Interface with Elements
    /// </summary>
    public interface IVehicle
    {
        string Model { get; set; }
        string Make { get; set; }
        string VIN { get; set; }
        string Color { get; set; }
        int Year { get; set; }
    }
}
