using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Application.User.Models;

namespace TravelAgency.UserService.Application.User.Queries.GetUserByEmail;
public sealed record GetUserByEmailQuery(string Email) : IResultRequest;

public sealed class GetUserByEmailQueryHandler : IResultRequestHandler<GetUserByEmailQuery>
{
    private readonly IAmazonCognitoService _cognitoService;

    public GetUserByEmailQueryHandler(IAmazonCognitoService cognitoService)
	{
        _cognitoService = cognitoService;
    }

    public async Task<CustomResult> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
		try
		{
			var result = await _cognitoService.GetSimpleUserByEmailAsync(request.Email, cancellationToken);

			if (result is null)
			{
				return CustomResults.Ok(new UserExistsDto { Exists = false });
            }

			return CustomResults.Ok(new UserExistsDto { UserId = result.Id, Exists = false });
		}
		catch (Exception ex)
		{
			return CustomErrors.BadRequest(ex.Message);
		}
    }
}
