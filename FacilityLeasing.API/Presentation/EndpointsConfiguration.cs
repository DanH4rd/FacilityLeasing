using FacilityLeasing.API.Abstract;
using FacilityLeasing.API.Models;
using FluentValidation;
using MediatR;

namespace FacilityLeasing.API.Presentation
{
    public static class EndpointsConfiguration
    {
        public static IEndpointRouteBuilder ConfigureRoutes(this IEndpointRouteBuilder endpoints)
        {
            // welcome
            endpoints.MapGet("/", () => "Welcome to Facility Leasing API!").WithName("Welcome");


            // get all contracts            
            endpoints.MapGet("/contracts",
            async (IMediator mediator,
                    CancellationToken cancellationToken) =>
            {
                var contracts = await mediator.Send(new GetContractsQuery(), cancellationToken);
                return Results.Ok(contracts);
            })
           .AddEndpointFilter<FLAuthorizationFilter>()
           .WithName("GetContracts")
           .WithDescription("Gets all current contracts.");


            // create contract
            endpoints.MapPost("/contracts",
            async (PlacementContractDTO contractDto, IContractRepository repo,
                  IValidator<CreateContractCommand> validator,
                  IMediator mediator,
                  CancellationToken cancellationToken) =>
            {
                var command = new CreateContractCommand(contractDto);
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var (createdContract, error) = await mediator.Send(command, cancellationToken);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    return Results.BadRequest(error);
                }

                await mediator.Publish(new ContractCreatedNotification(contractDto)); // send for background processing

                return Results.Created($"/contracts/{createdContract.Id}", createdContract);
            })
            .AddEndpointFilter<FLAuthorizationFilter>()
            .WithName("CreateContract")
            .WithDescription("Creates a new contract.");


            return endpoints;
        }
    }
}
