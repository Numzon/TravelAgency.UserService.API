using Microsoft.AspNetCore.Http;

namespace TravelAgency.UserService.Application.Common.Result;

public static class CustomResults
{
    public static CustomResult NoContent()
    {
        return new CustomResult(Results.NoContent());
    }

    public static CustomResult Ok(object? value = null)
    {
        return new CustomResult(Results.Ok(value));
    }

    public static CustomResult CreateAtRoute(string? routeName = null, object? routeValues = null, object? value = null)
    {
        return new CustomResult(Results.CreatedAtRoute(routeName, routeValues, value));
    }
}