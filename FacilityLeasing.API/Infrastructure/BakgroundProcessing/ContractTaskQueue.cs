using FacilityLeasing.API.Models;
using System.Collections.Concurrent;

namespace FacilityLeasing.API.Infrastructure.BakgroundProcessing
{
    /// <summary>
    /// Simple in-memory tasks queue implementation. 
    /// In Azure environment can be alternatively implemented via Azure Queue Storage.
    /// </summary>
    public class ContractTaskQueue
    {
        private readonly ConcurrentQueue<PlacementContractDTO> _queue = new();
        private readonly SemaphoreSlim _signal = new(0);

        // add new task
        public void Enqueue(PlacementContractDTO task)
        {
            _queue.Enqueue(task);
            _signal.Release(); // signal for a new task
        }

        // dequeue task
        public async Task<PlacementContractDTO> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken); // wait for a signal to dequeue task
            _queue.TryDequeue(out var task);
            return task ?? throw new ArgumentNullException();
        }
    }
}
