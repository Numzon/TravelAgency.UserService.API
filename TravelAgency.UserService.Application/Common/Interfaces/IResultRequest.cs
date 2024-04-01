using MediatR;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Common.Interfaces;

public interface IResultRequest : IRequest<CustomResult>
{

}