using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.Infrastructure.Entities
{
    /// <summary>
    /// Concrete Class
    /// </summary>
    public class Truck : Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TruckId { get; set; }
    }
}