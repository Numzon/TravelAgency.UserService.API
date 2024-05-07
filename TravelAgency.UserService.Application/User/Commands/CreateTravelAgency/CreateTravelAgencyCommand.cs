using Amazon.Runtime;
using MediatR;
using TravelAgency.UserService.Application.Common.Commands;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.User.Commands.CreateTravelAgency;
public sealed record CreateTravelAgencyCommand(string Email, string AgencyName, string Password, string ConfirmPassword) 
    : CreateUserCommand(Email, Password, ConfirmPassword), IResultRequest;

public sealed class CreateTravelAgencyCommandHandler : IResultRequestHandler<CreateTravelAgencyCommand>
{
    private readonly IAmazonCognitoService _amazonService;
    private readonly IPublisher _publisher;

    public CreateTravelAgencyCommandHandler(IAmazonCognitoService service, IPublisher publisher)
    {
        _amazonService = service;
        _publisher = publisher;
    }

    public async Task<CustomResult> Handle(CreateTravelAgencyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await _amazonService.CreateTravelAgencyAsync(request, cancellationToken);

            if (userId == null)
            {
                return CustomErrors.NotFound(request.Email);
            }

            await _publisher.Publish(new TravelAgencyUserCreatedEvent(userId, request.AgencyName), cancellationToken);

            return CustomResults.CreateAtRoute("GetAsync", new { id = userId }, new { Id = userId, Email = request.Email });
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
