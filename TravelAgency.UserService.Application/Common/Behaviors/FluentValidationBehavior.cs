using FluentValidation;
using MediatR;
using TravelAgency.UserService.Application.Common.Exceptions;
using TravelAgency.UserService.Application.Common.Errors;

namespace TravelAgency.UserService.Application.Common.Behaviors;
public sealed class FluentValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public FluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (!failures.Any())
        {
            return await next();
        }


        var response = (TResponse?)Activator.CreateInstance(typeof(TResponse), CustomErrors.Validation(failures));

        if (response == null)
        {
            throw new FluentValidationException(failures);
        }

        return response;
    }

}
