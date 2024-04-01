using TravelAgency.UserService.Application.Common.Errors;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface ICustomResult
{
    public object? Value { get; }
    public BaseError? Error { get; }
    public bool IsSuccess { get; }
}
