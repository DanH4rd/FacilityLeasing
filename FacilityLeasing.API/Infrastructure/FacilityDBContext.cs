using FacilityLeasing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FacilityLeasing.API.Infrastructure
{
    /// <summary>
    /// Describes Facility Leasing data context.
    /// </summary>
    public class FacilityDBContext : DbContext
    {
        public required DbSet<ProductionFacility> ProductionFacilities { get; set; }

        public required DbSet<ProcessEquipment> ProcessEquipment { get; set; }

        public required DbSet<PlacementContract> PlacementContracts { get; set; }

        public FacilityDBContext(DbContextOptions<FacilityDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure production facility
            modelBuilder.Entity<ProductionFacility>().HasKey(f => f.Id);

            modelBuilder.Entity<ProductionFacility>()
                .HasIndex(f => f.Code)
                .IsUnique()
                .HasDatabaseName("IX_Facility_Code");

            // configure process equipment
            modelBuilder.Entity<ProcessEquipment>().HasKey(pe => pe.Id);

            modelBuilder.Entity<ProcessEquipment>()
                .HasIndex(pe => pe.Code)
                .IsUnique()
                .HasDatabaseName("IX_Equipment_Code");

            // configure placement contract
            modelBuilder.Entity<PlacementContract>().HasKey(pc => pc.Id);

            modelBuilder.Entity<PlacementContract>()
                .HasOne<ProductionFacility>(pc => pc.ProductionFacility)
                .WithMany()
                .HasForeignKey(pc => pc.ProductionFacilityId)
                .HasConstraintName("FK_PlacementContract_ProductionFacility")
                .OnDelete(DeleteBehavior.Restrict); // restrict removal if there are related contracts

            modelBuilder.Entity<PlacementContract>()
                .HasOne<ProcessEquipment>(pc => pc.ProcessEquipment)
                .WithMany()
                .HasForeignKey(pc => pc.ProcessEquipmentId)
                .HasConstraintName("FK_PlacementContract_ProcessEquipment")
                .OnDelete(DeleteBehavior.Restrict); // restrict removal if there are related contracts

            modelBuilder.Entity<PlacementContract>()
                .HasIndex(pc => pc.ProductionFacilityId)
                .HasDatabaseName("IX_PlacementContract_ProductionFacilityId");

            modelBuilder.Entity<PlacementContract>()
                .HasIndex(pc => pc.ProcessEquipmentId)
                .HasDatabaseName("IX_PlacementContract_ProcessEquipmentId");
        }
    }
}
