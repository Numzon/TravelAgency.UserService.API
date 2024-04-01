using Microsoft.AspNetCore.Http;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;

namespace TravelAgency.UserService.Application.Common.Result;

public class CustomResult : ICustomResult
{
    private readonly BaseError? _error;
    private readonly IResult? _value;

    public CustomResult(IResult value)
    {
        _value = value;
    }

    public CustomResult(BaseError error)
    {
        _error = error;
    }

    public BaseError? Error => _error;
    public bool IsSuccess => _value is not null;
    public object? Value => _value;


    public static implicit operator CustomResult(BaseError error)
    {
        return new CustomResult(error);
    }

    public IResult GetResult()
    {
        if (_error is not null)
        {
            return _error.GetErrorResult();
        }

        if (_value is not null)
        {
            return _value;
        }

        throw new InvalidOperationException("CustomResult doesn't contain value nor error");
    }
}
