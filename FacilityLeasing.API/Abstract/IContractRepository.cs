using FacilityLeasing.API.Models;

namespace FacilityLeasing.API.Abstract
{
    /// <summary>
    /// Interface defines methods to work with contracts data.
    /// </summary>
    public interface IContractRepository
    {
        /// <summary>
        /// Retrieves current list of contracts.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>List of contracts.</returns>
        Task<List<PlacementContractDTO>> GetAllContractsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new contract.
        /// </summary>
        /// <param name="contractDto">Required data to create a contract.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>Created contract data.</returns>
        Task<PlacementContract> CreateContractAsync(PlacementContractDTO contractDto, CancellationToken cancellationToken);

        /// <summary>
        /// Gets available facility area.
        /// </summary>
        /// <param name="contractDto"></param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>Available area.</returns>
        Task<double> GetAvailableFacilityAreaAsync(string facilityCode, CancellationToken cancellationToken);

        /// <summary>
        /// Gets process equipment by its code.
        /// </summary>
        /// <param name="code">Process equipment code.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>Process equipment data if found, otherwise null.</returns>
        Task<ProcessEquipment?> GetEquipmentByCodeAsync(string code, CancellationToken cancellationToken);
    }
}
