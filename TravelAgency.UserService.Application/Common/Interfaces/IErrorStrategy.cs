using Microsoft.AspNetCore.Http;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IErrorStrategy
{
    IResult GetErrorResult();
}
