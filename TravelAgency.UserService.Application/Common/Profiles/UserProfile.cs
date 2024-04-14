﻿using Amazon.CognitoIdentityProvider.Model;
using AutoMapper;
using TravelAgency.UserService.Application.User.Models;
using TravelAgency.UserService.Domain.Enums;

namespace TravelAgency.UserService.Application.Common.Profiles;
public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserType, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Attributes.Single(a => a.Name == CognitoAttributes.Id).Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Attributes.Single(a => a.Name == CognitoAttributes.Email).Value))
            .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.Attributes.Any(a => a.Name == CognitoAttributes.GivenName) ?
                    src.Attributes.Single(a => a.Name == CognitoAttributes.GivenName).Value : default))
            .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.Attributes.Any(a => a.Name == CognitoAttributes.FamilyName) ?
                    src.Attributes.Single(a => a.Name == CognitoAttributes.FamilyName).Value : default))
            .ForMember(dest => dest.AgencyName, opt => opt.MapFrom(src => src.Attributes.Any(a => a.Name == CognitoAttributes.AgencyName) ?
                    src.Attributes.Single(a => a.Name == CognitoAttributes.AgencyName).Value : default))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => Convert.ToBoolean(src.Attributes.Single(a => a.Name == CognitoAttributes.EmailVerified).Value)))
            .ForMember(dest => dest.IsDisabled, opt => opt.MapFrom(src => !src.Enabled));

        CreateMap<UserType, SimpleUserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Attributes.Single(a => a.Name == CognitoAttributes.Id).Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Attributes.Single(a => a.Name == CognitoAttributes.Email).Value))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => Convert.ToBoolean(src.Attributes.Single(a => a.Name == CognitoAttributes.EmailVerified).Value)))
            .ForMember(dest => dest.IsDisabled, opt => opt.MapFrom(src => !src.Enabled));
    }
}