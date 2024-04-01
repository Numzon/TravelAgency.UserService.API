using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.UserService.Application.Common.Result;
using TravelAgency.UserService.Application.NotificationTypes.Commands.CreateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Commands.DeleteNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Commands.UpdateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypeQuery;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypesQuery;

namespace TravelAgency.UserService.API.Controllers;

[ApiController]
[Route("api/notificationTypes")]
public class NotificationTypeController : Controller
{
    private readonly ISender _sender;

    public NotificationTypeController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IResult> Get([FromQuery] GetNotificationTypesQuery query)
    {
        var result = await _sender.Send(query);

        return result.GetResult();
    }

    [HttpGet("{id}", Name = nameof(Get))]
    public async Task<IResult> Get(int id)
    {
        var result = await _sender.Send(new GetNotificationTypeQuery(id));

        return result.GetResult();
    }

    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateNotificationTypeCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpPut]
    public async Task<IResult> Update([FromBody] UpdateNotificationTypeCommand command)
    {
        var result = await _sender.Send(command);

        return result.GetResult();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(int id)
    {
        var result = await _sender.Send(new DeleteNotificationTypeCommand(id));

        return result.GetResult();
    }
}
