using FluentValidation.Results;
using System.Runtime.Serialization;

namespace TravelAgency.UserService.Application.Common.Exceptions;

[Serializable]
public class FluentValidationException : Exception
{
    public FluentValidationException() : this("One or more validation failures have occurred.")
    {
    }

    public FluentValidationException(string message)
    : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public FluentValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = new Dictionary<string, string[]>();
    }

    protected FluentValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public FluentValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }

  
}
