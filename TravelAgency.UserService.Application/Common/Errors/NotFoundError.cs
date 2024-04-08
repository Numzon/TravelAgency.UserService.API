using Microsoft.AspNetCore.Http;

namespace TravelAgency.UserService.Application.Common.Errors;
public sealed class NotFoundError : BaseError
{
    private readonly string? _stringId;
    private readonly int? _intId;

    public override string? Message => $"Element with Id: {_stringId ?? _intId?.ToString() ?? string.Empty} not found";

    public NotFoundError(int id)
    {
        _intId = id;
    }

    public NotFoundError(string id)
    {
        _stringId = id;
    }

    public override IResult GetErrorResult()
    {
        return Results.NotFound(this);
    }
}
 