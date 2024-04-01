using MediatR;
using Serilog;

namespace TravelAgency.UserService.Application.Common.Behaviors;
public sealed class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
		try
		{
			return await next();
		}
		catch (Exception ex)
		{
			var requestName = typeof(TRequest).Name;

			Log.Error(ex, $"Unhandled exception for request {requestName} {request}");

			throw;
		}
    }
}
