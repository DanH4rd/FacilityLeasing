using System.ComponentModel.DataAnnotations;

namespace FacilityLeasing.API.Models
{
    /// <summary>
    /// Describes fields required for a contract creation.
    /// </summary>
    public class PlacementContractDTO
    {
        [MaxLength(255)]
        public required string FacilityCode { get; set; }

        [MaxLength(255)]
        public required string EquipmentCode { get; set; }

        public int EquipmentQuantity { get; set; }
    }
}
