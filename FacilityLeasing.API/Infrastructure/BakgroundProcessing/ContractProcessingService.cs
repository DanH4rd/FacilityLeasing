using FacilityLeasing.API.Models;

namespace FacilityLeasing.API.Infrastructure.BakgroundProcessing
{
    /// <summary>
    /// Simple contract tasks queue processor.
    /// In Azure environment can be alternatively implemented as an Azure Function.
    /// </summary>
    public class ContractProcessingService : BackgroundService
    {
        private readonly ContractTaskQueue _taskQueue;
        private readonly ILogger<ContractProcessingService> _logger;

        public ContractProcessingService(ContractTaskQueue taskQueue, ILogger<ContractProcessingService> logger)
        {
            _taskQueue = taskQueue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Contract background service started.");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var task = await _taskQueue.DequeueAsync(cancellationToken);
                    _logger.LogInformation($"Processing task: {task.FacilityCode},{task.EquipmentCode},{task.EquipmentQuantity}");
                    await ProcessTaskAsync(task);
                }
                catch (OperationCanceledException)
                {
                    // operation cancelled, breaking the loop
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Task processing error.");
                }
            }

            _logger.LogInformation("Contract background service stopped.");
        }

        private Task ProcessTaskAsync(PlacementContractDTO task)
        {
            // processing imitation
            return Task.Delay(1000);
        }
    }

}
