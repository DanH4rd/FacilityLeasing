namespace FacilityLeasing.API.Models
{
    /// <summary>
    /// Describes model base structure.
    /// </summary>
    public abstract class ModelBase
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
