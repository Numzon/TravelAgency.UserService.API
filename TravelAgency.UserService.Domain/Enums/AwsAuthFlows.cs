﻿namespace TravelAgency.UserService.Domain.Enums;
public static class AwsAuthFlows
{
    public static readonly string UserPasswordAuth = "USER_PASSWORD_AUTH";
    public static readonly string RefreshTokenAuth = "REFRESH_TOKEN";
    public static readonly string NewPasswordRequiredAuth = "NEW_PASSWORD_REQUIRED";
}
