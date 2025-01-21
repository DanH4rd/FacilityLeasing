using System.ComponentModel.DataAnnotations;

namespace FacilityLeasing.API.Models
{
    /// <summary>
    /// Describes Production Facility entity.
    /// </summary>
    public class ProductionFacility : ModelBase
    {
        [MaxLength(255)]
        public required string Code { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        public double StandardArea { get; set; }
    }
}
