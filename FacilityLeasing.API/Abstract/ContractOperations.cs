using FacilityLeasing.API.Models;
using MediatR;

namespace FacilityLeasing.API.Abstract
{
    public record GetContractsQuery() : IRequest<IEnumerable<PlacementContractDTO>>;

    public record CreateContractCommand(PlacementContractDTO contractDto) : IRequest<(PlacementContract?, string?)>;

    // to notify background processing service
    public record ContractCreatedNotification(PlacementContractDTO contractDto) : INotification;
}
