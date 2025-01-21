using FacilityLeasing.API.Abstract;
using FacilityLeasing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FacilityLeasing.API.Infrastructure
{
    public class ContractRepository : IContractRepository
    {
        private readonly FacilityDBContext _context;

        public ContractRepository(FacilityDBContext context)
        {
            _context = context;
        }

        public async Task<PlacementContract> CreateContractAsync(PlacementContractDTO contractDto, CancellationToken cancellationToken)
        {
            var facility = await _context.ProductionFacilities.AsNoTracking()
                .FirstAsync(f => f.Code == contractDto.FacilityCode && f.IsActive, cancellationToken);

            var equipment = await _context.ProcessEquipment.AsNoTracking()
                .FirstAsync(eq => eq.Code == contractDto.EquipmentCode && eq.IsActive, cancellationToken);

            var contract = new PlacementContract
            {
                ProductionFacilityId = facility.Id,
                ProcessEquipmentId = equipment.Id,
                EquipmentUnits = contractDto.EquipmentQuantity,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _context.PlacementContracts.AddAsync(contract, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return contract;
        }

        public async Task<List<PlacementContractDTO>> GetAllContractsAsync(CancellationToken cancellationToken)
        {
            var result = await _context.PlacementContracts
                .AsNoTracking()
                .Include(contract => contract.ProductionFacility)
                .Include(contract => contract.ProcessEquipment)
                .Select(contract => new PlacementContractDTO
                {
                    FacilityCode = contract.ProductionFacility.Code,
                    EquipmentCode = contract.ProcessEquipment.Code,
                    EquipmentQuantity = contract.EquipmentUnits
                })
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<double> GetAvailableFacilityAreaAsync(string facilityCode, CancellationToken cancellationToken)
        {
            var facility = await _context.ProductionFacilities.AsNoTracking()
                .FirstOrDefaultAsync(f => f.Code == facilityCode && f.IsActive, cancellationToken);

            if (facility == null)
            {
                throw new ArgumentException($"Facility with code '{facilityCode}' does not exist or inactive.");
            }

            var totalUsedArea = await _context.PlacementContracts.AsNoTracking()
                .Where(c => c.ProductionFacilityId == facility.Id)
                .Join(_context.ProcessEquipment,
                      contract => contract.ProcessEquipmentId,
                      equipment => equipment.Id,
                      (contract, equipment) => new { contract.EquipmentUnits, equipment.Area })
                .SumAsync(x => x.EquipmentUnits * x.Area, cancellationToken);

            // Calculate available area
            return facility.StandardArea - totalUsedArea;
        }

        public async Task<ProcessEquipment?> GetEquipmentByCodeAsync(string code, CancellationToken cancellationToken)
        {
            var equipment = await _context.ProcessEquipment.AsNoTracking()
                .FirstOrDefaultAsync(pe => pe.Code == code && pe.IsActive, cancellationToken);
            return equipment;
        }
    }
}
