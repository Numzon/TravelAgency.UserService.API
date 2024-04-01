using AutoMapper;
using TravelAgency.UserService.Application.Common.Errors;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Application.NotificationTypes.Models;

namespace TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypeQuery;

public sealed record GetNotificationTypeResponse(NotificationTypeDto type);

public sealed record GetNotificationTypeQuery(int Id) : IResultRequest;

public sealed class GetNotificationTypeQueryHandler : IResultRequestHandler<GetNotificationTypeQuery>
{
    private readonly INotificationTypeRepository _repository;
    private readonly IMapper _mapper;

    public GetNotificationTypeQueryHandler(INotificationTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CustomResult> Handle(GetNotificationTypeQuery request, CancellationToken cancellationToken)
    {
        var type = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (type == null)
        {
            return CustomErrors.NotFound(request.Id);
        }

        var mappedType = _mapper.Map<NotificationTypeDto>(type);

        return CustomResults.Ok(mappedType);
    }
}
