using FacilityLeasing.API.Models;

namespace FacilityLeasing.API.Abstract
{
    /// <summary>
    /// Interface defines methods for background placement contract processing.
    /// </summary>
    public interface IContractProcessing
    {
        /// <summary>
        /// Enqueues created contract data for background processing
        /// </summary>
        /// <param name="contractDto">Created contract data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        Task EnqueueCreatedContract(PlacementContractDTO contractDto, CancellationToken cancellationToken);
    }
}
