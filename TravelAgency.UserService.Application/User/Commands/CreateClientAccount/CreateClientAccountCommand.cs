using MediatR;
using TravelAgency.UserService.Application.Common.Commands;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.User.Commands.CreateClientAccount;

public sealed record CreateClientAccountCommand(string Email, string FirstName, string LastName, string Password, string ConfirmPassword)
    : CreateUserCommand(Email, Password, ConfirmPassword), IResultRequest;

public sealed class CreateClientAccountCommandHandler : IResultRequestHandler<CreateClientAccountCommand>
{
    private readonly IAmazonCognitoService _amazonService;
    private readonly IPublisher _publisher;

    public CreateClientAccountCommandHandler(IAmazonCognitoService amazonService, IPublisher publisher)
    {
        _amazonService = amazonService;
        _publisher = publisher;
    }

    public async Task<CustomResult> Handle(CreateClientAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await _amazonService.CreateClientAccountAsync(request, cancellationToken);

            if (userId == null)
            {
                return CustomErrors.NotFound(request.Email);
            }

            await _publisher.Publish(new ClientUserCreatedEvent(userId, request.Email, request.FirstName, request.LastName), cancellationToken);

            return CustomResults.CreateAtRoute("GetAsync", new { id = userId }, new { Id = userId, Email = request.Email });
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }

}
