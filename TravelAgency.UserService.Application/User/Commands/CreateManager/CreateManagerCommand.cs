using MediatR;
using TravelAgency.UserService.Application.Common.Commands;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.User.Commands.CreateManager;
public sealed record CreateManagerCommand(string Email, string FirstName, string LastName, string Password, string ConfirmPassword, int TravelAgencyId, string Group) :
    CreateUserCommand(Email, Password, ConfirmPassword), IResultRequest;

public sealed class CreateManagerCommandHandler : IResultRequestHandler<CreateManagerCommand>
{
    private readonly IAmazonCognitoService _amazonService;
    private readonly IPublisher _publisher;

    public CreateManagerCommandHandler(IAmazonCognitoService amazonService, IPublisher publisher)
    {
        _amazonService = amazonService;
        _publisher = publisher;
    }

    public async Task<CustomResult> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _amazonService.CreateManagerAsync(request, cancellationToken);
            var user = await _amazonService.GetSimpleUserByEmailAsync(request.Email, cancellationToken);

            if (user == null)
            {
                return CustomErrors.NotFound(request.Email);
            }

            await _publisher.Publish(new ManagerUserCreatedEvent(user.Id, request.Email, request.FirstName, request.LastName, request.Group, request.TravelAgencyId), cancellationToken);

            return CustomResults.CreateAtRoute("GetAsync", new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
