using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace TravelAgency.UserService.Application.Common.Errors;
public sealed class ValidationError : BaseError
{
    public ICollection<ValidationFailure> Failures { get; init; }
    public override string? Message => Failures.Count == 1 ? "Validation error" : $"{Failures.Count} validation errors";

    public ValidationError(ICollection<ValidationFailure> failures)
    {
        Failures = failures;
    }

    public override IResult GetErrorResult()
    {
        return Results.BadRequest(this);
    }
}
