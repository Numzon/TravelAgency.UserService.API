using FluentValidation.Results;

namespace TravelAgency.UserService.Application.Common.Errors;
public static class CustomErrors
{
    public static NotFoundError NotFound(int id)
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
}