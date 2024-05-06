using AutoMapper;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Application.NotificationTypes.Models;

namespace TravelAgency.UserService.Application.NotificationTypes.Commands.CreateNotificationType;

public sealed record CreateNotificationTypeCommand(string Name) : IResultRequest;

public sealed class CreateNotificationTypeCommandHandler : IResultRequestHandler<CreateNotificationTypeCommand>
{
    private readonly INotificationTypeRepository _repository;
    private readonly IMapper _mapper;

    public CreateNotificationTypeCommandHandler(INotificationTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CustomResult> Handle(CreateNotificationTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.CreateAsync(request, cancellationToken);

        if (result == null)
        {
            return CustomErrors.Repository(nameof(_repository.CreateAsync));
        }

        var mappedType = _mapper.Map<NotificationTypeDto>(result);

        return CustomResults.CreateAtRoute("GetAsync", new { id = result.Id }, mappedType);
    }
}
