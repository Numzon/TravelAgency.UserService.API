namespace TravelAgency.UserService.Application.Common.Errors;
public sealed class RepositoryError : BaseError
{
    private readonly string _methodName;
    public override string? Message => $"Database error, Method Name: {_methodName}";

    public RepositoryError(string methodName)
    {
        _methodName = methodName;
    }
}