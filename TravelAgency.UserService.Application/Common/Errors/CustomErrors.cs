using FluentValidation.Results;

namespace TravelAgency.UserService.Application.Common.Errors;
public static class CustomErrors
{
    public static NotFoundError<int> NotFound(int id)
    {
        return new NotFoundError<int>(id);
    }

    public static NotFoundError<string> NotFound(string id)
    {
        return new NotFoundError<string>(id);
    }

    public static RepositoryError Repository(string methodName)
    {
        return new RepositoryError(methodName);
    }

    public static ValidationError Validation(ICollection<ValidationFailure> failures)
    {
        return new ValidationError(failures);
    }

    public static BadRequestError BadRequest(object? value = null)
    {
        return new BadRequestError(value);
    }

    public static BadRequestError BadRequest(string message)
    {
        return new BadRequestError(message);
    }
}