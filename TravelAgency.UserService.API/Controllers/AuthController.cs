using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.UserService.Application.Authentication.Commands.ChangePassword;
using TravelAgency.UserService.Application.Authentication.Commands.ConfirmForgotPassword;
using TravelAgency.UserService.Application.Authentication.Commands.ForgotPassword;
using TravelAgency.UserService.Application.Authentication.Commands.RefreshToken;
using TravelAgency.UserService.Application.Authentication.Commands.SignIn;

namespace TravelAgency.UserService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("sign-in")]
    public async Task<IResult> SignInAsync([FromBody] SignInCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpPost("change-password")]
    public async Task<IResult> ChangePasswordAsync([FromBody] ChangePasswordCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpPost("forgot-password")]
    public async Task<IResult> ForgotPasswordAsync([FromBody] ForgotPasswordCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpPost("confirm-forgot-password")]
    public async Task<IResult> ConfirmForgotPasswordAsync([FromBody] ConfirmForgotPasswordCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpPost("refresh-token")]
    public async Task<IResult> RefreshTokenAsync([FromBody] RefreshTokenCommand command)
    {
        var result = await _sender.Send(command);    

        return result.GetResult();  
    }

    
}
