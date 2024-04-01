using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.NotificationTypes.Commands.DeleteNotificationType;
public sealed record DeleteNotificationTypeCommand(int Id) : IResultRequest;

public sealed class DeleteNotificationTypeCommandHandler : IResultRequestHandler<DeleteNotificationTypeCommand>
{
    private readonly INotificationTypeRepository _repository;

    public DeleteNotificationTypeCommandHandler(INotificationTypeRepository repository)
    { 
        _repository = repository;
    }

    public async Task<CustomResult> Handle(DeleteNotificationTypeCommand request, CancellationToken cancellationToken)
    {
        var type = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (type is null)
        {
            return CustomErrors.NotFound(request.Id);
        }

        await _repository.DeleteAsync(type, cancellationToken);

        return CustomResults.NoContent();
    }
}
