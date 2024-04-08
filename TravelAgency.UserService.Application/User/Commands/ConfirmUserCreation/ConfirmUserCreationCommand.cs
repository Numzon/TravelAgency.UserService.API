using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.User.Commands.ConfirmUserCreation;
public sealed record ConfirmUserCreationCommand(string Email, string ConfirmationCode) : IResultRequest;

public sealed class ConfirmUserCreationCommandHandler : IResultRequestHandler<ConfirmUserCreationCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public ConfirmUserCreationCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(ConfirmUserCreationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _amazonService.ConfirmUserCreationAsync(request, cancellationToken);
            return CustomResults.NoContent();
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
