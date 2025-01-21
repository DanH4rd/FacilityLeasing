using FacilityLeasing.API.Models;
using MediatR;

namespace FacilityLeasing.API.Abstract
{
    public abstract class QCHandlerBase
    {
        protected readonly IContractRepository _contractRepo;
        public QCHandlerBase(IContractRepository contractRepo) => _contractRepo = contractRepo;
    }

    // get all contracts
    public class GetContractsHandler : QCHandlerBase, IRequestHandler<GetContractsQuery, IEnumerable<PlacementContractDTO>>
    {
        public GetContractsHandler(IContractRepository contractRepo) : base(contractRepo) { }

        public async Task<IEnumerable<PlacementContractDTO>> Handle(GetContractsQuery request,
            CancellationToken cancellationToken) => await _contractRepo.GetAllContractsAsync(cancellationToken);
    }

    // create contract
    public class CreateContractHandler : QCHandlerBase, IRequestHandler<CreateContractCommand, (PlacementContract?, string?)>
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public CreateContractHandler(IContractRepository contractRepo) : base(contractRepo) { }

        public async Task<(PlacementContract?, string?)> Handle(CreateContractCommand request, CancellationToken cancellationToken)
        {
            var equipment = await _contractRepo.GetEquipmentByCodeAsync(
                request.contractDto.EquipmentCode, cancellationToken)
                ?? throw new ArgumentException("Process equipment not found.");

            // use sync here to prevent concurrent placing of new contracts between the checks
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var availableArea = await _contractRepo.GetAvailableFacilityAreaAsync(
                    request.contractDto.FacilityCode, cancellationToken);

                var requestedArea = equipment.Area * request.contractDto.EquipmentQuantity;
                var isContractFeasible = availableArea - requestedArea >= 0;

                if (!isContractFeasible)
                {
                    var error = $"Contract is not feasible: requested area ({requestedArea}) exceeds available value ({availableArea}).";
                    return (null, error);
                }

                var contract = await _contractRepo.CreateContractAsync(request.contractDto, cancellationToken);
                return (contract, null);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
