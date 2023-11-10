using System.ComponentModel.DataAnnotations;

namespace NetCore.Infrastructure.Entities
{
    /// <summary>
    /// Person Class
    /// </summary>
    public abstract class Vehicle : IVehicle
    {
        [Required]
        public string Model { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string VIN { get; set; }
        [Required]
        public string Color { get; set; }
        public int Year { get; set; }
        public int Tire { get; set; }
        public int Mileage { get; set; }
    }
}