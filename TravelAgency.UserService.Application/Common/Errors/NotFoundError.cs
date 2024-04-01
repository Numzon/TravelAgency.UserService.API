using Microsoft.AspNetCore.Http;

namespace TravelAgency.UserService.Application.Common.Errors;
public sealed class NotFoundError : BaseError
{
    public int Id { get; init; }
    public override string? Message => $"Element with Id: {Id} not found";

    public NotFoundError(int id)
    {
        Id = id;
    }

    public override IResult GetErrorResult()
    {
        return Results.NotFound(this);
    }
}
 