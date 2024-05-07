using Amazon.CognitoIdentityProvider.Model;
using AutoMapper;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Application.User.Models;
using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.Domain.Enums;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.Common.Profiles;
public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserType, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Attributes.Single(a => a.Name == CognitoAttributes.Id).Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Attributes.Single(a => a.Name == CognitoAttributes.Email).Value))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => Convert.ToBoolean(src.Attributes.Single(a => a.Name == CognitoAttributes.EmailVerified).Value)))
            .ForMember(dest => dest.IsDisabled, opt => opt.MapFrom(src => !src.Enabled));

        CreateMap<UserType, SimpleUserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Attributes.Single(a => a.Name == CognitoAttributes.Id).Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Attributes.Single(a => a.Name == CognitoAttributes.Email).Value));

        CreateMap<ClientUserCreatedEvent, ClientAccount>();

        CreateMap<TravelAgencyUserCreatedEvent, TravelAgencyCreatedPublishedDto>();

        CreateMap<ManagerCreatedPublishedDto, CreateManagerDto>();

        CreateMap<SimpleUserDto, UserForEmployeeCreatedPublishedDto>()
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.Id));
    }
}