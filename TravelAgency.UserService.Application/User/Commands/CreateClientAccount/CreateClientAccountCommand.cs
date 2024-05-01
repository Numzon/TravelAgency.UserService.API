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
            await _amazonService.CreateClientAccountAsync(request, cancellationToken);
            var user = await _amazonService.GetSimpleUserByEmailAsync(request.Email, cancellationToken);

            if (user == null)
            {
                return CustomErrors.NotFound(request.Email);
            }

            await _publisher.Publish(new ClientUserCreatedEvent(user.Id), cancellationToken);

            return CustomResults.CreateAtRoute("GetAsync", new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }

}
