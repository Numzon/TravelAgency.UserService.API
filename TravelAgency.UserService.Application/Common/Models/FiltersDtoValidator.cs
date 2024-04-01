using FluentValidation;

namespace TravelAgency.UserService.Application.Common.Models;
public abstract class FiltersDtoValidator : AbstractValidator<FiltersDto>
{
    protected FiltersDtoValidator()
    {
    }
}
