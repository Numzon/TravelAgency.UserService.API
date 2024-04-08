using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.User.Commands.DeleteUser;
public sealed record DeleteUserCommand(string Id) : IResultRequest;

public sealed class DeleteUserCommandHandler : IResultRequestHandler<DeleteUserCommand>
{
    private readonly IAmazonCognitoService _amazonService;

    public DeleteUserCommandHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _amazonService.GetSimpleUserByIdAsync(request.Id, cancellationToken);

            if (user == null)
            {
                return CustomErrors.NotFound(request.Id);
            }

            await _amazonService.DeleteUserAsync(user.Email, cancellationToken);
            return CustomResults.NoContent();
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
