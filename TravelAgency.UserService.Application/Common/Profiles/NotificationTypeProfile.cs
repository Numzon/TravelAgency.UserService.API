using AutoMapper;
using TravelAgency.UserService.Application.NotificationTypes.Commands.UpdateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Models;
using TravelAgency.UserService.Domain.Entities;

namespace TravelAgency.UserService.Application.Common.Profiles;
public sealed class NotificationTypeProfile : Profile
{
	public NotificationTypeProfile()
	{
		CreateMap<NotificationType, NotificationTypeDto>();
		CreateMap<NotificationTypeDto, NotificationType>();
		CreateMap<UpdateNotificationTypeCommand, NotificationType>();
	}
}
