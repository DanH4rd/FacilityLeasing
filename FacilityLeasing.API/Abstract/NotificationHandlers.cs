using MediatR;

namespace FacilityLeasing.API.Abstract
{
    public class ContractCreatedNotificationHandler : INotificationHandler<ContractCreatedNotification>
    {
        private readonly IContractProcessing _contractProcessing;
        private readonly ILogger<ContractCreatedNotificationHandler> _logger;

        public ContractCreatedNotificationHandler(IContractProcessing contractProcessing,
            ILogger<ContractCreatedNotificationHandler> logger)
        {
            _contractProcessing = contractProcessing;
            _logger = logger;
        }

        public async Task Handle(ContractCreatedNotification notification, CancellationToken cancellationToken)
        {
            var fCode = notification.contractDto.FacilityCode;
            var eCode = notification.contractDto.EquipmentCode;
            var eq = notification.contractDto.EquipmentQuantity;
            _logger.LogInformation($"Sending contract for background processing. Facility code: {fCode}. Equipment code: {eCode}. Quantity: {eq}");
            await _contractProcessing.EnqueueCreatedContract(notification.contractDto, cancellationToken);
        }
    }

}
