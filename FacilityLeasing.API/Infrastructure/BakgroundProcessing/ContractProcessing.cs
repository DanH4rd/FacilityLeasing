using FacilityLeasing.API.Abstract;
using FacilityLeasing.API.Models;

namespace FacilityLeasing.API.Infrastructure.BakgroundProcessing
{
    public class ContractProcessing : IContractProcessing
    {
        private readonly ContractTaskQueue _taskQueue;

        public ContractProcessing(ContractTaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }

        public Task EnqueueCreatedContract(PlacementContractDTO contractDto, CancellationToken cancellationToken)
        {
            _taskQueue.Enqueue(contractDto);
            return Task.CompletedTask;
        }
    }
}
