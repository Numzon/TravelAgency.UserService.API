using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.User.Queries.GetUser;
public sealed record GetUserQuery(string Id) : IResultRequest;

public sealed class GetUserQueryHandler : IResultRequestHandler<GetUserQuery>
{
    private readonly IAmazonCognitoService _amazonService;

    public GetUserQueryHandler(IAmazonCognitoService amazonService)
    {
        _amazonService = amazonService;
    }

    public async Task<CustomResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _amazonService.GetUserByIdAsync(request.Id, cancellationToken);
            if (user == null)
            {
                return CustomErrors.NotFound(request.Id);
            }

            return CustomResults.Ok(user);
        }
        catch (Exception ex)
        {
            return CustomErrors.BadRequest(ex.Message);
        }
    }
}
