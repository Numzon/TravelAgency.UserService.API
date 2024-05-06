using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.UserService.Application.Authentication.Commands.ChangeEmail;
using TravelAgency.UserService.Application.Authentication.Commands.ChangeUserAttributes;
using TravelAgency.UserService.Application.Authentication.Commands.ConfirmChangeEmail;
using TravelAgency.UserService.Application.User.Commands.ConfirmUserCreation;
using TravelAgency.UserService.Application.User.Commands.CreateClientAccount;
using TravelAgency.UserService.Application.User.Commands.CreateTravelAgency;
using TravelAgency.UserService.Application.User.Commands.DeleteUser;
using TravelAgency.UserService.Application.User.Queries.GetUser;
using TravelAgency.UserService.Application.User.Queries.GetUserByEmail;

namespace TravelAgency.UserService.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("travel-agencies")]
    public async Task<IResult> CreateAsync([FromBody] CreateTravelAgencyCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpPost("clients")]
    public async Task<IResult> CreateAsync([FromBody] CreateClientAccountCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpPost("confirm-user-creation")]
    public async Task<IResult> ConfirmUserCreationAsync([FromBody]ConfirmUserCreationCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();  
    }

    [HttpGet]
    public async Task<IResult> GetAsync([FromQuery]string id)
    {
        var result = await _sender.Send(new GetUserQuery(id));

        return result.GetResult();
    }

    //only for manager and travelAgency
    [HttpGet("validate-email")]
    public async Task<IResult> GetUserByEmailAsync([FromQuery] string email)
    {
        var result = await _sender.Send(new GetUserByEmailQuery(email));

        return result.GetResult();
    }

    [HttpDelete]
    public async Task<IResult> DeleteAsync([FromQuery]string id)
    {
        var result = await _sender.Send(new DeleteUserCommand(id));

        return result.GetResult();
    }

    [HttpPut("change-email")]
    public async Task<IResult> ChangeEmailAsync([FromBody] ChangeEmailCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpPost("confirm-change-email")]
    public async Task<IResult> ConfirmChangeEmailAsync([FromBody] ConfirmChangeEmailCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpPut("change-user-data")]
    public async Task<IResult> ChangeUserAttributesAsync([FromBody]ChangeUserAttributesCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }
}
