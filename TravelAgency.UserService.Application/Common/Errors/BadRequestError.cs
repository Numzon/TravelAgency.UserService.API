using Microsoft.AspNetCore.Http;

namespace TravelAgency.UserService.Application.Common.Errors;
public class BadRequestError : BaseError
{
    private readonly string? _message;
    private readonly object? _value;

    public virtual object? Value => _value;

    public override string? Message => _message ?? $"The client should not repeat this request without modification";

    public BadRequestError(string message)
    {
        _message = message;
    }

    public BadRequestError(object? value)
    {
        _value = value;
    }

    public override IResult GetErrorResult()
    {
        return Results.NotFound(this);
    }
}
