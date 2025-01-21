using System.ComponentModel.DataAnnotations;

namespace FacilityLeasing.API.Models
{
    public class ProcessEquipment : ModelBase
    {
        [MaxLength(255)]
        public required string Code { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        public double Area { get; set; }
    }
}
