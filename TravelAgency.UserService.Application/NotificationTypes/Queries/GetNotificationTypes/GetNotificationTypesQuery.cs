using AutoMapper;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Application.NotificationTypes.Models;

namespace TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypesQuery;

public sealed record GetNotificationTypesResponse(ICollection<NotificationTypeDto> NotificationTypes);

public sealed record GetNotificationTypesQuery : FiltersDto, IResultRequest;

public sealed class GetNotificationTypesQueryHandler : IResultRequestHandler<GetNotificationTypesQuery>
{
    private readonly INotificationTypeRepository _repository;
    private readonly IMapper _mapper;

    public GetNotificationTypesQueryHandler(INotificationTypeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CustomResult> Handle(GetNotificationTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await _repository.GetByFitlersAsync(request, cancellationToken);

        var mappedTypes = _mapper.Map<ICollection<NotificationTypeDto>>(types);

        return CustomResults.Ok(mappedTypes);
    }
}

