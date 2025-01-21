using FluentValidation;

namespace FacilityLeasing.API.Abstract
{
    public sealed class CreateContractValidator : AbstractValidator<CreateContractCommand>
    {
        public CreateContractValidator()
        {
            RuleFor(x => x.contractDto.FacilityCode)
                .NotEmpty().WithMessage("Facility Code is required.")
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");

            RuleFor(x => x.contractDto.EquipmentCode)
                .NotEmpty().WithMessage("Equipment Code is required.")
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");

            RuleFor(x => x.contractDto.EquipmentQuantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
            .LessThanOrEqualTo(1000).WithMessage("Quantity must not exceed 1000.");
        }
    }
}
