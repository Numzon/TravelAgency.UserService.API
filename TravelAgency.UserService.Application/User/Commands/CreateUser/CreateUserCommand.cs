using MediatR;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.User.Commands.CreateUser;
public sealed record CreateUserCommand(string Email, string? GivenName, string? FamilyName, string? AgencyName, string Password, string ConfirmPassword, bool IsTravelAgency) : IResultRequest;

public sealed class CreateUserCommandHandler : IResultRequestHandler<CreateUserCommand>
{
    private readonly IAmazonCognitoService _amazonService;
    private readonly IPublisher _publisher;

    public CreateUserCommandHandler(IAmazonCognitoService amazonService, IPublisher publisher)
    {
        _amazonService = amazonService;
        _publisher = publisher;
    }

    public async Task<CustomResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _amazonService.CreateUserAsync(request, cancellationToken);
            var user = await _amazonService.GetSimpleUserByEmailAsync(request.Email, cancellationToken);
            
            if (user == null)
            {
                return CustomErrors.NotFound(request.Email);
            }

            if (request.IsTravelAgency is false)
            {
                await _publisher.Publish(new ClientUserCreatedEvent(user.Id));
            }
            else
            {
                //await _publisher.Publish(new ClientUserCreatedEvent(user.Id));TravelAgency :P
            }

            return CustomResults.CreateAtRoute("GetAsync", new {  id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
