using Microsoft.AspNetCore.Http;

namespace TravelAgency.UserService.Application.Common.Errors;
public sealed class NotFoundError<T> : BaseError
{
    private readonly T? _id;

    public override string? Message => $"Element with Id: {_id?.ToString() ?? string.Empty} not found";

    public NotFoundError(T id)
    {
        _id = id;
    }

    public NotFoundError()
    {

    }


    public override IResult GetErrorResult()
    {
        return Results.NotFound(this);
    }
}
 