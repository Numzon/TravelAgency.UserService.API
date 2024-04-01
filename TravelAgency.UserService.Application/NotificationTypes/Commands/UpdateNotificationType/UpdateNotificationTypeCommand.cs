using AutoMapper;
using Microsoft.AspNetCore.Http;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;

namespace TravelAgency.UserService.Application.NotificationTypes.Commands.UpdateNotificationType;

public sealed record UpdateNotificationTypeCommand(int Id, string Name) : IResultRequest;

public sealed class UpdateNotificationTypeCommandHandler : IResultRequestHandler<UpdateNotificationTypeCommand>
{
    private readonly IMapper _mapper;
    private readonly INotificationTypeRepository _repository;

    public UpdateNotificationTypeCommandHandler(IMapper mapper, INotificationTypeRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<CustomResult> Handle(UpdateNotificationTypeCommand request, CancellationToken cancellationToken)
    {
        var type = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (type is null)
        {
            return CustomErrors.NotFound(request.Id);
        }
       
        type = _mapper.Map(request, type);

        await _repository.UpdateAsync(type, cancellationToken);

        return CustomResults.NoContent();
    }
}
