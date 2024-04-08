using FluentValidation.Results;

namespace TravelAgency.UserService.Application.Common.Errors;
public static class CustomErrors
{
    public static NotFoundError NotFound(int id)
    {
        return new NotFoundError(id);
    }

    public static NotFoundError NotFound(string id)
    {
        return new NotFoundError(id);
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