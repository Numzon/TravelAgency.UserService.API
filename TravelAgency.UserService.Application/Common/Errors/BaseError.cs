using Microsoft.AspNetCore.Http;
using TravelAgency.UserService.Application.Common.Interfaces;

namespace TravelAgency.UserService.Application.Common.Errors;
public abstract class BaseError : IErrorStrategy
{
    public virtual string? Message { get; }

    public virtual IResult GetErrorResult()
    {
        throw new InvalidOperationException("GetErrorResult is not implemented");
    }
}