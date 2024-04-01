using MediatR;
using Microsoft.AspNetCore.Http;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.Common.Interfaces;

public interface IResultRequestHandler<TInput> : IRequestHandler<TInput, CustomResult> where TInput : IResultRequest
{

}