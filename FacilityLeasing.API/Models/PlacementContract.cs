namespace FacilityLeasing.API.Models
{
    public class PlacementContract : ModelBase
    {
        public int ProductionFacilityId { get; set; }

        public int ProcessEquipmentId { get; set; }

        public int EquipmentUnits { get; set; }

        // navigation properties
        public ProductionFacility ProductionFacility { get; set; } = null!;
        public ProcessEquipment ProcessEquipment { get; set; } = null!;
    }
}
